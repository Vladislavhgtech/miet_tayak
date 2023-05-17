using System;
using Lab4_5.Interpreter.Primitives;

namespace Lab4_5.Interpreter;

internal class Assign : IExpression
{
    private Identifier Id;
    private Expression ExpressionVar;

    public Assign(Identifier id, Expression expression)
    {
        Id = id;
        ExpressionVar = expression;
    }

    public int Interpret(Context context)
    {
        context.SetVariable(Id.GetId(), ExpressionVar.Interpret(context));
        return 0;
    }
}
