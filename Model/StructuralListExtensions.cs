using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Model
{
    public static class StructuralListExtensions
    {
        private static StructuralList _list;
        private static StringColumn _assemblyColumn;

        public static StructuralList ConvertToStructuralList(this StructuralList list)
        {
            _list = list;

            TidyColumns();

            return _list;
        }

        private static void TidyColumns()
        {
            _assemblyColumn = ColumnUtils.FindColumn(_list, "Assembly", out _);
            FillRemainingColumns();

            DataChunk assemblyData = _assemblyColumn.GetLastChunk();
            List<string> assemblyEntries = assemblyData.Entries;

            for (int entryIndex = 0; entryIndex < assemblyEntries.Count; entryIndex++)
            {
                string entry = assemblyEntries.ElementAt(entryIndex);
                if (!entry.Contains(" "))
                {
                    OrganizeFirstRow(entryIndex);
                }
                else
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

            List<string> noEntries = ColumnUtils.FindColumn(_list, "No.", out _).Data.First().Entries;
            for (int entryIndex = 0; entryIndex < noEntries.Count; entryIndex++)
            {
                string noEntry = noEntries.ElementAt(entryIndex);
                if (!noEntry.Contains(" "))
                {
                    noEntries.RemoveAt(entryIndex);
                }
            }
        }

        private static void OrganizeFirstRow(int entryIndex)
        {
            List<string> assemblyEntries = ColumnUtils.FindColumn(_list, "Assembly", out _).Data.First().Entries;
            List<string> partEntries = ColumnUtils.FindColumn(_list, "Part", out _).Data.First().Entries;
            List<string> noEntries = ColumnUtils.FindColumn(_list, "No.", out _).Data.First().Entries;
            List<string> profileEntries = ColumnUtils.FindColumn(_list, "Profile", out _).Data.First().Entries;
            List<string> weightEntries = ColumnUtils.FindColumn(_list, "Weight", out _).Data.First().Entries;

            string assemblyEntry = assemblyEntries.ElementAt(entryIndex).Trim();
            string partEntry = partEntries.ElementAt(entryIndex).Trim();
            string noEntry = noEntries.ElementAt(entryIndex).Trim();
            string profileEntry = profileEntries.ElementAt(entryIndex).Trim();

            for (int columnIndex = 0; columnIndex < _list.Columns.Count; columnIndex++)
            {
                List<string> entries = _list.Columns.ElementAt(columnIndex).Data.First().Entries;

                if (entries != assemblyEntries && entries != noEntries && entries != profileEntries &&
                    entries != weightEntries)
                {
                    entries.Insert(entryIndex, "");
                }
            }

            assemblyEntries.Insert(entryIndex, assemblyEntry);
            noEntries.Insert(entryIndex, partEntry);
            profileEntries.Insert(entryIndex, noEntry);
            weightEntries.Insert(entryIndex, profileEntry);

            for (int columnIndex = 0; columnIndex < _list.Columns.Count; columnIndex++)
            {
                List<string> entries = _list.Columns.ElementAt(columnIndex).Data.First().Entries;
                if (entries != assemblyEntries && entries != noEntries && entries != profileEntries &&
                    entries != weightEntries)
                {
                    entries.RemoveAt(entries.Count - 1);
                }
                else
                {
                    entries.RemoveAt(entryIndex + 1);
                }
            }
        }

        private static void FillRemainingColumns()
        {
            List<string> assemblyEntries = _assemblyColumn.Data.First().Entries;
            int numAssemblyEntries = assemblyEntries.Count;
            assemblyEntries.RemoveAt(--numAssemblyEntries);

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