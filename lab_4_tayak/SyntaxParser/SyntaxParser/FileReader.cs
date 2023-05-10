namespace SyntaxParser;

using System;
using System.IO;

public static class FileReader
{
    public static string[] Read()
    {
        string text;
        try
        {
            using (StreamReader sr = new StreamReader("../../../file.txt", System.Text.Encoding.Default))
            {
                text = sr.ReadToEnd();
            }
            return text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
}