using System.Collections.Generic;

namespace Model.Material
{
    public class StringColumn : IColumn
    {
        public string Name { get; }
        public ICollection<DataChunk> Data { get; }

        public StringColumn(string columnName)
        {
            Name = columnName;
            Data = new List<DataChunk>();
        }

        public void AppendData(DataChunk chunk)
        {
            Data.Add(chunk);
        }
    }
}