using Model.DataModel;
using System.Collections.Generic;
using System.Linq;
using Model.List;

namespace Model.Extensions
{
    public static class StructuralListExtensions
    {
        private static StructuralList _list;

        public static StructuralList ConvertToStructuralList(this AbstractList list)
        {
            _list = (StructuralList) list;

            TidyColumns();

            return _list;
        }

        private static void TidyColumns()
        {
            FillRemainingColumns();
            MoveToRightColumns();
            HandleFirstRows();

            _list.Columns = GetDividedData();
        }

        private static List<StringColumn> GetDividedData()
        {
            List<StringColumn> dividedColumns = ColumnUtils.GetClonedColumns(_list);
            List<StringColumn> originalColumns = _list.Columns;

            int numEntries = originalColumns.ElementAt(1).GetLastChunk().Entries.Count;
            for (int entryIndex = 0; entryIndex < numEntries; entryIndex++)
            {
                string assemblyEntry = originalColumns.First().GetLastChunk().Entries.ElementAt(entryIndex);
                if (!string.IsNullOrEmpty(assemblyEntry))
                {
                    StartNewChunk(dividedColumns);
                }

                for (int columnIndex = 0; columnIndex < dividedColumns.Count; columnIndex++)
                {
                    string entry = originalColumns.ElementAt(columnIndex).GetLastChunk().Entries.ElementAt(entryIndex);
                    dividedColumns.ElementAt(columnIndex).GetLastChunk().AddEntry(entry);
                }
            }

            return dividedColumns;
        }

        private static void StartNewChunk(List<StringColumn> columns)
        {
            for (int columnIndex = 0; columnIndex < columns.Count; columnIndex++)
            {
                StringColumn currentColumn = columns.ElementAt(columnIndex);
                currentColumn.Data.Add(new DataChunk());
            }
        }

        private static void HandleFirstRows()
        {
            List<string> assemblyEntries = ColumnUtils.FindColumn(_list, "Assembly", out _).GetLastChunk().Entries;
            List<string> partEntries = ColumnUtils.FindColumn(_list, "Part", out _).GetLastChunk().Entries;
            List<string> gradeEntries = ColumnUtils.FindColumn(_list, "Grade", out _).GetLastChunk().Entries;
            List<string> weightEntries = ColumnUtils.FindColumn(_list, "Weight", out _).GetLastChunk().Entries;
            List<string> lengthEntries = ColumnUtils.FindColumn(_list, "Length", out _).GetLastChunk().Entries;

            for (int entryIndex = 0; entryIndex < assemblyEntries.Count; entryIndex++)
            {
                string partEntry = partEntries.ElementAt(entryIndex);
                if (!partEntry.Contains(" "))
                {
                    string gradeEntry = gradeEntries.ElementAt(entryIndex);

                    assemblyEntries.UpdateElementAt(entryIndex, partEntry);
                    partEntries.UpdateElementAt(entryIndex, "");

                    weightEntries.InsertAndRemoveLastElement(entryIndex, gradeEntry);
                    gradeEntries.UpdateElementAt(entryIndex, "");

                    lengthEntries.InsertAndRemoveLastElement(entryIndex, "");
                }
            }
        }

        private static void MoveToRightColumns()
        {
            List<string> assemblyEntries = ColumnUtils.FindColumn(_list, "Assembly", out _).GetLastChunk().Entries;

            for (int entryIndex = 0; entryIndex < assemblyEntries.Count; entryIndex++)
            {
                List<StringColumn> columns = _list.Columns;
                for (int columnIndex = columns.Count - 2; columnIndex >= 0; columnIndex--)
                {
                    List<string> leftEntries = columns.ElementAt(columnIndex).Data.First().Entries;
                    List<string> rightEntries = columns.ElementAt(columnIndex + 1).Data.First().Entries;

                    string leftEntry = leftEntries.ElementAt(entryIndex);

                    leftEntries.RemoveAt(entryIndex);
                    leftEntries.Insert(entryIndex, "");

                    rightEntries.RemoveAt(entryIndex);
                    rightEntries.Insert(entryIndex, leftEntry);
                }
            }
        }

        private static void FillRemainingColumns()
        {
            List<string> assemblyEntries = ColumnUtils.FindColumn(_list, "Assembly", out _).GetLastChunk().Entries;
            int numAssemblyEntries = assemblyEntries.Count;

            List<StringColumn> columns = _list.Columns;
            foreach (StringColumn column in columns)
            {
                DataChunk data = column.Data.First();
                int numEntries = data.Entries.Count;
                int remainingEntries = numAssemblyEntries - numEntries;

                for (int i = 0; i < remainingEntries; i++)
                {
                    data.Entries.Add("");
                }
            }
        }
    }
}