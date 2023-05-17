using System;

namespace Lab4_5.Interpreter.Primitives;

internal class Number : IExpression
{
    private int Num;

    public Number(int num)
    {
        Num = num;
    }

    public int Interpret(Context context)
    {
        try
        {
            if(Num == null)
            {
                throw new Exception("Number is empty");   
            }
            else return Num;
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex.Message.ToString());
            return 0;
        }
    }
}
