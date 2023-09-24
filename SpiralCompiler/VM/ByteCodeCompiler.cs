using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.IR;

namespace SpiralCompiler.VM;
public sealed class ByteCodeCompiler
{
    public static ByteCode Compile(Assembly assembly)
    {
        var compiler = new ByteCodeCompiler(assembly);
        compiler.CodeGen();
        return new ByteCode(compiler.byteCode.ToImmutable());
    }

    private readonly Assembly assembly;

    private readonly ImmutableArray<Instruction>.Builder byteCode = ImmutableArray.CreateBuilder<Instruction>();

    private readonly Dictionary<IOperand, int> locals = new();

    private ByteCodeCompiler(Assembly assembly)
    {
        this.assembly = assembly;
    }

    public void CodeGen()
    {
        foreach (var proc in assembly.Procedures.Values)
        {
            locals.Clear();

            var stackallocInstr = new Instruction(OpCode.Stackalloc, new object[] { 1 });
            byteCode.Add(stackallocInstr);

            foreach (var param in proc.Parameters)
            {
                AllocateLocal(param.Value);
            }
            foreach (var block in proc.BasicBlocks)
            {
                foreach (var instr in block.Instructions)
                {
                    byteCode.Add(TranslateInstruction(instr));
                }
            }

            // Patch stackalloc
            stackallocInstr.Operands[0] = locals.Count;
        }
    }

    private VM.Instruction TranslateInstruction(IR.Instruction instruction)
    {
        switch (instruction)
        {
            case LoadInstruction load:
                return new Instruction(OpCode.Load, new object[] { AllocateLocal(load.Target), AllocateLocal(load.Source) });
            case ReturnInstruction ret:
                switch (ret.Value)
                {
                    case Register:
                    case Local:
                        return new Instruction(OpCode.ReturnLocal, new object[] { AllocateLocal(ret.Value) });
                    default: throw new ArgumentOutOfRangeException(nameof(ret.Value));
                }
            default: throw new ArgumentOutOfRangeException(nameof(instruction));
        }
    }

    private int AllocateLocal(IOperand local)
    {
        if (!locals.TryGetValue(local, out var index))
        {
            index = locals.Count;
            locals.Add(local, index);
        }
        return index;
    }
}
