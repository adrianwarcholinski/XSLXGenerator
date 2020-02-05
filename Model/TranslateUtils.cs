using System;
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
    }
}