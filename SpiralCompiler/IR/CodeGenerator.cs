using System;
using System.Collections.Generic;
using System.Linq;
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
        throw new NotImplementedException();
    }

    private Procedure DefineProcedure(FunctionSymbol function)
    {
        if (assembly.Procedures.TryGetValue(function, out var proc))
        {
            return proc;
        }
        proc = new Procedure();
        assembly.Procedures.Add(function, proc);
        return proc;
    }
}
