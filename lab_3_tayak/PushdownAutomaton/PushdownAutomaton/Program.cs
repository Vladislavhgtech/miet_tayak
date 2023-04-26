namespace PushdownAutomaton;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Program
{
    static void Main(string[] args)
    {
        HashSet<string> S = new HashSet<string>();
        HashSet<string> Z = new HashSet<string>();
        HashSet<string> P = new HashSet<string>();
        HashSet<string> F = new HashSet<string>();

        string[] textPushdownAutomaton = FileReader.Read();
        CreatingSets(ref S, ref Z, ref P, ref F, textPushdownAutomaton);

        List<KeyValuePair<string, string>> regulations = CreatingRegulations(textPushdownAutomaton);
        List<DFAState> commands = new List<DFAState>();

        while (regulations.Count() != 0)
        {
            commands.Add(new DFAState());
            commands[commands.Count() - 1].CreateCommandFirstType(ref regulations);
        }

        while (P.Count() != 0)
        {
            commands.Add(new DFAState());
            commands[commands.Count() - 1].CreateCommandSecondType(ref P);
        }

        commands.Add(new DFAState());
        commands[commands.Count() - 1].CreateFinishCommand();
        int a = 0;

        DFARun(commands, "!/c-c/", Z);
    }

    public static void CreatingSets(ref HashSet<string> S, ref HashSet<string> Z, ref HashSet<string> P, ref HashSet<string> F, string[] textShopAutomat)
    {
        foreach (string item in textShopAutomat)
        {
            bool isFirstBigger = true;

            foreach (char sym in item)
            {
                if (sym == '\r' || sym == '|')
                {
                    continue;
                }
                else if (sym == '>' && isFirstBigger)
                {
                    isFirstBigger = false;
                    continue;
                }
                else
                {
                    //adding to P
                    if (!char.IsLetter(sym) || char.IsLower(sym))
                    {
                        P.Add(sym.ToString());
                    }
                    //adding to Z
                    Z.Add(sym.ToString());
                }
            }
        }
        Z.Add("h0");
    }

    public static List<KeyValuePair<string, string>> CreatingRegulations(string[] textShopAutomat)
    {
        List<KeyValuePair<string, string>> regulations = new List<KeyValuePair<string, string>>();

        foreach (string item in textShopAutomat)
        {
            string[] temporaryRightRegulations = item.Substring(2).Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string elem in temporaryRightRegulations)
            {
                char[] temporaryReverse = elem.ToCharArray();
                Array.Reverse(temporaryReverse);
                string reverseString = "";

                for (int i = 0; i < temporaryReverse.Count(); i++)
                {
                    reverseString += temporaryReverse[i].ToString();
                }

                reverseString = reverseString.Replace("\r", "");
                regulations.Add(new KeyValuePair<string, string>(item[0].ToString(), reverseString));
            }
        }

        return regulations;
    }

    public static DFAState ComandSearch(List<DFAState> commands, string searchingSymbol)
    {
        foreach (DFAState item in commands)
        {
            if (item.symbolOfShop == searchingSymbol)
            {
                return item;
            }
        }

        return null;
    }

    public static void DFARun(List<DFAState> commands, string checkingStr, HashSet<string> Z)
    {
        List<string> currentState = new List<string>();
        currentState.Add(checkingStr);
        List<Stack<string>> currentsShop = new List<Stack<string>>();

        currentsShop.Add(new Stack<string>());
        currentsShop[0].Push("h0"); currentsShop[0].Push(Z.ElementAt<string>(0));

        bool isEnd;
        int countOfIterations = 0;

        while (countOfIterations < 1e6)
        {
            isEnd = DepthFirstSearch(commands, ref currentsShop, ref currentState);

            if (isEnd)
            {
                break;
            }
            countOfIterations++;
        }

        if (countOfIterations == 1e6)
        {
            Console.WriteLine("Automat not solved");
        }

        Console.ReadKey();
    }

    public static bool DepthFirstSearch(List<DFAState> commands, ref List<Stack<string>> currentsShop, ref List<string> currentsStr)
    {
        DFAState temporaryComand;
        Stack<string> copyStack;
        string copyString;

        List<Stack<string>> copiesShop = new List<Stack<string>>();

        foreach (Stack<string> elem in currentsShop)
        {
            Stack<string> temporaryStack = new Stack<string>(elem);
            copiesShop.Add(new Stack<string>(temporaryStack));
        }

        foreach (Stack<string> item in copiesShop)
        {
            if (item.Peek().ToUpper() == item.Peek() && char.IsLetter(item.Peek()[0]))
            {
                temporaryComand = ComandSearch(commands, item.Peek());

                if (temporaryComand != null)
                {
                    copyStack = new Stack<string>(item);
                    int index = -1;

                    foreach (Stack<string> el in currentsShop)
                    {
                        string[] temporaryCurrShop = el.ToArray<string>();
                        string[] temporaryCopShop = item.ToArray<string>();
                        string str1 = ""; string str2 = "";
                        for (int i = 0; i < temporaryCurrShop.Length; i++)
                        {
                            str1 += temporaryCurrShop[i];
                        }
                        for (int i = 0; i < temporaryCopShop.Length; i++)
                        {
                            str2 += temporaryCopShop[i];
                        }
                        if (str1.Equals(str2))
                        {
                            index = currentsShop.IndexOf(el);
                            break;
                        }
                    }
                    copyString = currentsStr[index];

                    foreach (string str in temporaryComand.conversions)
                    {
                        index = -1;
                        foreach (Stack<string> el in currentsShop)
                        {
                            string[] temporaryCurrShop = el.ToArray<string>();
                            string[] temporaryCopShop = item.ToArray<string>();
                            string str1 = ""; string str2 = "";

                            for (int i = 0; i < temporaryCurrShop.Length; i++)
                            {
                                str1 += temporaryCurrShop[i];
                            }

                            for (int i = 0; i < temporaryCopShop.Length; i++)
                            {
                                str2 += temporaryCopShop[i];
                            }

                            if (str1.Equals(str2))
                            {
                                index = currentsShop.IndexOf(el);
                                break;
                            }
                        }

                        if (temporaryComand.conversions.IndexOf(str) == 0)
                        {
                            item.Pop();
                            foreach (char sym in str)
                            {
                                item.Push(sym.ToString());
                            }

                            currentsShop[index] = new Stack<string>(item);
                            currentsShop[index] = new Stack<string>(currentsShop[index]);
                            continue;
                        }
                        int count = 0;

                        Stack<string> temporaryStack = new Stack<string>(copyStack);
                        temporaryStack.Pop();

                        foreach (char sym in str)
                        {
                            temporaryStack.Push(sym.ToString());
                            count++;
                        }

                        temporaryStack = new Stack<string>(temporaryStack);
                        currentsShop.Add(new Stack<string>(temporaryStack));
                        currentsStr.Add(copyString);

                        for (int i = 0; i < count; i++)
                        {
                            temporaryStack.Pop();
                        }
                    }
                }
                else
                {
                    int index = -1;

                    foreach (Stack<string> el in currentsShop)
                    {
                        string[] temporaryCurrShop = el.ToArray<string>();
                        string[] temporaryCopShop = item.ToArray<string>();
                        string str1 = ""; string str2 = "";

                        for (int i = 0; i < temporaryCurrShop.Length; i++)
                        {
                            str1 += temporaryCurrShop[i];
                        }

                        for (int i = 0; i < temporaryCopShop.Length; i++)
                        {
                            str2 += temporaryCopShop[i];
                        }

                        if (str1.Equals(str2))
                        {
                            index = currentsShop.IndexOf(el);
                        }
                    }

                    currentsStr.RemoveAt(index);
                    currentsShop.RemoveAt(index);
                }
            }
            else
            {
                int index = -1;
                foreach (Stack<string> el in currentsShop)
                {
                    string[] temporaryCurrShop = el.ToArray<string>();
                    string[] temporaryCopShop = item.ToArray<string>();
                    string str1 = ""; string str2 = "";
                    for (int i = 0; i < temporaryCurrShop.Length; i++)
                    {
                        str1 += temporaryCurrShop[i];
                    }
                    for (int i = 0; i < temporaryCopShop.Length; i++)
                    {
                        str2 += temporaryCopShop[i];
                    }
                    if (str1.Equals(str2))
                        index = currentsShop.IndexOf(el);
                }
                if (currentsStr[index].Length != 0 && item.Peek() == currentsStr[index][0].ToString())
                {
                    currentsStr[index] = currentsStr[index].Substring(1);
                    currentsShop[index].Pop();
                }
                else if (currentsStr[index].Length == 0 && currentsShop[index].Pop().Equals("h0"))
                {
                    Console.WriteLine("Automat solved");
                    return true;
                }
                else
                {
                    if (index != -1)
                    {
                        currentsStr.RemoveAt(index);
                        currentsShop.RemoveAt(index);
                    }
                }
            }
        }

        return false;
    }
}