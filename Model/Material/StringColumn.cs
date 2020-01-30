using System.Collections.Generic;
using System.Linq;

namespace Model.Material
{
    public class StringColumn
    {
        public string Name { get; }
        public ICollection<DataChunk> Data { get; private set; }

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
    }
}