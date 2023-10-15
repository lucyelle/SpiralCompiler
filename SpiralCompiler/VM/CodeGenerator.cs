using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.BoundTree;
using SpiralCompiler.Symbols;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpiralCompiler.VM;

public sealed class CodeGenerator
{
    private readonly ModuleSymbol module;
    private readonly ImmutableArray<Instruction>.Builder byteCode = ImmutableArray.CreateBuilder<Instruction>();
    private readonly Dictionary<ParameterSymbol, int> parameters = new();
    private readonly Dictionary<LocalVariableSymbol, int> locals = new();
    private readonly Dictionary<FunctionSymbol, int> functionAddresses = new();

    private int CurrentAddress => byteCode.Count;

    private CodeGenerator(ModuleSymbol module)
    {
        this.module = module;
    }

    public static ByteCode Generate(ModuleSymbol module)
    {
        var generator = new CodeGenerator(module);
        generator.CodeGen();
        generator.PatchAddresses();
        return new(generator.byteCode.ToImmutable());
    }

    private void CodeGen()
    {
        foreach (var member in module.Members)
        {
            CodeGen(member);
        }
    }

    private void PatchAddresses()
    {
        foreach (var instr in byteCode)
        {
            for (var i = 0; i < instr.Operands.Length; ++i)
            {
                var op = instr.Operands[i];

                if (op is not FunctionSymbol func) continue;
                if (!functionAddresses.TryGetValue(func, out var addr)) continue;

                // Patch
                instr.Operands[i] = addr;
            }
        }
    }

    private void CodeGen(Symbol member)
    {
        switch (member)
        {
            case SourceFunctionSymbol function:
                CodeGen(function);
                break;
            case SourceClassSymbol cl:
                CodeGen(cl);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(member));
        }
    }

    private void CodeGen(SourceClassSymbol cl)
    {
        // Codegen ctors
        foreach (var ctor in cl.Constructors.Functions)
        {
            switch (ctor)
            {
                case SynthetizedDefaultConstructorSymbol synthetized:
                    CodeGen(synthetized);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    private void CodeGen(SynthetizedDefaultConstructorSymbol ctor)
    {
        functionAddresses.Add(ctor, CurrentAddress);
        locals.Clear();
        parameters.Clear();
        // Return 'this'
        Instruction(OpCode.PushParam, 0);
        Instruction(OpCode.Return);
    }

    private void CodeGen(SourceFunctionSymbol function)
    {
        functionAddresses.Add(function, CurrentAddress);
        locals.Clear();
        parameters.Clear();
        var stackallocInstr = Instruction(OpCode.Stackalloc, 0);
        foreach (var param in function.Parameters) AllocateParameter(param);
        CodeGen(function.Body);
        // Patch
        stackallocInstr.Operands[0] = locals.Count;
        if (function.ReturnType == BuiltInTypeSymbol.Void)
        {
            Instruction(OpCode.PushConst, 0);
            Instruction(OpCode.Return);
        }
    }

    private void CodeGen(BoundStatement statement)
    {
        switch (statement)
        {
            case BoundExpressionStatement expr:
            {
                CodeGen(expr.Expression);
                Instruction(OpCode.Pop);
                break;
            }
            case BoundBlockStatement block:
            {
                foreach (var stmt in block.Statements) CodeGen(stmt);
                break;
            }
            case BoundReturnStatement ret:
            {
                if (ret.Expression is null) Instruction(OpCode.PushConst, null!);
                else CodeGen(ret.Expression);
                Instruction(OpCode.Return);
                break;
            }
            case BoundIfStatement fi:
            {
                CodeGen(fi.Condition);
                Instruction(OpCode.Not);
                var jumpToElse = Instruction(OpCode.JmpIf, 0);
                CodeGen(fi.Then);
                jumpToElse.Operands[0] = CurrentAddress;
                if (fi.Else is not null)
                {
                    var jumpToEnd = Instruction(OpCode.Jmp, 0);
                    CodeGen(fi.Else);
                    jumpToEnd.Operands[0] = CurrentAddress;
                }
                break;
            }
            case BoundWhileStatement wh:
            {
                var startAddress = CurrentAddress;
                CodeGen(wh.Condition);
                Instruction(OpCode.Not);
                var jumpToEnd = Instruction(OpCode.JmpIf, 0);
                CodeGen(wh.Body);
                Instruction(OpCode.Jmp, startAddress);
                jumpToEnd.Operands[0] = CurrentAddress;
                break;
            }
            default: throw new ArgumentOutOfRangeException(nameof(statement));
        }
    }

    private void CodeGen(BoundExpression expression)
    {
        switch (expression)
        {
            case BoundLocalVariableExpression localVar:
            {
                if (localVar.Variable is ParameterSymbol param)
                {
                    // It's a parameter reference
                    Instruction(OpCode.PushParam, AllocateParameter(param));
                }
                else
                {
                    // Other local variable
                    Instruction(OpCode.PushLocal, AllocateLocalVariable(localVar.Variable));
                }
                break;
            }
            case BoundLiteralExpression lit:
            {
                Instruction(OpCode.PushConst, lit.Value);
                break;
            }
            case BoundAssignmentExpression assignment:
            {
                CodeGen(assignment.Source);
                Instruction(OpCode.Dup);
                StoreTo(assignment.Target);
                break;
            }
            case BoundFieldExpression field:
            {
                CodeGen(field.Receiver);
                var fieldIdx = field.Receiver.Type.Members
                    .OfType<FieldSymbol>()
                    .Select((f, i) => (Field: f, Index: i))
                    .First(pair => pair.Field == field.Field)
                    .Index;
                Instruction(OpCode.PushField, fieldIdx);
                break;
            }
            case BoundCallExpression call:
            {
                if (call.Function.IsConstructor)
                {
                    // First instantiate object
                    Instruction(OpCode.NewObj, call.Function.ReturnType);
                    // Args
                    foreach (var arg in call.Args) CodeGen(arg);
                    // One more arg, we include "this"
                    Instruction(OpCode.Call, call.Function, call.Args.Length + 1);
                }
                else
                {
                    foreach (var arg in call.Args) CodeGen(arg);
                    if (call.Function is OpCodeFunctionSymbol opCode)
                    {
                        foreach (var instr in opCode.Instructions) byteCode.Add(instr);
                    }
                    else
                    {
                        // Regular function
                        Instruction(OpCode.Call, call.Function, call.Args.Length);
                    }
                }
                break;
            }
            default: throw new ArgumentOutOfRangeException(nameof(expression));
        }
    }

    private void StoreTo(BoundExpression lvalue)
    {
        switch (lvalue)
        {
            case BoundLocalVariableExpression local:
            {
                if (local.Variable is ParameterSymbol param)
                {
                    Instruction(OpCode.StoreArg, AllocateParameter(param));
                }
                else
                {
                    Instruction(OpCode.StoreLocal, AllocateLocalVariable(local.Variable));
                }
                break;
            }
            default: throw new ArgumentOutOfRangeException(nameof(lvalue));
        }
    }

    private Instruction Instruction(OpCode op, params object?[] args)
    {
        var result = new Instruction(op, args);
        byteCode.Add(result);
        return result;
    }

    private int AllocateLocalVariable(LocalVariableSymbol symbol)
    {
        if (symbol is ParameterSymbol) throw new ArgumentOutOfRangeException(nameof(symbol));

        if (!locals.TryGetValue(symbol, out var index))
        {
            index = locals.Count;
            locals.Add(symbol, index);
        }
        return index;
    }

    private int AllocateParameter(ParameterSymbol symbol)
    {
        if (!parameters.TryGetValue(symbol, out var index))
        {
            index = locals.Count;
            parameters.Add(symbol, index);
        }
        return index;
    }
}
