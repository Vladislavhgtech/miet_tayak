using System.Collections.Generic;

namespace Lab4_5.Automat
{
    internal class TransitionFunction
    {

        private readonly string InputSymbol;
        public string GetInputSymbol { get { return InputSymbol; } }

        private readonly string NonTerminal;
        public string GetNonTerminal { get { return NonTerminal; } }


        private readonly List<string> StackOutput = new();
        public List<string> GetStackOutput { get { return StackOutput; } }

        private readonly bool IsSync;
        public bool GetIsSync { get { return IsSync; } }

        public TransitionFunction( string _nonTerm, string _inpSymbol, List<string> _stackOutSymbols)
        {
            InputSymbol = _inpSymbol;
            NonTerminal = _nonTerm;
            StackOutput = _stackOutSymbols;
            IsSync = false;
        }

        public TransitionFunction(string _nonTerm, string _inpSymbol, bool isSync)
        {
            InputSymbol = _inpSymbol;
            NonTerminal = _nonTerm;
            StackOutput = null;
            IsSync = isSync;
        }

    }
}
