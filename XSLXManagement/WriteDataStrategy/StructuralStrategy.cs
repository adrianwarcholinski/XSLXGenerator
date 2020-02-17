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


                        if (isNumber)
                        {
                            int decimalPts = columns.ElementAt(columnIndex).GetNumDecimalPlaces();
                            switch (decimalPts)
                            {
                                case -1:
                                    cell.SetCellValue(entry);
                                    break;

                                case 0:
                                    cell.CellStyle = CellStyleFactory.CreateCenterAlignmentStyle0DecimalPts(sheet.Workbook);
                                    cell.SetCellValue(double.Parse(entry, NumberStyles.Any, CultureInfo.InvariantCulture));
                                    break;

                                case 1:
                                    cell.CellStyle = CellStyleFactory.CreateCenterAlignmentStyle1DecimalPts(sheet.Workbook);
                                    cell.SetCellValue(double.Parse(entry, NumberStyles.Any, CultureInfo.InvariantCulture));
                                    break;

                                default:
                                    cell.CellStyle = CellStyleFactory.CreateCenterAlignmentStyle2DecimalPts(sheet.Workbook);
                                    cell.SetCellValue(double.Parse(entry, NumberStyles.Any, CultureInfo.InvariantCulture));
                                    break;
                            }
                        }
                        else
                        {
                            cell.SetCellValue(entry.Trim());
                        }

                        if (isFirstRow)
                        {
                            if (isNumber)
                            {
                                int decimalPts = columns.ElementAt(columnIndex).GetNumDecimalPlaces();

                                switch (decimalPts)
                                {
                                    case -1:
                                        cell.SetCellValue(entry);
                                        cell.CellStyle = CellStyleFactory.CreateSummaryStyle(sheet.Workbook);
                                        break;

                                    case 0:
                                        cell.CellStyle = CellStyleFactory.CreateSummaryStyle0DecimalPts(sheet.Workbook);
                                        cell.SetCellValue(double.Parse(entry, NumberStyles.Any, CultureInfo.InvariantCulture));
                                        break;

                                    case 1:
                                        cell.CellStyle = CellStyleFactory.CreateSummaryStyle1DecimalPts(sheet.Workbook);
                                        cell.SetCellValue(double.Parse(entry, NumberStyles.Any, CultureInfo.InvariantCulture));
                                        break;

                                    default:
                                        cell.CellStyle = CellStyleFactory.CreateSummaryStyle2DecimalPts(sheet.Workbook);
                                        cell.SetCellValue(double.Parse(entry, NumberStyles.Any, CultureInfo.InvariantCulture));
                                        break;
                                }
                            }
                            else
                            {
                                cell.CellStyle = CellStyleFactory.CreateSummaryStyle(sheet.Workbook);
                            }
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
                    int decimalPts = columns.ElementAt(columnIndex).GetNumDecimalPlaces();
                    switch (decimalPts)
                    {
                        case -1:
                            cell.SetCellValue(((StructuralList)list).WeightSummary);
                            break;

                        case 0:
                            cell.CellStyle =
                                CellStyleFactory.CreateFinalSummaryStyle0DecimalPts(sheet.Workbook);
                            cell.SetCellValue(double.Parse(((StructuralList)list).WeightSummary, NumberStyles.Any, CultureInfo.InvariantCulture));
                            break;

                        case 1:
                            cell.CellStyle =
                                CellStyleFactory.CreateFinalSummaryStyle1DecimalPts(sheet.Workbook);
                            cell.SetCellValue(double.Parse(((StructuralList)list).WeightSummary, NumberStyles.Any, CultureInfo.InvariantCulture));
                            break;

                        default:
                            cell.CellStyle =
                                CellStyleFactory.CreateFinalSummaryStyle2DecimalPts(sheet.Workbook);
                            cell.SetCellValue(double.Parse(((StructuralList)list).WeightSummary, NumberStyles.Any, CultureInfo.InvariantCulture));
                            break;
                    }
                    // cell.SetCellValue(SheetUtils.SeparateThousands(((StructuralList) list).WeightSummary, false));
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