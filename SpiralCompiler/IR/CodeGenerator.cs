using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.BoundTree;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.IR;

public sealed class CodeGenerator
{
    private readonly ModuleSymbol module;

    private readonly Assembly assembly = new();

    private Procedure? currentProcedure;

    private BasicBlock? currentBasicBlock;

    private CodeGenerator(ModuleSymbol module)
    {
        this.module = module;
    }

    public static Assembly Generate(ModuleSymbol module)
    {
        var generator = new CodeGenerator(module);
        generator.CodeGen();
        return generator.assembly;
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
        var proc = DefineProcedure(function);
        currentProcedure = proc;
        currentBasicBlock = proc.BasicBlocks[^1];

        CodeGen(function.Body);
    }

    private void CodeGen(BoundStatement statement)
    {
        switch (statement)
        {
            case BoundBlockStatement block:
                CodeGen(block);
                break;
            case BoundReturnStatement ret:
                CodeGen(ret);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(statement));
        }
    }

    private void CodeGen(BoundBlockStatement block)
    {
        foreach (var statement in block.Statements)
        {
            CodeGen(statement);
        }
    }

    private void CodeGen(BoundReturnStatement ret)
    {
        var value = ret.Expression is null ? null : CodeGen(ret.Expression);
        WriteInstruction(new ReturnInstruction()
        {
            Value = value
        });
    }

    private IOperand CodeGen(BoundExpression expression) => expression switch
    {
        BoundLocalVariableExpression local => CodeGen(local),
        _ => throw new ArgumentOutOfRangeException(nameof(expression))
    };

    private IOperand CodeGen(BoundLocalVariableExpression expression)
    {
        var register = AllocateRegister();
        WriteInstruction(new LoadInstruction()
        {
            Target = register,
            Source = ToOperand(expression.Variable)
        });
        return register;
    }

    private Procedure DefineProcedure(FunctionSymbol function)
    {
        if (assembly.Procedures.TryGetValue(function, out var proc))
        {
            return proc;
        }
        proc = new Procedure();
        var bb = new BasicBlock();
        proc.BasicBlocks.Add(bb);
        assembly.Procedures.Add(function, proc);
        return proc;
    }

    private void WriteInstruction(Instruction instruction)
    {
        if (currentBasicBlock is null)
        {
            throw new InvalidOperationException("no basic block defined");
        }
        currentBasicBlock.Instructions.Add(instruction);
    }

    private Register AllocateRegister() => new();

    private IOperand ToOperand(LocalVariableSymbol symbol) => symbol switch
    {
        ParameterSymbol param => currentProcedure!.DefineParameter(param),
        _ => currentProcedure!.DefineLocal(symbol)
    };
}
