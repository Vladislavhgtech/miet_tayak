using System.Collections.Generic;

namespace Lab4_5.Interpreter;

internal class Context
{
    private Dictionary<string, int> Variables;

    public Context()
    {
        Variables = new();
    }

    public int GetVariable(string name)
    {
        return Variables[name];
    }

    public void SetVariable(string name, int value)
    {
        if(Variables.ContainsKey(name))
            Variables[name] = value;
        else
            Variables.Add(name, value);
    }

    public Dictionary<string, int> GetVariables()
    {
        return Variables;
    }

}
