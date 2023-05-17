using Lab4_5;
namespace Lab4_5.Interpreter.Expressions;

internal class ElseExp : IExpression
{
    private Statement StatementExpression;

    public ElseExp(Statement statement)
    {
        StatementExpression = statement;
    }
    public int Interpret(Context context)
    {
        if (StatementExpression != null)
            return StatementExpression.Interpret(context);
        else
            return 0;
    }
}
