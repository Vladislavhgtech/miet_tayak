using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lab4_5.Interpreter.Primitives;
using Lab4_5.Interpreter;
namespace Lab4_5;

internal class ForExp : IExpression
{
    private Identifier Id;
    private Expression ExpressionStart;
    private Expression ExpressionEnd;

    public ForExp(Identifier id, Expression expressionStart, Expression expressionEnd)
    {
        Id = id;
        ExpressionStart = expressionStart;
        ExpressionEnd = expressionEnd;
    }

    public int Interpret(Context context)
    {
        Assign assign = new(Id,ExpressionStart);
        assign.Interpret(context);

        //

        return 0;
    }
}
