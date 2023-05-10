namespace SyntaxParser;

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private static KeyValuePair<(string A, string a), string> temporaryPair;

    static void Main(string[] args)
    {
        string[]? textOrders = FileReader.Read();

        List<Order> orders = CreateOrders(textOrders, true);
        List<Order> ordersForConversion = CreateOrders(textOrders, false);

        Dictionary<string, HashSet<string>> Start = new Dictionary<string, HashSet<string>>();
        Dictionary<string, HashSet<string>> Next = new();

        List<Conversion> allConversions = new List<Conversion>();
        List<string> terminals = new List<string>();

        List<KeyValuePair<(string A, string a), string>> predictionAnalisisTable = new List<KeyValuePair<(string A, string a), string>>();

        foreach (Order item in orders)
        {
            Start.Add(item.LEFT, new HashSet<string>());
            Next.Add(item.LEFT, new HashSet<string>());

            if (item.LEFT.Equals("<program>"))
            {
                Next[item.LEFT].Add("$");
            }

            foreach (string str in item.conversions)
            {
                if (str[0] == '\'')
                {
                    terminals.Add(str);
                }
            }

        }

        foreach (Order item in ordersForConversion)
        {
            foreach (string str in item.conversions)
            {
                allConversions.Add(new Conversion(item.LEFT, str));
            }
        }

        CreateStart(ref Start, allConversions);
        CreateNext(Start, ref Next, allConversions);

        FillingTable(Start, Next, allConversions, ref predictionAnalisisTable, orders);

        Run(predictionAnalisisTable, "int checker () { if ( a > 12 ) { int w = a ; } }", terminals, orders, allConversions);

        Run(predictionAnalisisTable, "int main () { for ( qwerty a = 0 ; a < b ; ) { asd w = a ; } }", terminals, orders, allConversions);

        Run(predictionAnalisisTable, "int main () { return 0 ; }", terminals, orders, allConversions);
    }

    static void CreateStart(ref Dictionary<string, HashSet<string>> Start, List<Conversion> allConversions)
    {
        bool modification = true;

        while (modification)
        {
            modification = false;

            foreach (Conversion conversion in allConversions)
            {
                int temporarySize = Start[conversion.LEFT].Count();
                if (conversion.RIGHT[0] == '<')
                {
                    int index = conversion.RIGHT.IndexOf('>') + 1;

                    if (index < conversion.RIGHT.Length && conversion.RIGHT[index] == '<')
                    {
                        if (Start[conversion.RIGHT.Substring(0, index)].Contains("$"))
                        {
                            foreach (string str in Start[conversion.RIGHT.Substring(0, index)])
                            {
                                Start[conversion.LEFT].Add(str);
                            }

                            foreach (string str in Start[conversion.RIGHT.Substring(index)])
                            {
                                Start[conversion.LEFT].Add(str);
                            }
                        }
                        else
                        {
                            foreach (string str in Start[conversion.RIGHT.Substring(0, index)])
                            {
                                Start[conversion.LEFT].Add(str);
                            }
                        }
                    }
                    else
                    {
                        foreach (string str in Start[conversion.RIGHT.Substring(0, index)])
                        {
                            Start[conversion.LEFT].Add(str);
                        }
                    }
                }
                else if (conversion.RIGHT[0] == '\'')
                {
                    int index = conversion.RIGHT.Substring(1).IndexOf('\'') + 1;

                    Start[conversion.LEFT].Add(conversion.RIGHT.Substring(0, index + 1));
                }
                else if (conversion.RIGHT[0] == '$')
                {
                    Start[conversion.LEFT].Add("$");
                }

                if (temporarySize != Start[conversion.LEFT].Count())
                {
                    modification = true;
                }
            }
        }
    }

    static List<Order> CreateOrders(string[] textOrders, bool flag)
    {
        List<Order> orders = new List<Order>();
        foreach (string str in textOrders)
        {
            string LEFT = str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];

            if (LEFT[LEFT.Length - 1].Equals('\r'))
                LEFT = LEFT.Remove(LEFT.Length - 1);

            string RIGHT = str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1];

            if (RIGHT[RIGHT.Length - 1].Equals('\r'))
                RIGHT = RIGHT.Remove(RIGHT.Length - 1);

            string[] conversions = RIGHT.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            orders.Add(new Order(LEFT, conversions, flag));
        }

        return orders;
    }

    static void CreateNext(Dictionary<string, HashSet<string>> Start, ref Dictionary<string, HashSet<string>> Next, List<Conversion> allConversions)
    {
        bool modification = true;

        while (modification)
        {
            modification = false;

            foreach (Conversion conversion in allConversions)
            {
                int LEFTIndex = conversion.RIGHT.IndexOf('<');

                if (LEFTIndex == -1 || conversion.RIGHT.Contains("'>'") || conversion.RIGHT.Contains("'<'"))
                {
                    continue;
                }
                else
                {
                    int RIGHTIndex = conversion.RIGHT.IndexOf('>');

                    if (RIGHTIndex == conversion.RIGHT.Length - 1)
                    {
                        int temporarySize = Next[conversion.RIGHT.Substring(LEFTIndex)].Count();
                        foreach (string str in Next[conversion.LEFT])
                        {
                            Next[conversion.RIGHT.Substring(LEFTIndex)].Add(str);
                        }
                        if (temporarySize != Next[conversion.RIGHT.Substring(LEFTIndex)].Count())
                            modification = true;
                    }
                    else
                    {
                        string LEFTPart = conversion.RIGHT.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 1);
                        string temporaryString = conversion.RIGHT.Substring(RIGHTIndex + 1);
                        LEFTIndex = 0;
                        if (temporaryString[LEFTIndex].Equals('<'))
                        {
                            RIGHTIndex = temporaryString.IndexOf('>');

                            string nonTerminal = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 1);
                            int temporarySize = Next[LEFTPart].Count();

                            bool isNext = false;

                            foreach (string str in Start[nonTerminal])
                            {
                                if (str != "$")
                                {
                                    Next[LEFTPart].Add(str);
                                }
                                else
                                {
                                    isNext = true;
                                }
                            }

                            if (isNext)
                            {
                                foreach (string str in Next[conversion.LEFT])
                                {
                                    Next[LEFTPart].Add(str);
                                }
                            }

                            if (temporarySize != Next[LEFTPart].Count())
                            {
                                modification = true;
                            }

                            while (true)
                            {
                                LEFTIndex = temporaryString.IndexOf('<');
                                RIGHTIndex = temporaryString.IndexOf('>');

                                if (LEFTIndex == -1)
                                {
                                    break;
                                }
                                LEFTPart = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 1);
                                temporaryString = temporaryString.Substring(RIGHTIndex + 1);
                                LEFTIndex = 0;

                                if (temporaryString == "")
                                {
                                    temporarySize = Next[LEFTPart].Count();
                                    foreach (string str in Next[conversion.LEFT])
                                    {
                                        Next[LEFTPart].Add(str);
                                    }
                                    if (temporarySize != Next[LEFTPart].Count())
                                    {
                                        modification = true;
                                    }
                                }
                                else if (temporaryString[LEFTIndex].Equals('<'))
                                {
                                    RIGHTIndex = temporaryString.IndexOf('>');
                                    nonTerminal = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 1);
                                    temporarySize = Next[LEFTPart].Count();

                                    isNext = false;

                                    foreach (string str in Start[nonTerminal])
                                    {
                                        if (str != "$")
                                        {
                                            Next[LEFTPart].Add(str);
                                        }
                                        else
                                        {
                                            isNext = true;
                                        }
                                    }

                                    if (isNext)
                                    {
                                        foreach (string str in Next[conversion.LEFT])
                                        {
                                            Next[LEFTPart].Add(str);
                                        }
                                    }

                                    if (temporarySize != Next[LEFTPart].Count())
                                    {
                                        modification = true;
                                    }
                                }
                                else
                                {
                                    RIGHTIndex = temporaryString.Substring(1).IndexOf("\'");
                                    nonTerminal = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 2);

                                    Next[LEFTPart].Add(nonTerminal);
                                }
                            }
                        }
                        else
                        {
                            LEFTIndex = temporaryString.IndexOf('\'');
                            if (LEFTIndex != -1)
                            {
                                RIGHTIndex = temporaryString.Substring(1).IndexOf("\'");
                                int temporarySize = Next[LEFTPart].Count();

                                string nonTerminal = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 2);

                                Next[LEFTPart].Add(nonTerminal);

                                if (temporarySize != Next[LEFTPart].Count())
                                {
                                    modification = true;
                                }

                                while (true)
                                {
                                    LEFTIndex = temporaryString.IndexOf('<');
                                    RIGHTIndex = temporaryString.IndexOf('>');

                                    if (LEFTIndex == -1)
                                    {
                                        break;
                                    }

                                    LEFTPart = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 1);
                                    temporaryString = temporaryString.Substring(RIGHTIndex + 1);
                                    LEFTIndex = 0;

                                    if (temporaryString[LEFTIndex].Equals('<'))
                                    {
                                        RIGHTIndex = temporaryString.IndexOf('>');
                                        nonTerminal = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 1);
                                        temporarySize = Next[LEFTPart].Count();

                                        bool isNext = false;

                                        foreach (string str in Start[nonTerminal])
                                        {
                                            if (str != "$")
                                            {
                                                Next[LEFTPart].Add(str);
                                            }
                                            else
                                            {
                                                isNext = true;
                                            }

                                        }
                                        if (isNext)
                                        {
                                            foreach (string str in Next[conversion.LEFT])
                                            {
                                                Next[LEFTPart].Add(str);
                                            }
                                        }
                                        if (temporarySize != Next[LEFTPart].Count())
                                        {
                                            modification = true;
                                        }
                                    }
                                    else
                                    {
                                        RIGHTIndex = temporaryString.Substring(1).IndexOf("\'");
                                        nonTerminal = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 2);

                                        Next[LEFTPart].Add(nonTerminal);
                                    }
                                }
                            }
                            else if (temporaryString.IndexOf(' ') != -1)
                            {
                                int temporarySize = Next[LEFTPart].Count();
                                string nonTerminal = " ";

                                Next[LEFTPart].Add(nonTerminal);

                                if (temporarySize != Next[LEFTPart].Count())
                                {
                                    modification = true;
                                }

                                while (true)
                                {
                                    LEFTIndex = temporaryString.IndexOf('<');
                                    RIGHTIndex = temporaryString.IndexOf('>');

                                    if (LEFTIndex == -1)
                                    {
                                        break;
                                    }
                                    LEFTPart = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 1);
                                    temporaryString = temporaryString.Substring(RIGHTIndex + 1);
                                    LEFTIndex = 0;

                                    if (temporaryString == "")
                                    {
                                        temporarySize = Next[LEFTPart].Count();

                                        foreach (string str in Next[conversion.LEFT])
                                        {
                                            Next[LEFTPart].Add(str);
                                        }

                                        if (temporarySize != Next[LEFTPart].Count())
                                        {
                                            modification = true;
                                        }
                                    }
                                    else if (temporaryString[LEFTIndex].Equals('<'))
                                    {
                                        RIGHTIndex = temporaryString.IndexOf('>');
                                        nonTerminal = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 1);
                                        temporarySize = Next[LEFTPart].Count();

                                        bool isNext = false;

                                        foreach (string str in Start[nonTerminal])
                                        {
                                            if (str != "$")
                                            {
                                                Next[LEFTPart].Add(str);
                                            }
                                            else
                                            {
                                                isNext = true;
                                            }
                                        }

                                        if (isNext)
                                        {
                                            foreach (string str in Next[conversion.LEFT])
                                            {
                                                Next[LEFTPart].Add(str);
                                            }
                                        }

                                        if (temporarySize != Next[LEFTPart].Count())
                                        {
                                            modification = true;
                                        }
                                    }
                                    else if (temporaryString[0] == ' ')
                                    {
                                        temporarySize = Next[LEFTPart].Count();
                                        Next[LEFTPart].Add(" ");

                                        if (temporarySize != Next[LEFTPart].Count())
                                        {
                                            modification = true;
                                        }
                                    }
                                    else
                                    {
                                        temporarySize = Next[LEFTPart].Count();
                                        RIGHTIndex = temporaryString.Substring(1).IndexOf("\'");
                                        nonTerminal = temporaryString.Substring(LEFTIndex, RIGHTIndex - LEFTIndex + 2);

                                        Next[LEFTPart].Add(nonTerminal);

                                        if (temporarySize != Next[LEFTPart].Count())
                                        {
                                            modification = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static HashSet<string> GetStart(Dictionary<string, HashSet<string>> Start, string checkString)
    {
        HashSet<string> ans = new HashSet<string>();
        int RIGHTIndex = 0;
        int LEFTIndex = 0;
        string temporaryString = checkString;


        while (true)
        {
            if (temporaryString[0] == '\'')
            {
                LEFTIndex = 0;
                RIGHTIndex = temporaryString.Substring(1).IndexOf('\'');
                ans.Add(temporaryString.Substring(0, RIGHTIndex + 2));
                temporaryString = temporaryString.Substring(RIGHTIndex + 2);
                break;
            }
            else if (temporaryString[0] == '<')
            {
                LEFTIndex = 0;
                RIGHTIndex = temporaryString.Substring(1).IndexOf('>');
                string nonTerminal = temporaryString.Substring(0, RIGHTIndex + 2);
                bool flag = false;
                temporaryString = temporaryString.Substring(RIGHTIndex + 2);
                foreach (string str in Start[nonTerminal])
                {
                    ans.Add(str);
                    if (str == "$")
                        flag = true;
                }
                if (!flag)
                    break;
                else
                {
                    flag = false;
                }
            }
            else if (temporaryString[0] == '$')
            {
                ans.Add("$");
                break;
            }

        }

        return ans;
    }

    public static void FillingTable(Dictionary<string, HashSet<string>> Start, Dictionary<string, HashSet<string>> Next,
        List<Conversion> allConversions, ref List<KeyValuePair<(string A, string a), string>> predictionAnalisisTable, List<Order> orders)
    {
        bool flag = false;

        foreach (string str in GetStart(Start, item.RIGHT))
        {
            if (str.StartsWith('\''))
            {
                predictionAnalisisTable.Add(temporaryPair);

            }
            else if (str.Contains('$'))
                flag = true;
        }
        if (flag)
        {
            flag = false;

            foreach (string str in Next[item.LEFT])
            {
                if (str.StartsWith('\''))
                {
                    KeyValuePair<(string A, string a), string> temporaryPair = new KeyValuePair<(string A, string a), string>((item.LEFT, str), item.RIGHT);

                    predictionAnalisisTable.Add(temporaryPair);

                }
                else if (str.Contains('$'))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                flag = false;

                KeyValuePair<(string A, string a), string> temporaryPair = new KeyValuePair<(string A, string a), string>((item.LEFT, "$"), item.RIGHT);
            }
        }

        for (int i = 0; i < Next.Count(); i++)
        {
            string key = Next.ElementAt(i).Key;
            string conversions = "";

            foreach (Order item in orders)
            {
                if (item.LEFT == key)
                {
                    foreach (string s in item.conversions)
                    {
                        conversions += s;
                    }
                    break;
                }
            }

            int LEFTIndex = conversions.IndexOf('<');
            int RIGHTIndex = conversions.IndexOf('>');

            if (LEFTIndex != -1)
            {
                string newKey = "";

                if (RIGHTIndex + 1 < conversions.Length && key != "<loop>")
                    newKey = conversions.Substring(LEFTIndex, RIGHTIndex + 1 - LEFTIndex);
                else if (key != "<loop>")
                    newKey = conversions.Substring(LEFTIndex);

                if (newKey != "")
                {
                    foreach (string item in Next[newKey])
                    {
                        KeyValuePair<(string A, string a), string> temporaryPair = new KeyValuePair<(string A, string a), string>((key, item), "synch");
                        predictionAnalisisTable.Add(temporaryPair);
                    }
                }
            }
        }

        for (int i = 0; i < Start.Count(); i++)
        {
            string key = Start.ElementAt(i).Key;

            foreach (string item in Start[key])
            {
                KeyValuePair<(string A, string a), string> temporaryPair = new KeyValuePair<(string A, string a), string>((key, item), "synch");

                predictionAnalisisTable.Add(temporaryPair);
            }
        }
    }

    static void Run(List<KeyValuePair<(string A, string a), string>> predictionAnalisisTable, string inputStr, List<string> terminals, List<Order> orders, List<Conversion> allConversions)
    {
        Stack<string> stack = new Stack<string>();

        stack.Push("$");
        stack.Push("<program>");

        while (true)
        {
            if (stack.Peek() == "$" && stack.Count != 1)
            {
                stack.Pop();
            }

            if (stack.Peek() == "$" && inputStr.Length == 0)
            {
                break;
            }
            else if (inputStr.Length == 0)
            {
                break;
            }
            else if (stack.Peek()[0] == '\'')
            {
                int RIGHT = stack.Peek().Substring(1).IndexOf('\'');
                string temporary = stack.Peek().Substring(1, RIGHT);
                int space = inputStr.IndexOf(" ");
                string str = inputStr.Substring(0, space + 1);

                if (space == 0)
                {
                    space = inputStr.Substring(1).IndexOf(" ");
                    if (space == -1)
                        str.Substring(1);
                    else
                        str = inputStr.Substring(1, space);
                }
                else if (space == -1)
                {
                    str = inputStr;
                }

                if (isTerminal(terminals, str))
                {
                    if (space == -1)
                        inputStr = "";
                    else
                        inputStr = inputStr.Substring(space + 1);
                    stack.Pop();
                }

                else if (temporary.Length == 1)
                {
                    if (temporary == str[0].ToString())
                    {
                        inputStr = inputStr.Substring(1);
                        stack.Pop();
                    }
                    else
                    {
                        Console.WriteLine(str[0].ToString() + " is Wrong");
                        inputStr = inputStr.Substring(1);
                        stack.Pop();
                    }
                }
                else if (!temporary.Contains(str))
                {
                    Console.WriteLine(str.ToString() + " is Wrong");
                    inputStr = inputStr.Substring(space + 1);
                    stack.Pop();
                }
            }
            else if (stack.Peek()[0] == '<')
            {
                int space = inputStr.IndexOf(" ");
                string str = inputStr.Substring(0, space + 1);
                if (space == -1)
                {
                    str = inputStr;
                }
                else if (space == 0)
                {
                    space = inputStr.Substring(1).IndexOf(" ");
                    if (space == -1)
                        str.Substring(1);
                    else
                        str = inputStr.Substring(1, space);

                }

                KeyValuePair<(string A, string a), string> temporarySpan = SearchSpan(predictionAnalisisTable, stack.Peek(), str);

                if (temporarySpan.Value == null)
                {

                    string conversions = "";
                    string temporaryConversions = "";

                    List<int> indexes = new List<int>();

                    foreach (Conversion item in allConversions)
                    {
                        if (item.LEFT == stack.Peek())
                        {
                            indexes.Add(allConversions.IndexOf(item));
                            conversions += item.RIGHT;
                        }
                    }
                    if (stack.Peek() == "<statement>")
                        conversions = allConversions[indexes[1]].RIGHT;
                    bool flag = true;
                    foreach (char item in conversions)
                    {
                        if (flag && item == '\'')
                        {
                            temporaryConversions += item;
                            flag = false;
                        }
                        else if (!flag && item == '\'')
                        {
                            temporaryConversions += item;
                            temporaryConversions += ' ';
                            flag = true;
                        }
                        else if (item == '>')
                        {
                            temporaryConversions += item;
                            temporaryConversions += ' ';
                        }
                        else if (item == ' ')
                        {
                            temporaryConversions += '*';
                        }
                        else if (item == '$')
                        {
                            temporaryConversions += item;
                            temporaryConversions += ' ';
                        }
                        else
                        {
                            temporaryConversions += item;
                        }
                    }
                    string[] trans = temporaryConversions.Split(new string[] { " " }, StringSpliTOPtions.RemoveEmptyEntries);

                    for (int i = 0; i < trans.Length; i++)
                    {
                        trans[i] = trans[i].Replace('*', ' ');
                    }

                    bool hasNonTerminal = false;

                    bool isError = true;
                    for (int i = trans.Length - 1; i >= 0; i--)
                    {
                        if (trans[i][0] == '<')
                        {
                            hasNonTerminal = true;
                            break;
                        }
                    }

                    if (hasNonTerminal)
                    {
                        for (int i = trans.Length - 1; i >= 0; i--)
                        {
                            stack.Push(trans[i]);
                        }

                    }
                    else
                    {
                        if (stack.Peek().Contains(str) || stack.Peek()[0] == '<')
                            isError = false;

                        stack.Pop();
                    }

                    if (isError)
                    {
                        Console.WriteLine(str + " is wrong");
                    }

                    if (space == -1)
                    {
                        inputStr = "";
                    }
                    else
                    {
                        inputStr = inputStr.Substring(space + 1);
                    }

                }
                else if (temporarySpan.Value == "synch")
                {
                    string conversions = "";
                    string temporaryConversions = "";

                    foreach (Order item in orders)
                    {
                        if (item.LEFT == stack.Peek())
                        {
                            foreach (string s in item.conversions)
                            {
                                conversions += s;
                            }
                            break;
                        }
                    }

                    bool flag = true;

                    foreach (char item in conversions)
                    {
                        if (flag && item == '\'')
                        {
                            temporaryConversions += item;
                            flag = false;
                        }
                        else if (!flag && item == '\'')
                        {
                            temporaryConversions += item;
                            temporaryConversions += ' ';
                            flag = true;
                        }
                        else if (item == '>')
                        {
                            temporaryConversions += item;
                            temporaryConversions += ' ';
                        }
                        else if (item == ' ')
                        {
                            temporaryConversions += '*';
                        }
                        else
                        {
                            temporaryConversions += item;
                        }
                    }

                    string[] trans = temporaryConversions.Split(new string[] { " " }, StringSpliTOPtions.RemoveEmptyEntries);

                    for (int i = 0; i < trans.Length; i++)
                    {
                        trans[i] = trans[i].Replace('*', ' ');
                    }

                    stack.Pop();

                    bool hasNonTerminal = false;

                    for (int i = trans.Length - 1; i >= 0; i--)
                    {
                        if (trans[i][0] == '<')
                        {
                            hasNonTerminal = true;
                            break;
                        }
                    }

                    if (hasNonTerminal)
                    {
                        for (int i = trans.Length - 1; i >= 0; i--)
                        {
                            stack.Push(trans[i]);
                        }
                    }
                    stack.Pop();
                }
                else
                {
                    string conversions = "";
                    bool flag = true;

                    foreach (char item in temporarySpan.Value)
                    {
                        if (flag && item == '\'')
                        {
                            conversions += item;
                            flag = false;
                        }
                        else if (!flag && item == '\'')
                        {
                            conversions += item;
                            conversions += ' ';
                            flag = true;
                        }
                        else if (item == '>')
                        {
                            conversions += item;
                            conversions += ' ';
                        }
                        else if (item == ' ')
                        {
                            conversions += '*';
                        }
                        else
                        {
                            conversions += item;
                        }
                    }

                    string[] trans = conversions.Split(new string[] { " " }, StringSpliTOPtions.RemoveEmptyEntries);

                    for (int i = 0; i < trans.Length; i++)
                    {
                        trans[i] = trans[i].Replace('*', ' ');
                    }

                    stack.Pop();

                    for (int i = trans.Length - 1; i >= 0; i--)
                    {
                        stack.Push(trans[i]);
                    }
                }
            }
        }
    }

    public static KeyValuePair<(string A, string a), string> SearchSpan(List<KeyValuePair<(string A, string a), string>> predictionAnalisisTable, string nonTerminal, string terminal)
    {
        foreach (KeyValuePair<(string A, string a), string> item in predictionAnalisisTable)
        {
            if (item.Key.A == nonTerminal && item.Key.a.Length > 2 && item.Key.a.Substring(1, item.Key.a.Length - 2) == terminal)
            {
                return item;
            }
            else if (item.Key.A == nonTerminal && item.Key.a.Length > 2 && item.Key.a.Substring(1, item.Key.a.Length - 2).Contains(terminal) && terminal.Length != 1)
            {
                return item;
            }
        }
        return new KeyValuePair<(string A, string a), string>();
    }

    public static bool isTerminal(List<string> terminals, string check)
    {
        foreach (string item in terminals)
        {
            if (item.Length != 1 && item.Contains(check) || item.Substring(1, item.Length - 2) == check)
            {
                return true;
            }
        }
        return false;
    }
}

struct Conversion
{
    public string LEFT;
    public string RIGHT;
    public Conversion(string LEFT, string RIGHT)
    {
        this.LEFT = LEFT;
        this.RIGHT = RIGHT;
    }
}