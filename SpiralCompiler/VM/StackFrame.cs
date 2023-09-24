using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.VM;

public sealed class StackFrame
{
    public dynamic?[] Args { get; set; } = Array.Empty<dynamic?>();
    public dynamic?[] Locals { get; set; } = Array.Empty<dynamic?>();
    public Stack<dynamic?> ComputationStack { get; set; } = new();
    public int ReturnAddress { get; set; }
}
