using Lab4_5;

namespace Lab4_5.Interpreter.Expressions;

internal class Statement : IExpression
{
    private Statement StatementExpression;
    private Print PrintExpression;
    private Scan ScanExpression;
    private Assign AssignExpression;
    private ForExp ForExpression;
    private IfExp IfExpression;
    private ElseExp ElseExpression;

    public Statement(Statement statement, Print print)
    {
        StatementExpression = statement;
        PrintExpression = print;
    }
    public Statement(Statement statement, Scan scan)
    {
        StatementExpression = statement;
        ScanExpression = scan;
    }
    public Statement(Statement statement, Assign assign)
    {
        StatementExpression = statement;
        AssignExpression = assign;
    }
    public Statement(Statement statement, ForExp forExp)
    {
        StatementExpression = statement;
        ForExpression = forExp;
    }
    public Statement(Statement statement, IfExp ifExp)
    {
        StatementExpression = statement;
        IfExpression = ifExp;
    }
    public Statement(Statement statement, IfExp ifExp, ElseExp elseExp)
    {
        StatementExpression = statement;
        IfExpression = ifExp;
        ElseExpression = elseExp;
    }

    public int Interpret(Context context)
    {
        if(PrintExpression != null)
        {
            PrintExpression.Interpret(context);
            StatementExpression?.Interpret(context);
        }
        else if(ScanExpression != null)
        {
            ScanExpression.Interpret(context);
            StatementExpression?.Interpret(context);
        }
        else if (AssignExpression != null)
        {
            AssignExpression.Interpret(context);
            StatementExpression?.Interpret(context);
        }
        else if (ForExpression != null)
        {
            ForExpression.Interpret(context);
            StatementExpression?.Interpret(context);
        }
        else if (IfExpression != null)
        {
            IfExpression.Interpret(context);
            StatementExpression?.Interpret(context);
            if(ElseExpression != null)
            {
                ElseExpression.Interpret(context);
                StatementExpression?.Interpret(context);
            }
        }
        return 0;
    }
}
