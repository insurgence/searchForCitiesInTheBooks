using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

using eBdb.EpubReader;
using FuzzyString;

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

            List<string> lines = new List<string>();

            foreach (string s in res)
            {
                if (char.IsUpper(s[0]))
                    lines.Add(s);
            }

            List<FuzzyStringComparisonOptions> options = new List<FuzzyStringComparisonOptions>();

            options.Add(FuzzyStringComparisonOptions.UseHammingDistance);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);

            FuzzyStringComparisonTolerance tolerance = FuzzyStringComparisonTolerance.Strong;

            string[] file = File.ReadAllLines(args[1]);
            string path = args[2];

            List<string> cities = new List<string>();
            List<string> countries = new List<string>();

            foreach (string src in lines)
            {
                foreach (string temp in file)
                {
                    string[] arr = temp.Split(':');

                    string cityInTheFile = arr[0];
                    string countryInTheFile = arr[1];

                    bool boolCity = src.ApproximatelyEquals(cityInTheFile, options, tolerance);
                    bool boolCountry = src.ApproximatelyEquals(countryInTheFile, options, tolerance);

                    if (boolCity)
                        cities.Add(cityInTheFile);

                    if (boolCountry)
                        countries.Add(countryInTheFile);
                }

                if (cities.Count() > 0)
                {
                    String.Join(", ", cities);
                    cities.Insert(0, src + " : ");
                }

                if (countries.Count() > 0)
                {
                    String.Join(", ", countries);
                    countries.Insert(0, src + " : ");
                }

                File.WriteAllLines(path, cities);
                File.WriteAllText(path, "\n");
                File.WriteAllLines(path, cities);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
