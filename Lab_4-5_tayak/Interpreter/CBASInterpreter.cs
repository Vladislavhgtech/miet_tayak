using Lab4_5;
using System.Collections.Generic;
using System.Linq;

namespace Lab4_5.Interpreter.Expressions;

internal class CBASInterpreter
{
    private Context CurContext;
    private string Program;
    private List<IExpression> Commands;
    private Dictionary<int, string> KeyWords;
    private Dictionary<int, string> Operands;

    public CBASInterpreter(string program)
    {
        Program = program;
        CurContext = new Context();
        KeyWords = SetKeyWords();
        Operands= SetOperands();
    }

    public void Execute(ref System.Windows.Controls.TextBox textBox)
    {
        int i = 0;
        var program = ParseProgram(Program, ref i,ref textBox);
    }

    private void AnalyzeProgram()
    {

    }

    private Statement ParseProgram(string line, ref int i,ref System.Windows.Controls.TextBox textBox)
    {
        Statement statement = null;
        bool isChangeBrackets = false;
        Stack<string> brackets = new Stack<string>();
        Stack<IExpression> expressions = new Stack<IExpression>();
        Statement? lastStatement;


        textBox.Text = string.Empty;
        while (i < line.Length)
        {
            textBox.Text += GetNextItem(line, ref i) + "\n";
        }

        return statement;
    }

    private string GetNextItem(string line, ref int i)
    {
        string curLine = "";
        for (; i < line.Length; i++)
        {
            curLine += line[i];
            if (line[i] == ' ' || Operands.ContainsValue(line[i].ToString()))
            {
                if (curLine.Length == 1)
                {
                    i++;
                    return curLine;
                }
                else
                {
                    //i--;
                    return curLine.Remove(curLine.Length - 1, 1);
                }
               
            }
        }

        return line;
    }

    private Dictionary<int, string> SetKeyWords()
    {
        Dictionary<int, string> keyWords = new()
        {
            { 1, "if" },
            { 2, "for" },
            { 3, "else" },
            { 4, "print" },
            { 5, "scan" }
        };

        return keyWords;
    }

    private Dictionary<int, string> SetOperands()
    {
        Dictionary<int, string> operands = new()
        {
            { 1, "+"},
            {2, "-"},
            {3, "/"},
            {4,"*"},
            {5,"=" },
            {6,"("},
            {7,")"},
        };

        return operands;
    }
}
