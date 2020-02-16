using System;
using System.Globalization;
using System.Linq;

namespace Model.DataModel
{
    public class Header
    {
        public string No { get; }
        private static readonly string NO_NAME_1 = "No";
        private static readonly string NO_NAME_2 = "PROJECT NUMBER";

        public string Title { get; }
        private static readonly string TITLE_NAME = "TITLE";

        public string Phase { get; }
        private static readonly string PHASE_NAME = "PHASE";

        public string Date { get; }
        private static readonly string DATE_NAME = "Date";

        public Header(string content)
        {
            string[] lines = content.Split('\n');

            foreach (string line in lines)
            {
                if (line.Contains(NO_NAME_1))
                {
                    No = line.Split(NO_NAME_1 + ":").Last().Split("  ").First().Trim();
                }

                if (line.Contains(NO_NAME_2))
                {
                    No = line.Split(NO_NAME_2 + ":").Last().Split("  ").First().Trim();
                }

                if (line.Contains(TITLE_NAME))
                {
                    Title = line.Split(TITLE_NAME + ":").Last().Split("  ").First().Trim();
                }

                if (line.Contains(PHASE_NAME))
                {
                    Phase = line.Split(PHASE_NAME + ":").Last().Split("  ").First().Trim();
                }

                if (line.Contains(DATE_NAME))
                {
                    Date = line.Split(DATE_NAME + ":").Last().Split("  ").First().Trim();
                }
            }
        }
    }
}