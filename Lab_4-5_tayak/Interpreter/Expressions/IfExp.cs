using Lab4_5.Interfaces;
namespace Lab4_5.Interpreter;

internal class IfExp : IExpression
{
    private BoolExpression BoolExpr;

    public IfExp(BoolExpression boolExpression)
    {
        BoolExpr = boolExpression;
    }

    // Need it?
    public int Interpret(Context context)
    {
        return 0;
    }
}
