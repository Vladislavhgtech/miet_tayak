using System;

namespace Lab4_5.Interpreter;

internal class Term : IExpression
{
    private Term TermVar;
    private Expression ExpressionVar;
    private char Operation;

    public Term(Term term, Expression expression, char operation)
    {
        TermVar = term;
        ExpressionVar = expression;
        Operation = operation;
    }

    public Term(Expression expression)
    {
        TermVar = null;
        ExpressionVar = expression;
    }

    public int Interpret(Context context)
    {
        try
        {
            if (TermVar == null)
                return ExpressionVar.Interpret(context);
            else if (Operation == '*')
                return ExpressionVar.Interpret(context) * TermVar.Interpret(context);
            else if (Operation == '/')
                return ExpressionVar.Interpret(context) / TermVar.Interpret(context);
            else
                throw new Exception("Incorrect Operation Exception");

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
        }
        return 0;
    }
}
