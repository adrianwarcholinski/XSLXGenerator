using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model;
using Model.DataModel;
using Model.List;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using XLSXManagement.Utils;

namespace XLSXManagement.WriteDataStrategy
{
    public class BoltsDeliveryStrategy : IXLSXWriteDataStrategy
    {
        public void WriteData(AbstractList list, ISheet sheet)
        {
            ICollection<StringColumn> columns = list.Columns;
            int numDataChunks = columns.First().Data.Count;
            int numColumns = columns.Count;

            for (int i = 0; i < numDataChunks; i++)
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
                        cell.SetCellValue(entry);
                    }
                }

                if (i == numDataChunks - 1)
                {
                    continue;
                }

                IRow blankRow = SheetUtils.CreateRow(sheet);

                for (int c = 0; c < numColumns; c++)
                {
                    string summary = columns.ElementAt(c).Data.ElementAt(i).Summary;

                    ICell cell = blankRow.CreateCell(c);
                    cell.CellStyle = CellStyleFactory.CreateCenterAlignmentStyle(sheet.Workbook);

                    // if (string.IsNullOrEmpty(summary) || summary.Contains("Tota") || summary.Contains("for"))
                    // {
                    //     emptySummaryCount++;
                    //     cell.SetCellValue(sumLabel);
                    // }
                    // else
                    // {
                    //     bool isNumber = double.TryParse(summary, NumberStyles.Any, CultureInfo.InvariantCulture,
                    //         out double result);
                    //     if (isNumber)
                    //     {
                    //         cell.SetCellValue(SheetUtils.SeparateThousands(summary, true));
                    //     }
                    //     else
                    //     {
                    //         emptySummaryCount++;
                    //     }
                    // }
                    //
                    // if (!string.IsNullOrEmpty(summary) && summary.Contains("for") && summary.Length == 3)
                    // {
                    //     emptySummaryCount++;
                    // }
                }
            }
        }
    }
}