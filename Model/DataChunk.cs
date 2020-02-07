using System.Collections.Generic;

namespace Model
{
    public class DataChunk
    {
        public List<string> Entries { get; }
        public string Summary { get; set; }

        public DataChunk()
        {
            Entries = new List<string>();
        }

        public void AddEntry(string value)
        {
            Entries.Add(value);
        }
    }
}