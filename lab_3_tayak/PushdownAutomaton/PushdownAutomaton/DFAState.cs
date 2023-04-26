using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushdownAutomaton
{
    internal class DFAState
    {
        //P
        public string symbolOfEntranceTape;
        //Z
        public string symbolOfShop;
        //S
        public string symbolOfControlDevice;
        public List<string> conversions;
        public DFAState()
        {
            conversions = new List<string>();
        }

        public void CreateCommandFirstType(ref List<KeyValuePair<string, string>> regulations)
        {
            symbolOfControlDevice = "s0";
            symbolOfShop = regulations[0].Key;
            symbolOfEntranceTape = "`"; // lambda

            int whatDelete = 0;

            for (int i = 0; i < regulations.Count(); i++)
            {
                string temporaryLeftPart = regulations[0].Key;

                if (temporaryLeftPart.Equals(regulations[i].Key))
                {
                    conversions.Add(regulations[i].Value);
                    whatDelete = i;
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i <= whatDelete; i++)
            {
                regulations.Remove(regulations[0]);
            }
        }

        public void CreateCommandSecondType(ref HashSet<string> P)
        {
            symbolOfControlDevice = "s0";
            symbolOfShop = P.First();
            symbolOfEntranceTape = P.First();
            conversions.Add("`");

            P.Remove(P.First());
        }

        public void CreateFinishCommand()
        {
            symbolOfControlDevice = "s0";
            symbolOfShop = "h0";
            symbolOfEntranceTape = "`";
            conversions.Add("`");
        }
    }
}
