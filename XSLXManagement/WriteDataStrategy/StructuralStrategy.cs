using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace XLSXManagement
{
    internal class StructuralStrategy : IXLSXWriteDataStrategy
    {
        public void WriteData(AbstractList list, ISheet sheet)
        {
            ICollection<StringColumn> columns = list.Columns;
            int numDataChunks = columns.First().Data.Count;
            int numColumns = columns.Count;

            IEnumerable<string> nonDividableColumns = TranslateUtils.GetTranslatedStrings(new[] { "Assembly", "Part", "No." });

            for (int chunkIndex = 0; chunkIndex < numDataChunks; chunkIndex++)
            {
                int numEntries = columns.First().Data.ElementAt(chunkIndex).Entries.Count;
                for (int entryIndex = 0; entryIndex < numEntries; entryIndex++)
                {
                    IRow row = SheetUtils.CreateRow(sheet);

                    string assemblyEntry = columns.First().Data.ElementAt(chunkIndex).Entries.ElementAt(entryIndex);
                    bool isFirstRow = !string.IsNullOrEmpty(assemblyEntry);

                    for (int columnIndex = 0; columnIndex < numColumns; columnIndex++)
                    {
                        ICell cell = row.CreateCell(columnIndex);
                        cell.CellStyle = CellStyleFactory.CreateCenterAlignmentStyle(sheet.Workbook);
                        string entry = columns.ElementAt(columnIndex).Data.ElementAt(chunkIndex).Entries
                            .ElementAt(entryIndex);

                        bool isNumber = double.TryParse(entry, NumberStyles.Any, CultureInfo.InvariantCulture,
                            out double result);

                        string columnName = columns.ElementAt(columnIndex).Name;

                        if (isNumber && !nonDividableColumns.Contains(columnName))
                        {
                            IEnumerable<string> intColumns = TranslateUtils.GetTranslatedStrings(new[] { "Length" });
                            bool isInteger = intColumns.Any(name => columnName.Contains(name));
                            cell.SetCellValue(SheetUtils.SeparateThousands(entry, !isInteger));
                        }
                        else
                        {
                            cell.SetCellValue(entry.Trim());
                        }

                        if (isFirstRow)
                        {
                            cell.CellStyle = CellStyleFactory.CreateSummaryStyle(sheet.Workbook); ;
                        }
                    }
                }
            }

            IRow summaryRow = SheetUtils.CreateRow(sheet);
            int emptySummaryCount = 0;
            string weightColumnName = TranslateUtils.Translate("Weight");

            for (int columnIndex = 0; columnIndex < columns.Count; columnIndex++)
            {
                ICell cell = summaryRow.CreateCell(columnIndex);
                cell.CellStyle = CellStyleFactory.CreateFinalSummaryStyle(sheet.Workbook);

                if (columns.ElementAt(columnIndex).Name.Contains(weightColumnName))
                {
                    cell.SetCellValue(SheetUtils.SeparateThousands(((StructuralList) list).WeightSummary, false));
                }
                else
                {
                    cell.SetCellValue("Suma całkowita");
                    emptySummaryCount++;
                }
            }

            CellRangeAddress region =
                new CellRangeAddress(sheet.LastRowNum, sheet.LastRowNum, 0, emptySummaryCount - 1);
            sheet.AddMergedRegion(region);
        }
    }
}