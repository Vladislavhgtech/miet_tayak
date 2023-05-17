using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lab4_5.Interfaces;

namespace Lab4_5.Analyzer
{
    internal class ErrorAnalyzer : IAnalyzer
    {
        private Dictionary<int, List<int>> ErrorPlace;
        public Dictionary<int, List<int>> GetErrorPlace { get { return ErrorPlace; } }

        private List<string> Stack;
        public List<string> GetStack { get { return Stack; } }
        private List<string> Input;
        public List<string> GetInput { get { return Input; } }
        private List<string> Comment;
        public List<string> GetComment { get { return Comment; } }


        public ErrorAnalyzer()
        {
            ErrorPlace = new();
            Input = new();
            Stack = new();
            Comment = new();
        }

        public void AddErrorPlace(int line, int position)
        {
            if(!ErrorPlace.ContainsKey(line))
                ErrorPlace.Add(line, new List<int> { position});
            else
                ErrorPlace[line].Add(position);
        }

        public void FillTableOnce(Stack<string> stack, string input, string comment)
        {
            Input.Add(input);
            Stack.Add(ConvertStackToString(stack));
            Comment.Add(comment);
        }

        private string ConvertStackToString(Stack<string> stack)
        {
            string[] result = new string[stack.Count];
            stack.CopyTo(result, 0);
            StringBuilder sb = new StringBuilder();
            foreach(var item in result)
            {
                sb.Append(item);
            }


            return sb.ToString();
        }


        public bool IsTextCorrect()
        {
            throw new NotImplementedException();
        }

        public bool IsTextLineCorrect(string analyzingLine)
        {
            throw new NotImplementedException();
        }
    }
}
