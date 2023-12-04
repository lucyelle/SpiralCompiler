using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Syntax;

namespace SpiralCompiler;

public sealed record class ErrorMessage(string Message, Range? Range);
