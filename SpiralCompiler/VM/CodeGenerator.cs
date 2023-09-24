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
        CodeGen(function.Body);
    }

    private void CodeGen(BoundStatement statement)
    {
        switch (statement)
        {
            case BoundBlockStatement block:
            {
                foreach (var stmt in block.Statements) CodeGen(stmt);
                break;
            }
            case BoundReturnStatement ret:
            {
                if (ret.Expression is null)
                {
                    Instruction(OpCode.Return_0);
                }
                else
                {
                    CodeGen(ret.Expression);
                    Instruction(OpCode.Return_1);
                }
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
                // TODO
                throw new NotImplementedException();
            }
            case BoundLiteralExpression lit:
            {
                Instruction(OpCode.PushConst, lit.Value);
                break;
            }
            default: throw new ArgumentOutOfRangeException(nameof(expression));
        }
    }

    private Instruction Instruction(OpCode op, params object?[] args)
    {
        var result = new Instruction(op, args);
        byteCode.Add(result);
        return result;
    }
}
