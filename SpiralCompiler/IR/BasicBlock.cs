using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.IR;

public sealed class BasicBlock
{
    public List<Instruction> Instructions { get; } = new();
}
