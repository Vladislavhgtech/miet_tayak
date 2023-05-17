using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_5.Interpreter
{
    internal interface IExpression
    {
        int Interpret(Context context);
    }
}
