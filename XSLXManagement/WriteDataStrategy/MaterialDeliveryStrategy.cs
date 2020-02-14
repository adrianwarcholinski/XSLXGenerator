using Model;
using Model.DataModel;
using Model.List;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using XLSXManagement.Utils;

namespace XLSXManagement.WriteDataStrategy
{
    internal class MaterialDeliveryStrategy : IXLSXWriteDataStrategy
    {
        public void WriteData(AbstractList list, ISheet sheet)
        {
            ICollection<StringColumn> columns = list.Columns;
            int numDataChunks = columns.First().Data.Count;
            int numColumns = columns.Count;

            IEnumerable<string> intColumns = new[] {"Asmbly Pos.", "No."};

            for (int i = 0; i < numDataChunks - 1; i++)
            {
                int rowCount = columns.First().Data.ElementAt(i).Entries.Count;
                for (int r = 0; r < rowCount; r++)
                {
                    IRow row = SheetUtils.CreateRow(sheet);
                    for (int c = 0; c < numColumns; c++)
                    {
                        ICell cell = row.CreateCell(c);
                        cell.CellStyle = CellStyleFactory.CreateCenterAlignmentStyle(sheet.Workbook);

                        string entry = columns.ElementAt(c).Data.ElementAt(i).Entries.ElementAt(r);
                        bool isNumber = double.TryParse(entry, NumberStyles.Any, CultureInfo.InvariantCulture,
                            out double result);

                        string columnName = columns.ElementAt(c).Name;

                        if (isNumber && !columnName.Contains(TranslateUtils.Translate("Part"))
                                     && !columnName.Contains(TranslateUtils.Translate("No."))
                                     && !columnName.Contains(TranslateUtils.Translate("Asmbly Pos."))
                                     && !columnName.Contains(TranslateUtils.Translate("Length")))
                        {
                            cell.SetCellValue(SheetUtils.SeparateThousands(entry, true));
                        }
                        else
                        {
                            cell.SetCellValue(entry);
                        }
                    }
                }

                IRow summaryRow = SheetUtils.CreateRow(sheet);
                int emptySummaryCount = 0;
                bool isFinal = i == numDataChunks - 2;
                ICellStyle summaryStyle = isFinal
                    ? CellStyleFactory.CreateFinalSummaryStyle(sheet.Workbook)
                    : CellStyleFactory.CreateSummaryStyle(sheet.Workbook);
                string sumLabel = isFinal ? "Suma całkowita" : "Suma";

                for (int c = 0; c < numColumns; c++)
                {
                    string summary = columns.ElementAt(c).Data.ElementAt(i).Summary;

                    ICell cell = summaryRow.CreateCell(c);
                    cell.CellStyle = summaryStyle;

                    if (string.IsNullOrEmpty(summary) || summary.Contains("Tota") || summary.Contains("for"))
                    {
                        emptySummaryCount++;
                        cell.SetCellValue(sumLabel);
                    }
                    else
                    {
                        bool isNumber = double.TryParse(summary, NumberStyles.Any, CultureInfo.InvariantCulture,
                            out double result);
                        if (isNumber)
                        {
                            cell.SetCellValue(SheetUtils.SeparateThousands(summary, true));
                        }
                        else
                        {
                            emptySummaryCount++;
                        }
                    }

                    if (!string.IsNullOrEmpty(summary) && summary.Contains("for") && summary.Length == 3)
                    {
                        emptySummaryCount++;
                    }
                }

                CellRangeAddress region =
                    new CellRangeAddress(sheet.LastRowNum, sheet.LastRowNum, 0, emptySummaryCount - 1);
                sheet.AddMergedRegion(region);
            }
        }
    }
}