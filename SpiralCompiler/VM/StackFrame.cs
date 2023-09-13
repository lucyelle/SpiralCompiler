using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.VM;

public sealed class StackFrame
{
    public dynamic[] Locals { get; } = new dynamic[0];

    public int ReturnAddress { get; set; }
}
