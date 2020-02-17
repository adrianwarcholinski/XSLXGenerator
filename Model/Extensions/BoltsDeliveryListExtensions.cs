using System;
using Model.DataModel;
using Model.List;
using System.Collections.Generic;
using System.Linq;

namespace Model.Extensions
{
    public static class BoltsDeliveryListExtensions
    {
        private static BoltsDeliveryList _list;

        public static BoltsDeliveryList ConvertToBoltsDeliveryList(this AbstractList list)
        {
            _list = (BoltsDeliveryList) list;

            FillRemainingColumns();
            FixEmptyEntries();
            DivideDataIntoChunks();
            ReplaceTextInSize();
            SetColumnsInOrder();

            return _list;
        }

        private static void FillRemainingColumns()
        {
            int actualNumEntries = GetNumEntries();
            List<StringColumn> columns = _list.Columns;

            foreach (StringColumn column in columns)
            {
                FillMissingEntries(column.Data.First().Entries, actualNumEntries);
            }
        }

        private static void FillMissingEntries(ICollection<string> entries, int actualNumEntries)
        {
            int numRemainingEntries = actualNumEntries - entries.Count();

            for (int i = 0; i < numRemainingEntries; i++)
            {
                entries.Add(string.Empty);
            }
        }

        private static int GetNumEntries()
        {
            return _list.Columns.First().Data.First().Entries.Count;
        }

        private static void FixEmptyEntries()
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

        private static void MoveDataToRightByOneColumn(int entryIndex)
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

        private static void DivideDataIntoChunks()
        {
            int numEntries = GetNumEntries();

            BoltsDeliveryContentType lastContentType = BoltsDeliveryContentType.Null;
            string lastStandardEntry = string.Empty;

            List<StringColumn> newColumns = ColumnUtils.GetClonedColumns(_list);

            for (int entryIndex = 0; entryIndex < numEntries; entryIndex++)
            {
                BoltsDeliveryContentType entryType = GetEntryContentType(entryIndex);
                string standardEntry = _list.Columns.First().GetLastChunk().Entries.ElementAt(entryIndex);

                if (entryType == BoltsDeliveryContentType.Null)
                {
                    continue;
                }

                if (entryType != lastContentType || !lastStandardEntry.Equals(standardEntry))
                {
                    AddDataChunks(newColumns);
                }

                AddEntry(entryIndex, newColumns);

                lastContentType = entryType;
                lastStandardEntry = standardEntry;
            }

            _list.Columns = newColumns;
        }

        private static void AddDataChunks(IEnumerable<StringColumn> columns)
        {
            foreach (StringColumn column in columns)
            {
                column.Data.Add(new DataChunk());
            }
        }

        private static void AddEntry(int entryIndex, ICollection<StringColumn> targetColumns)
        {
            int numColumns = targetColumns.Count;
            for (int columnIndex = 0; columnIndex < numColumns; columnIndex++)
            {
                StringColumn column = targetColumns.ElementAt(columnIndex);
                IEnumerable<string> originalColumn = _list.Columns.ElementAt(columnIndex).GetLastChunk().Entries;

                string entry = originalColumn.ElementAt(entryIndex);
                column.GetLastChunk().AddEntry(entry);
            }
        }

        private static void ReplaceTextInSize()
        {
            StringColumn sizeColumn = ColumnUtils.FindColumn(_list, "Size", out _);
            StringColumn standardColumn = ColumnUtils.FindColumn(_list, "Standard", out _);
            StringColumn nameColumn = ColumnUtils.FindColumn(_list, "Name", out _);

            for (int chunkIndex = 0; chunkIndex < sizeColumn.Data.Count; chunkIndex++)
            {
                DataChunk sizeChunk = sizeColumn.Data.ElementAt(chunkIndex);
                DataChunk standardChunk = standardColumn.Data.ElementAt(chunkIndex);
                DataChunk nameChunk = nameColumn.Data.ElementAt(chunkIndex);

                for (int entryIndex = 0; entryIndex < sizeChunk.Entries.Count; entryIndex++)
                {
                    string sizeEntry = sizeChunk.Entries.ElementAt(entryIndex);
                    string standardEntry = standardChunk.Entries.ElementAt(entryIndex);
                    string nameEntry = nameChunk.Entries.ElementAt(entryIndex);

                    sizeEntry = sizeEntry.Replace("BOLT", IsHVM(standardEntry) ? "HVM" : "M")
                        .Replace("NUT", "Nakrętka M")
                        .Replace("WASHER", "Podkładka");

                    while (sizeEntry.Contains(".0"))
                    {
                        sizeEntry = sizeEntry.Replace(".0", ".");
                    }

                    sizeEntry = sizeEntry.Replace(".", string.Empty);

                    if (nameEntry.Contains("HILTI"))
                    {
                        string sizePrefix = nameEntry.Split(" ").ElementAt(1);
                        sizeEntry = sizePrefix + " " + sizeEntry;

                        standardChunk.Entries.Insert(entryIndex, "Kotew HILTI");
                        standardChunk.Entries.RemoveAt(entryIndex + 1);
                    }

                    if (nameEntry.Contains("ANCHORBAR"))
                    {
                        standardChunk.Entries.Insert(entryIndex, "Pręt gwintowany");
                        standardChunk.Entries.RemoveAt(entryIndex + 1);
                    }

                    sizeChunk.Entries.Insert(entryIndex, sizeEntry);
                    sizeChunk.Entries.RemoveAt(entryIndex + 1);
                }
            }
        }

        private static void SetColumnsInOrder()
        {
            StringColumn sizeColumn = ColumnUtils.FindColumn(_list, "Size", out _);
            StringColumn classColumn = GenerateClassColumn();
            StringColumn quantityColumn = ColumnUtils.FindColumn(_list, "Quantity", out _);
            StringColumn standardColumn = ColumnUtils.FindColumn(_list, "Standard", out _);
            StringColumn notesColumn = GenerateNotesColumn();

            _list.Columns = new List<StringColumn>(new []{sizeColumn, classColumn, quantityColumn, standardColumn, notesColumn});
        }

        private static bool IsHVM(string standardEntry)
        {
            return standardEntry.Equals("6914");
        }

        private static StringColumn GenerateClassColumn()
        {
            StringColumn returnValue = new StringColumn("Klasa");
            returnValue.Data.Clear();

            StringColumn standardColumn = _list.Columns.First();
            StringColumn nameColumn = _list.Columns.Last();

            for (int chunkIndex = 0; chunkIndex < standardColumn.Data.Count; chunkIndex++)
            {
                returnValue.Data.Add(new DataChunk());

                DataChunk standardChunk = standardColumn.Data.ElementAt(chunkIndex);
                DataChunk nameChunk = nameColumn.Data.ElementAt(chunkIndex);

                for (int entryIndex = 0; entryIndex < standardChunk.Entries.Count; entryIndex++)
                {
                    string classEntry = string.Empty;

                    string standardEntry = standardChunk.Entries.ElementAt(entryIndex).ToUpper();
                    string nameEntry = nameChunk.Entries.ElementAt(entryIndex).ToUpper();

                    if (nameEntry.Equals("BOLT"))
                    {
                        classEntry = IsHVM(standardEntry) ? "10.9" : "8.8";
                    }

                    if (nameEntry.Contains("HILTI") || nameEntry.Contains("ANCHORBAR"))
                    {
                        classEntry = "8.8";
                    }

                    returnValue.GetLastChunk().Entries.Add(classEntry);
                }
            }

            return returnValue;
        }

        private static StringColumn GenerateNotesColumn()
        {
            StringColumn returnValue = new StringColumn("Uwagi");
            returnValue.Data.Clear();

            StringColumn standardColumn = _list.Columns.First();
            foreach (DataChunk chunk in standardColumn.Data)
            {
                returnValue.Data.Add(new DataChunk());
                foreach (string entry in chunk.Entries)
                {
                    returnValue.GetLastChunk().Entries.Add(string.Empty);
                }
            }

            return returnValue;
        }

        private static BoltsDeliveryContentType GetEntryContentType(int entryIndex)
        {
            IEnumerable<StringColumn> columns = _list.Columns;
            StringColumn sizeColumn = ColumnUtils.FindColumn(_list, "Size", out _);
            StringColumn nameColumn = ColumnUtils.FindColumn(_list, "Name", out _);
            StringColumn standardColumn = columns.First();

            string sizeEntry = sizeColumn.GetLastChunk().Entries.ElementAt(entryIndex).ToUpper();
            string nameEntry = nameColumn.GetLastChunk().Entries.ElementAt(entryIndex).ToUpper();
            string standardEntry = standardColumn.GetLastChunk().Entries.ElementAt(entryIndex).ToUpper();

            if (standardEntry.Contains("HILTI"))
            {
                return BoltsDeliveryContentType.Null;
            }

            if (nameEntry.Contains("HILTI"))
            {
                return BoltsDeliveryContentType.Hilti;
            }

            if (nameEntry.Contains("ANCHORBAR"))
            {
                return BoltsDeliveryContentType.Anchorbar;
            }

            if (sizeEntry.Contains("BOLT"))
            {
                return BoltsDeliveryContentType.Bolt;
            }

            if (sizeEntry.Contains("NUT"))
            {
                return BoltsDeliveryContentType.Nut;
            }

            if (sizeEntry.Contains("WASHER"))
            {
                return BoltsDeliveryContentType.Washer;
            }

            return BoltsDeliveryContentType.Null;
        }
    }
}