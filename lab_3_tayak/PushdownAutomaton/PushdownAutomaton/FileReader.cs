using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushdownAutomaton
{
    public static class FileReader
    {
        public static string[] Read()
        {
            string text;

            try
            {
                using (StreamReader sr = new StreamReader("../../Files/text1.txt", System.Text.Encoding.Default))
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
}
