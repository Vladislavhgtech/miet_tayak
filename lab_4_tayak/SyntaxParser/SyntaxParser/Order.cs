namespace SyntaxParser;

using System.Collections.Generic;

public class Order
{
    public string LEFT;
    public List<string> conversions;

    public Order(string LEFT, string[] conversions, bool flag)
    {
        this.LEFT = LEFT;
        this.conversions = new List<string>();

        foreach (string str in conversions)
        {
            if (flag)
            {
                string temporaryConversions = str;

                while (true)
                {
                    if (temporaryConversions[0] == '\'')
                    {
                        int RIGHT = temporaryConversions.Substring(1).IndexOf("\'");
                        string temporary = temporaryConversions.Substring(0, RIGHT + 2);

                        this.conversions.Add(temporary);

                        temporaryConversions = temporaryConversions.Substring(RIGHT + 2);
                    }
                    else
                    {
                        if (temporaryConversions[0] == '<')
                        {
                            int RIGHT = temporaryConversions.Substring(1).IndexOf(">");
                            string temporary = temporaryConversions.Substring(0, RIGHT + 2);
                            this.conversions.Add(temporary);
                            temporaryConversions = temporaryConversions.Substring(RIGHT + 2);
                        }
                    }
                    if (temporaryConversions.IndexOf('\'') == -1 && temporaryConversions.IndexOf('<') == -1)
                        break;
                }
            }
            else
            {
                this.conversions.Add(str);
            }
        }
    }

    public static Order Search(List<Order> orders, string LEFT)
    {
        foreach (Order item in orders)
        {
            if (item.LEFT == LEFT)
                return item;
        }

        return null;
    }
}