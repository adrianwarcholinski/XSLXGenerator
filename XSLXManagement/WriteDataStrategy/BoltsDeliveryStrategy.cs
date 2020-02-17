using Model.DataModel;
using Model.List;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

                        if (isNumber)
                        {
                            int decimalPts = columns.ElementAt(c).GetNumDecimalPlaces();
                            switch (decimalPts)
                            {
                                case -1:
                                    cell.SetCellValue(entry);
                                    break;

                                case 0:
                                    cell.CellStyle =
                                        CellStyleFactory.CreateCenterAlignmentStyle0DecimalPts(sheet.Workbook);
                                    cell.SetCellValue(double.Parse(entry, NumberStyles.Any, CultureInfo.InvariantCulture));
                                    break;

                                case 1:
                                    cell.CellStyle =
                                        CellStyleFactory.CreateCenterAlignmentStyle1DecimalPts(sheet.Workbook);
                                    cell.SetCellValue(double.Parse(entry, NumberStyles.Any, CultureInfo.InvariantCulture));
                                    break;

                                default:
                                    cell.CellStyle =
                                        CellStyleFactory.CreateCenterAlignmentStyle2DecimalPts(sheet.Workbook);
                                    cell.SetCellValue(double.Parse(entry, NumberStyles.Any, CultureInfo.InvariantCulture));
                                    break;
                            }
                        }
                        else
                        {
                            cell.SetCellValue(entry);
                        }
                    }

                    for (int notesIndex = 0; notesIndex < 2; notesIndex++)
                    {
                        ICell notesCell = row.CreateCell(numColumns + notesIndex);
                        // if ()
                        notesCell.CellStyle = CellStyleFactory.CreateCenterAlignmentStyle(sheet.Workbook);
                    }

                    CellRangeAddress region =
                        new CellRangeAddress(sheet.LastRowNum, sheet.LastRowNum, numColumns - 1, numColumns + 1);
                    sheet.AddMergedRegion(region);

                }

                if (i == numDataChunks - 1)
                {
                    continue;
                }

                IRow blankRow = SheetUtils.CreateRow(sheet);

                for (int c = 0; c < numColumns + 2; c++)
                {
                    ICell cell = blankRow.CreateCell(c);
                    cell.CellStyle = CellStyleFactory.CreateFinalSummaryStyle(sheet.Workbook);
                }

                CellRangeAddress blankRegion =
                    new CellRangeAddress(sheet.LastRowNum, sheet.LastRowNum, numColumns - 1, numColumns + 1);
                sheet.AddMergedRegion(blankRegion);
            }
        }
    }
}