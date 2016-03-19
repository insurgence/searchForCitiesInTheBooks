using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

using eBdb.EpubReader;

namespace searchForCitiesInTheBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!File.Exists(args[0]))
            {
                Console.WriteLine("Error! File not exist! Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            Epub epub = new Epub(args[0]);

            string title  = epub.Title[0];
            string  author = epub.Creator[0];

            string str = epub.GetContentAsPlainText();

            StringBuilder sb = new StringBuilder();

            foreach (char ch in str)
                if (char.IsLetter(ch) || char.IsWhiteSpace(ch) || char.IsSeparator(ch) || ch.Equals('-'))
                    sb.Append(ch);
            string[] res = sb.ToString().Split(new Char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            sb.Clear();

            List<string> ls = new List<string>();

            foreach (string s in res)
            {
                if (char.IsUpper(s[0]))
                    ls.Add(s);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
