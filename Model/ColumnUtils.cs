using Model.DataModel;
using Model.List;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class ColumnUtils
    {
        public static StringColumn FindColumn(AbstractList list, string columnName, out int index)
        {
            StringColumn foundColumn = list.Columns.First(column => column.Name.Contains(TranslateUtils.Translate(columnName)));
            index = list.Columns.ToList().IndexOf(foundColumn);
            return foundColumn;
        }

        public static List<StringColumn> GetClonedColumns(AbstractList list)
        {
            List<StringColumn> clonedColumns = new List<StringColumn>();
            List<StringColumn> oldColumns = list.Columns;
            foreach (StringColumn oldColumn in oldColumns)
            {
                StringColumn newColumn = new StringColumn(oldColumn.Name);
                newColumn.Data = new List<DataChunk>();
                clonedColumns.Add(newColumn);
            }

            return clonedColumns;
        }
    }
}