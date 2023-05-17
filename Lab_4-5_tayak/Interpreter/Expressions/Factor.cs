using Lab4_5.Interpreter.Primitives;
using System;

namespace Lab4_5.Interpreter;

internal class Factor : IExpression
{
    private IExpression ExpressionVar;

    public Factor(Expression expression)
    {
        ExpressionVar = expression;
    }
    public Factor(Identifier identifier)
    {
        ExpressionVar = identifier;
    }
    public Factor(Number number)
    {
        ExpressionVar = number;
    }

    public int Interpret(Context context)
    {
        return ExpressionVar.Interpret(context);
    }
}
