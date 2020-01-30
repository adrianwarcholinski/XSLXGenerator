using System.Collections.Generic;

namespace Model.Material
{
    public class DataChunk
    {
        public ICollection<string> Entries { get; }
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