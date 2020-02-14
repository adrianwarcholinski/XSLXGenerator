using Model.DataModel;
using Model.List;
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
    }
}