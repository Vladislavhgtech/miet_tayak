namespace Lab4_5.Interpreter;

internal class Print : IExpression
{
    private Expression ExpressionVar;
    private Print PrintVar;

    public Print(Expression expression)
    {
        ExpressionVar = expression;
    }

    public Print(Expression expression, Print print)
    {
        ExpressionVar = expression;
        PrintVar = print;
    }


    public int Interpret(Context context)
    {
         return ExpressionVar.Interpret(context);
    }
}
