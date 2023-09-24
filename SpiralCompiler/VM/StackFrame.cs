using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.VM;

public sealed class StackFrame
{
    public Stack<dynamic> ComputationStack { get; } = new();

    public int ReturnAddress { get; set; }
}
