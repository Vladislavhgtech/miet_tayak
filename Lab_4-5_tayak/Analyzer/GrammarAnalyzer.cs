using System;
using System.Collections.Generic;
using Lab4_5.Interfaces;

namespace Lab4_5.Analyzer;

internal class GrammarAnalyzer : IAnalyzer
{
    private List<string> InputGrammarLines;

    public GrammarAnalyzer(string inputGrammarLines)
    {
        InputGrammarLines = new(inputGrammarLines.Split("\r\n"));
    }

    public List<string> GetInputGrammarLines()
    {
        return InputGrammarLines;
    }

    public bool IsTextCorrect()
    {
        foreach(var line in InputGrammarLines)
        {
            if (line == "") continue;
            if (!IsTextLineCorrect(line)) return false;
        }  
            
      
        return true;
    }

    public bool IsTextLineCorrect(string analyzingLine)
    {
        DeleteSpaces(ref analyzingLine);

        if (!IsDividePartsCorrect(analyzingLine)) return false;
        try
        {
            var leftPart = analyzingLine.Substring(0, analyzingLine.IndexOf(":"));
            var rightPart = analyzingLine.Substring(analyzingLine.IndexOf(":") + 1, analyzingLine.Length - analyzingLine.IndexOf(":") - 1);

            if (!IsLeftPartCorrect(leftPart) ||
                !IsRightPartCorrect(rightPart))
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    private void DeleteSpaces(ref string inpLine)
    {
        inpLine = inpLine.Replace(" ", "");
    }

    private bool IsDividePartsCorrect(string analyzingLine)
    {
        if(!analyzingLine.Contains('|')) return true;
        if (analyzingLine.IndexOf(":") > analyzingLine.IndexOf("|")) return false;
        return true;
    }

    private bool IsLeftPartCorrect(string leftPart)
    {
        if(!leftPart.StartsWith('<') || !leftPart.EndsWith('>')) return false;
        var infix = leftPart.Substring(1, leftPart.Length - 2);
        foreach (var letter in infix)
        {
            if(!(char.IsLetter(letter) || letter == '_')) return false;
        }
        return true;
    }

    private bool IsRightPartCorrect(string rightPart)
    {
        var expressions = rightPart.Split('|');
        foreach(var exp in expressions)
        {
            if(!IsExpressionOfRightPartCorrect(exp)) return false;
        }
        return true;
    }

    private bool IsExpressionOfRightPartCorrect(string expr)
    {
        Stack<char> stack = new();
        for (int i = 0; i < expr.Length; i++)
        {
            try
            {

                if (i == 0 && expr[i] != '<' && expr[i] != '‘')
                    return false;

                if (i == 0 && expr[i] == '<')
                {
                    stack.Push(expr[i]);
                    continue;
                }

                if(expr[i] == '<' && expr[i-1] != '‘')
                {
                    stack.Push(expr[i]);
                }

                if(expr[i] == '‘')
                    stack.Push(expr[i]);

                if (expr[i] == '>' && expr[i - 1] != '‘')
                    stack.Pop();

                if (expr[i] == '’')
                    stack.Pop();

                if (stack.Count > 1) return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("incorrect grammar:" + ex.ToString());
                return false;
            }
        }

        if (stack.Count > 0) return false;

        return true;
    }


}
