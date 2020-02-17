using System.Collections.Generic;
using System.Linq;

namespace Model.DataModel
{
    public class StringColumn
    {
        private static readonly IEnumerable<string> NUMBER_STRING_COLUMNS =
            new[] {"Standard", "Assembly", "Part", "Asmbly Pos.", "Klasa"};

        private static readonly IEnumerable<string> INT_COLUMNS =
            new[] { "Length", "No.", "Quantity", "Number" };

        public string Name { get; }
        public List<DataChunk> Data { get; set; }

        public StringColumn(string columnName)
        {
            Name = columnName;
            InitData();
        }

        private void InitData()
        {
            Data = new List<DataChunk>(1);
            Data.Add(new DataChunk());
        }

        public DataChunk GetLastChunk()
        {
            return Data.Last();
        }

        public void FinishChunk(string summary)
        {
            GetLastChunk().Summary = summary;
            Data.Add(new DataChunk());
        }

        public int GetNumDecimalPlaces()
        {
            if (NUMBER_STRING_COLUMNS.Any(s => Name.Contains(TranslateUtils.Translate(s))))
            {
                return -1;
            }

            if (INT_COLUMNS.Any(s => Name.Contains(TranslateUtils.Translate(s))))
            {
                return 0;
            }

            return Data.First().Entries.First().Split(".").Last().Trim().Length;
        }
    }
}