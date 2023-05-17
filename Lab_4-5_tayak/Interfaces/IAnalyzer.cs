using System.Collections.Generic;

namespace Lab4_5.Interfaces;

public interface IAnalyzer
{
    public bool IsTextCorrect();
    public bool IsTextLineCorrect(string analyzingLine);
}

