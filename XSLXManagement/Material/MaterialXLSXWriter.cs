using System.Collections.Generic;
using Model.Material;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace XLSXManagement.Material
{
    public static class MaterialXLSXWriter
    {
        private static IWorkbook _workbook;
        private static ISheet _sheet;
        private static MaterialList _list;

        public static void WriteMaterialList(MaterialList list, string path)
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
        }

        private static void AutosizeAllColumns()
        {
            int numColumns = _list.Columns.Count;
            for (int i = 0; i < numColumns; i++)
            {
                _sheet.AutoSizeColumn(i);
            }
        }

        private static ICellStyle GetCenterAlignmentStyle()
        {
            ICellStyle style = GetBorderStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            return style;
        }

        private static ICellStyle GetBorderStyle()
        {
            ICellStyle style = _workbook.CreateCellStyle();
            style.BorderTop = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;

            return style;
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
                cell.CellStyle = GetCenterAlignmentStyle();
            }
        }

        private static void WriteKSType()
        {
            IRow row = GetNewRow();
            int numColumns = _list.Columns.Count;
            int currentRow = _sheet.LastRowNum;

            ICell cell = row.CreateCell(0);
            cell.SetCellValue("KS - typ");

            ICellStyle style = GetCenterAlignmentStyle();
            style.SetFont(GetBoldFont());

            cell.CellStyle = style;

            for (int i = 1; i < numColumns; i++)
            {
                row.CreateCell(i).CellStyle = style;
            }

            CellRangeAddress region = new CellRangeAddress(currentRow, currentRow, 0, numColumns - 1);

            _sheet.AddMergedRegion(region);
        }

        private static IFont GetBoldFont()
        {
            IFont font = _workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
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