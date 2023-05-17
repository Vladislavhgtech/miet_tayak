using Lab4_5.Interfaces;
namespace Lab4_5.Interpreter;

internal class BoolExpression : IExpression
{
    private Expression ExpressionFirst;
    private Expression ExpressionSecond;
    private EnumRelop Relop;

    public BoolExpression(Expression expression1, Expression expression2, EnumRelop relop)
    {
        ExpressionFirst = expression1;
        ExpressionSecond = expression2;
        Relop = relop;
    }

    public int Interpret(Context context)
    {
        var firstValue = ExpressionFirst.Interpret(context);
        var secondValue = ExpressionSecond.Interpret(context);

        switch(Relop)
        {
            case EnumRelop.Less:
                return (firstValue < secondValue) ? 1 : 0;
            case EnumRelop.More:
                return (firstValue > secondValue) ? 1 : 0;
            case EnumRelop.Equal:
                return (firstValue == secondValue) ? 1 : 0;
            case EnumRelop.NotEqual:
                return (firstValue != secondValue) ? 1 : 0;
            default:
                System.Console.WriteLine("Incorrect relop");
                return -1;
        }
    }
}
