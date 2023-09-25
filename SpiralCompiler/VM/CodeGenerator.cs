using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.BoundTree;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.VM;

public sealed class CodeGenerator
{
    private readonly ModuleSymbol module;
    private readonly ImmutableArray<Instruction>.Builder byteCode = ImmutableArray.CreateBuilder<Instruction>();
    private readonly Dictionary<ParameterSymbol, int> parameters = new();
    private readonly Dictionary<LocalVariableSymbol, int> locals = new();

    private CodeGenerator(ModuleSymbol module)
    {
        this.module = module;
    }

    public static ByteCode Generate(ModuleSymbol module)
    {
        var generator = new CodeGenerator(module);
        generator.CodeGen();
        return new(generator.byteCode.ToImmutable());
    }

    private void CodeGen()
    {
        foreach (var member in module.Members)
        {
            CodeGen(member);
        }
    }

    private void CodeGen(Symbol member)
    {
        switch (member)
        {
            case SourceFunctionSymbol function:
                CodeGen(function);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(member));
        }
    }

    private void CodeGen(SourceFunctionSymbol function)
    {
        locals.Clear();
        parameters.Clear();
        var stackallocInstr = Instruction(OpCode.Stackalloc, 0);
        foreach (var param in function.Parameters) AllocateParameter(param);
        CodeGen(function.Body);
        // Patch
        stackallocInstr.Operands[0] = locals.Count;
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
            case BoundCallExpression call:
            {
                foreach (var arg in call.Args) CodeGen(arg);
                if (call.Function is OpCodeFunctionSymbol opCode)
                {
                    foreach (var instr in opCode.Instructions) byteCode.Add(instr);
                }
                else
                {
                    // TODO
                    throw new NotImplementedException();
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
