using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.IR;

public sealed class Procedure
{
    public List<BasicBlock> BasicBlocks { get; } = new();
}
