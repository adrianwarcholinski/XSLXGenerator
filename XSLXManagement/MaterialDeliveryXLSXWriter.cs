using Model;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace XLSXManagement
{
    internal static class MaterialDeliveryXLSXWriter
    {
        private static IWorkbook _workbook;
        private static ISheet _sheet;
        private static TeklaList _list;

        private static ICellStyle _borderStyle;
        private static ICellStyle _centerAlignmentStyle;
        private static ICellStyle _boldStyle;
        private static ICellStyle _summaryStyle;
        private static ICellStyle _finalSummaryStyle;

        public static void WriteMaterialList(TeklaList list, string path)
        {
            InitWorkbook();
            _list = list;

            WriteColumnsNames();
            WriteKSType();
            WriteData();

            AutosizeAllColumns();
            WriteWorkbook(path);
        }

        private static void InitWorkbook()
        {
            _workbook = new XSSFWorkbook();
            _sheet = _workbook.CreateSheet("lista");

            InitStyles();
        }

        private static void AutosizeAllColumns()
        {
            int numColumns = _list.Columns.Count;
            for (int i = 0; i < numColumns; i++)
            {
                _sheet.AutoSizeColumn(i);
            }
        }

        private static void InitStyles()
        {
            InitBorderStyle();
            InitCenterAlignmentStyle();
            InitBoldStyle();
            InitSummaryStyle();
            InitFinalSummaryStyle();
        }

        private static void InitBorderStyle()
        {
            _borderStyle = _workbook.CreateCellStyle();
            _borderStyle.BorderTop = BorderStyle.Thin;
            _borderStyle.BorderRight = BorderStyle.Thin;
            _borderStyle.BorderBottom = BorderStyle.Thin;
            _borderStyle.BorderLeft = BorderStyle.Thin;
        }

        private static void InitCenterAlignmentStyle()
        {
            _centerAlignmentStyle = _workbook.CreateCellStyle();
            _centerAlignmentStyle.CloneStyleFrom(_borderStyle);
            _centerAlignmentStyle.Alignment = HorizontalAlignment.Center;
            _centerAlignmentStyle.VerticalAlignment = VerticalAlignment.Center;
        }

        private static void InitBoldStyle()
        {
            _boldStyle = _workbook.CreateCellStyle();
            _boldStyle.CloneStyleFrom(_centerAlignmentStyle);
            _boldStyle.SetFont(GetBoldFont());
        }

        private static void InitSummaryStyle()
        {
            _summaryStyle = _workbook.CreateCellStyle();
            _summaryStyle.CloneStyleFrom(_boldStyle);
            _summaryStyle.FillForegroundColor = IndexedColors.Aqua.Index;
            _summaryStyle.FillPattern = FillPattern.SolidForeground;
        }

        private static void InitFinalSummaryStyle()
        {
            _finalSummaryStyle = _workbook.CreateCellStyle();
            _finalSummaryStyle.CloneStyleFrom(_summaryStyle);
            _finalSummaryStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        }

        private static void WriteColumnsNames()
        {
            IRow row = GetNewRow();
            row.Height = 900;

            int numColumns = _list.Columns.Count;

            for (int i = 0; i < numColumns; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(_list.Columns.ElementAt(i).Name);
                cell.CellStyle = _centerAlignmentStyle;
            }
        }

        private static void WriteKSType()
        {
            IRow row = GetNewRow();
            int numColumns = _list.Columns.Count;
            int currentRow = _sheet.LastRowNum;

            ICell cell = row.CreateCell(0);
            cell.SetCellValue("KS - typ");

            cell.CellStyle = _boldStyle;

            for (int i = 1; i < numColumns; i++)
            {
                row.CreateCell(i).CellStyle = _boldStyle;
            }

            CellRangeAddress region = new CellRangeAddress(currentRow, currentRow, 0, numColumns - 1);

            _sheet.AddMergedRegion(region);
        }

        private static IFont GetBoldFont()
        {
            IFont font = _workbook.CreateFont();
            font.Boldweight = (short) FontBoldWeight.Bold;
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";

            return font;
        }

        private static IRow GetNewRow()
        {
            return _sheet.CreateRow(_sheet.LastRowNum + 1);
        }

        private static void WriteData()
        {
            ICollection<StringColumn> columns = _list.Columns;
            int numDataChunks = columns.First().Data.Count;
            int numColumns = _list.Columns.Count;

            for (int i = 0; i < numDataChunks - 1; i++)
            {
                int rowCount = columns.First().Data.ElementAt(i).Entries.Count;
                for (int r = 0; r < rowCount; r++)
                {
                    IRow row = GetNewRow();
                    for (int c = 0; c < numColumns; c++)
                    {
                        ICell cell = row.CreateCell(c);
                        cell.CellStyle = _centerAlignmentStyle;

                        string entry = columns.ElementAt(c).Data.ElementAt(i).Entries.ElementAt(r);
                        bool isNumber = double.TryParse(entry, NumberStyles.Any, CultureInfo.InvariantCulture,
                            out double result);

                        if (isNumber && columns.ElementAt(c).Name != TranslateUtils.Translate("Part"))
                        {
                            cell.SetCellValue(SeparateThousands(entry));
                        }
                        else
                        {
                            cell.SetCellValue(entry);
                        }
                    }
                }

                IRow summaryRow = GetNewRow();
                int emptySummaryCount = 0;
                bool isFinal = i == numDataChunks - 2;
                ICellStyle summaryStyle = isFinal ? _finalSummaryStyle : _summaryStyle;
                string sumLabel = isFinal ? "Suma całkowita" : "Suma";

                for (int c = 0; c < numColumns; c++)
                {
                    string summary = columns.ElementAt(c).Data.ElementAt(i).Summary;

                    ICell cell = summaryRow.CreateCell(c);
                    cell.CellStyle = summaryStyle;

                    if (string.IsNullOrEmpty(summary) || summary.Contains("Tota"))
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
                            cell.SetCellValue(SeparateThousands(summary));
                        }
                        else
                        {
                            emptySummaryCount++;
                        }
                    }
                }

                CellRangeAddress region =
                    new CellRangeAddress(_sheet.LastRowNum, _sheet.LastRowNum, 0, emptySummaryCount - 1);
                _sheet.AddMergedRegion(region);
            }
        }

        private static string SeparateThousands(string number)
        {
            double d = double.Parse(number, NumberStyles.Any, CultureInfo.InvariantCulture);
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            string str = d.ToString("#,0.00", nfi).Replace(".", ",").Replace(",00", "");
            if (str.Last() == '0' && str.Contains(","))
            {
                return str.Remove(str.Length - 1, 1);
            }

            return str;
        }

        private static void WriteWorkbook(string path)
        {
            using FileStream stream = File.Open(path, FileMode.Create, FileAccess.ReadWrite);
            _workbook.Write(stream);

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };

            process.Start();
        }
    }
}