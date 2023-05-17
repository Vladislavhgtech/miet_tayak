namespace Lab4_5.Interpreter.Primitives;

internal class Identifier : IExpression
{
    private string Id;

    public Identifier(string id)
    {
        Id = id;
    }

    public string GetId()
    {
        return Id;
    }

    public int Interpret(Context context)
    {
        return 0;
    }
}
