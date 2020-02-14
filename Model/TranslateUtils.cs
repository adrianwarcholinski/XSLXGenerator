using System;
using System.Collections.Generic;
using System.IO;

namespace Model
{
    public class TranslateUtils
    {
        public static string Translate(string input)
        {
            using (FileStream stream = File.Open("tlumaczenia.txt", FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
                string[] lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(";");
                    string source = parts[0];
                    string target = parts[1];
                    if (input.Contains(source))
                    {
                        return input.Replace(source, target).Replace("(", " (").Replace("˛", "2");
                    }
                }
            }

            return input;
        }

        public static IEnumerable<string> GetTranslatedStrings(IEnumerable<string> originalStrings)
        {
            ICollection<string> translatedStrings = new List<string>();
            foreach (string originalString in originalStrings)
            {
                translatedStrings.Add(Translate(originalString));
            }

            return translatedStrings;
        }
    }
}