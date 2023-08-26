using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.IR;

public interface IOperand
{

}

public sealed class Register : IOperand
{
}

public sealed class Local : IOperand
{

}

public sealed class Parameter : IOperand
{

}
