using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab4_5.Interfaces;

namespace Lab4_5.Automat
{
    internal class AlphabetAnalyzer : IAnalyzer
    {
        private HashSet<string> NonTerminals = new();
        private HashSet<string> Terminals = new();
        private List<string> InputLines = new();

        public HashSet<string> GetNonTerminalsAlphabet()
        {
            return NonTerminals;
        }
        public HashSet<string> GetTerminalsAlphabet()
        {
            return Terminals;
        }

        public AlphabetAnalyzer(string inputLines)
        {
            InputLines = new(inputLines.Split("\r\n"));
 
            IsTextCorrect();
        }

        public bool IsTextCorrect()
        {
            foreach(var line in InputLines)
            {
                IsTextLineCorrect(line);
            }
            return true;
        }

        public bool IsTextLineCorrect(string analyzingLine)
        {
            var leftPart = analyzingLine.Substring(0, analyzingLine.IndexOf(":"));
            var rightPart = analyzingLine.Substring(analyzingLine.IndexOf(":") + 1, analyzingLine.Length - analyzingLine.IndexOf(":") - 1);

            AddLeftPartToAlphabet(leftPart);
            AddRightPartToAlphabet(rightPart);
            return true;
        }

        private void AddLeftPartToAlphabet(string line)
        {
            NonTerminals.Add(line.Substring(1, line.Length - 2));
        }

        private void AddRightPartToAlphabet(string line)
        {
            var splittedLines = line.Split('|');
            foreach(var splittedLine in splittedLines)
            {
                AnalyzeSplittedLineOfRightPart(splittedLine);
            }
        }

        private void AnalyzeSplittedLineOfRightPart(string line)
        {
            string curSymbols = "";
            bool isTerminal = false;
            bool isNonTerminal = false;
            for (int i = 0; i < line.Length; i++)
            {
                if(isTerminal)
                {
                    if(line[i] == '’')
                    {
                        Terminals.Add(curSymbols);
                        curSymbols = "";
                        isNonTerminal = isTerminal = false;
                    }
                    else
                    {
                        curSymbols += line[i];
                    }
                }
                else if (isNonTerminal)
                {
                    if (line[i] == '>')
                    {
                        NonTerminals.Add(curSymbols);
                        curSymbols = "";
                        isNonTerminal = isTerminal = false;
                    }
                    else
                    {
                        curSymbols += line[i];
                    }
                }
                else
                {
                    if(line[i] == '‘')
                        isTerminal = true;
                    else if(line[i] == '<')
                        isNonTerminal = true;
                }
            }
        }


    }
}
