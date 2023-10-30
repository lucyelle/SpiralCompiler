using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Binding;
using System.Xml.Linq;
using SpiralCompiler.BoundTree;
using SpiralCompiler.Symbols;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpiralCompiler.VM;

public sealed class CodeGenerator
{
    private readonly ModuleSymbol module;
    private readonly ImmutableArray<Instruction>.Builder byteCode = ImmutableArray.CreateBuilder<Instruction>();
    private readonly Dictionary<GlobalVariableSymbol, int> globals = new();
    private readonly List<BoundExpression?> globalInitializers = new();
    private readonly SynthetizedGlobalInitializerSymbol globalInitializer = new();
    private readonly Dictionary<ParameterSymbol, int> parameters = new();
    private readonly Dictionary<LocalVariableSymbol, int> locals = new();
    private readonly Dictionary<FunctionSymbol, int> functionAddresses = new();
    private readonly Dictionary<TypeSymbol, TypeInfo> typeInfos = new();
    private int paramsOffset = 0;

    private int CurrentAddress => byteCode.Count;

    private CodeGenerator(ModuleSymbol module)
    {
        this.module = module;
    }

    public static ByteCode Generate(ModuleSymbol module)
    {
        var generator = new CodeGenerator(module);
        generator.CodeGen();
        generator.CodegenGlobalInitializer();
        generator.PatchAddresses();
        generator.PatchTypeInfos();
        return new(
            generator.globalInitializers.Count,
            generator.functionAddresses.ToImmutableDictionary(kv => kv.Value, kv => kv.Key),
            generator.byteCode.ToImmutable());
    }

    private void CodeGen()
    {
        foreach (var member in module.Members)
        {
            CodeGen(member);
        }
    }

    private void CodegenGlobalInitializer()
    {
        functionAddresses.Add(globalInitializer, CurrentAddress);
        foreach (var (index, value) in globalInitializers.Select((val, idx) => (idx, val)))
        {
            if (value is null) continue;
            CodeGen(value);
            Instruction(OpCode.StoreGlobal, index);
        }
        Instruction(OpCode.PushConst, 0);
        Instruction(OpCode.Return);
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

    private void PatchTypeInfos()
    {
        foreach (var instr in byteCode)
        {
            for (var i = 0; i < instr.Operands.Length; ++i)
            {
                var op = instr.Operands[i];

                if (op is not TypeSymbol type) continue;
                var typeInfo = GetTypeInfo(type);

                // Patch
                instr.Operands[i] = typeInfo;
            }
        }
    }

    private TypeInfo GetTypeInfo(TypeSymbol typeSymbol)
    {
        if (!typeInfos.TryGetValue(typeSymbol, out var typeInfo))
        {
            var vtables = typeSymbol.BaseTypes
                .OfType<InterfaceSymbol>()
                .Select(i => BuildVtable(typeSymbol, i))
                .ToImmutableArray();
            var defaultFieldValues = typeSymbol.Members
                .OfType<FieldSymbol>()
                .Select(f => GetDefaultValue(f.Type))
                .ToImmutableArray();
            typeInfo = new(typeSymbol, vtables, defaultFieldValues);
        }
        return typeInfo;
    }

    private static dynamic? GetDefaultValue(TypeSymbol type)
    {
        if (type == BuiltInTypeSymbol.Int) return 0;
        if (type == BuiltInTypeSymbol.Bool) return false;
        return null;
    }

    private VTable BuildVtable(TypeSymbol type, InterfaceSymbol iface) =>
        new(iface, type.Members
            .OfType<FunctionSymbol>()
            .ToImmutableDictionary(f => f.BaseDeclaration, f => functionAddresses[f]));

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
            case SourceGlobalVariableSymbol glob:
                CodeGen(glob);
                break;
            case SourceInterfaceSymbol iface:
                break;
            default: throw new ArgumentOutOfRangeException(nameof(member));
        }
    }

    private void CodeGen(GlobalVariableSymbol global)
    {
        globalInitializers.Add(global.InitialValue);
    }

    private void CodeGen(SourceClassSymbol cl)
    {
        // Codegen ctors
        foreach (var ctor in cl.Members)
        {
            switch (ctor)
            {
                case SourceFunctionSymbol func:
                    CodeGen(func);
                    break;
                case SynthetizedDefaultConstructorSymbol synthetized:
                    CodeGen(synthetized);
                    break;
                case SourceConstructorSymbol source:
                    CodeGen(source);
                    break;
                case FieldSymbol:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    private void FunctionPreamble(FunctionSymbol symbol)
    {
        functionAddresses.Add(symbol, CurrentAddress);
        locals.Clear();
        parameters.Clear();
        paramsOffset = symbol.IsInstance ? 1 : 0;
    }

    private void CodeGen(SynthetizedDefaultConstructorSymbol ctor)
    {
        FunctionPreamble(ctor);
        // Return 'this'
        Instruction(OpCode.PushParam, 0);
        Instruction(OpCode.Return);
    }

    private void CodeGen(SourceConstructorSymbol ctor)
    {
        FunctionPreamble(ctor);
        var stackallocInstr = Instruction(OpCode.Stackalloc, 0);
        foreach (var param in ctor.Parameters) AllocateParameter(param);
        CodeGen(ctor.Body);
        // Patch
        stackallocInstr.Operands[0] = locals.Count;
        // Return 'this'
        Instruction(OpCode.PushParam, 0);
        Instruction(OpCode.Return);
    }

    private void CodeGen(SourceFunctionSymbol function)
    {
        FunctionPreamble(function);
        var stackallocInstr = Instruction(OpCode.Stackalloc, 0);
        foreach (var param in function.Parameters) AllocateParameter(param);
        // Main starts by calling global initializer
        if (function.Name == "main") Instruction(OpCode.Call, globalInitializer, 0);
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
            case BoundGlobalVariableExpression globalVar:
            {
                Instruction(OpCode.PushGlobal, AllocateGlobalVariable(globalVar.Variable));
                break;
            }
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
                if (field.Receiver is null)
                {
                    // Implicit access, push this
                    Instruction(OpCode.PushParam, 0);
                }
                else
                {
                    CodeGen(field.Receiver);
                }
                var fieldIdx = field.Field.ContainingSymbol!.Members
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
            case BoundMemberCallExpression mcall:
            {
                CodeGen(mcall.Receiver);
                foreach (var arg in mcall.Args) CodeGen(arg);
                Instruction(
                    mcall.Function.IsVirtual ? OpCode.CallVirt : OpCode.Call,
                    mcall.Function.BaseDeclaration,
                    mcall.Args.Length + 1);
                break;
            }
            case BoundElementAtExpression elementAt:
            {
                CodeGen(elementAt.Array);
                CodeGen(elementAt.Index);
                Instruction(OpCode.ElementAt, true);
                break;
            }
            default: throw new ArgumentOutOfRangeException(nameof(expression));
        }
    }

    private void StoreTo(BoundExpression lvalue)
    {
        switch (lvalue)
        {
            case BoundGlobalVariableExpression global:
            {
                Instruction(OpCode.StoreGlobal, AllocateGlobalVariable(global.Variable));
                break;
            }
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
            case BoundFieldExpression field:
            {
                if (field.Receiver is null)
                {
                    // Implicit access, push this
                    Instruction(OpCode.PushParam, 0);
                }
                else
                {
                    CodeGen(field.Receiver);
                }
                var fieldIdx = field.Field.ContainingSymbol!.Members
                    .OfType<FieldSymbol>()
                    .Select((f, i) => (Field: f, Index: i))
                    .First(pair => pair.Field == field.Field)
                    .Index;
                Instruction(OpCode.StoreField, fieldIdx);
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

    private int AllocateGlobalVariable(GlobalVariableSymbol symbol)
    {
        if (!globals.TryGetValue(symbol, out var index))
        {
            index = globals.Count;
            globals.Add(symbol, index);
        }
        return index;
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
            index = parameters.Count + paramsOffset;
            parameters.Add(symbol, index);
        }
        return index;
    }
}
