using Model.DataModel;
using Model.List;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;
using Model.Extensions;

namespace XLSXManagement.WriteDataStrategy
{
    public class BoltsDeliveryStrategy : IXLSXWriteDataStrategy
    {
        private AbstractList _list;

        public void WriteData(AbstractList list, ISheet sheet)
        {
            _list = list;

            FillRemainingColumns();
            FixEmptyEntries();
        }

        private void FillRemainingColumns()
        {
            int actualNumEntries = GetNumEntries();
            List<StringColumn> columns = _list.Columns;

            foreach (StringColumn column in columns)
            {
                FillMissingEntries(column.Data.First().Entries, actualNumEntries);
            }
        }

        private void FillMissingEntries(ICollection<string> entries, int actualNumEntries)
        {
            int numRemainingEntries = actualNumEntries - entries.Count();

            for (int i = 0; i < numRemainingEntries; i++)
            {
                entries.Add(string.Empty);
            }
        }

        private int GetNumEntries()
        {
            return _list.Columns.First().Data.First().Entries.Count;
        }

        private void FixEmptyEntries()
        {
            ICollection<string> lastColumnEntries = _list.Columns.Last().Data.First().Entries;

            for (int entryIndex = 0; entryIndex < GetNumEntries(); entryIndex++)
            {
                string lastColumnEntry = lastColumnEntries.ElementAt(entryIndex);
                if (string.IsNullOrEmpty(lastColumnEntry))
                {
                    MoveDataToRightByOneColumn(entryIndex);
                }
            }
        }

        private void MoveDataToRightByOneColumn(int entryIndex)
        {
            int numColumns = _list.Columns.Count;
            for (int columnIndex = numColumns - 2; columnIndex >= 1; columnIndex--)
            {
                List<string> leftEntries = _list.Columns.ElementAt(columnIndex).Data.First().Entries;
                List<string> rightEntries = _list.Columns.ElementAt(columnIndex + 1).Data.First().Entries;

                string leftEntry = leftEntries.ElementAt(entryIndex);

                leftEntries.UpdateElementAt(entryIndex, string.Empty);
                rightEntries.UpdateElementAt(entryIndex, leftEntry);
            }
        }
    }
}