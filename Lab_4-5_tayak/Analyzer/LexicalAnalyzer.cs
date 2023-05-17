using System;

namespace Lab4_5.Analyzer;

internal class LexicalAnalyzer
{
    private string InputStr;

    public LexicalAnalyzer(string inp)
    {
        InputStr = inp;
    }

    public string DeleteDuplicatedSpaces()
    {
        string removeSpaces = string.Join(" ", InputStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        return removeSpaces;
    }

    public string DeleteEndl()
    {
        string removeEndl = InputStr.Replace("\r\n", "");
        return removeEndl;
    }

    public string Analyze()
    {
        InputStr = DeleteEndl();
        InputStr = DeleteDuplicatedSpaces();
        return InputStr;
    }
}
