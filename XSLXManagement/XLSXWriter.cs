using Model.List;
using NPOI.SS.UserModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Model;
using XLSXManagement.Utils;
using XLSXManagement.WriteDataStrategy;
using XLSXManagement.WriteHeaderStrategy;

namespace XLSXManagement
{
    internal static class XLSXWriter
    {
        private static ISheet _sheet;
        private static AbstractList _list;

        public static void WriteList(AbstractList list, string path, IXLSXWriteDataStrategy writeDataStrategy, IXLSXWriteHeaderStrategy headerStrategy, ListType listType)
        {
            CellStyleFactory.ResetStyles();

            _sheet = SheetFactory.CreateSheet(path);
            _list = list;
            
            headerStrategy.FormatHeader(_list, _sheet, listType);

            WriteColumnsNames();

            writeDataStrategy.WriteData(list, _sheet);

            AutosizeAllColumns();
            WriteWorkbook(path);
            OpenWorkbook(path);
        }

        private static void AutosizeAllColumns()
        {
            int numColumns = _list.Columns.Count;
            for (int i = 0; i < numColumns; i++)
            {
                _sheet.AutoSizeColumn(i);
            }
        }

        private static void WriteColumnsNames()
        {
            IRow row = SheetUtils.CreateRow(_sheet);
            row.Height = 900;

            int numColumns = _list.Columns.Count;

            for (int i = 0; i < numColumns; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(_list.Columns.ElementAt(i).Name);
                cell.CellStyle = CellStyleFactory.CreateCenterAlignmentStyle(_sheet.Workbook);
            }
        }

        private static void WriteWorkbook(string path)
        {
            using (FileStream stream = File.Create(path))
            {
                _sheet.Workbook.Write(stream);
            }
        }

        private static void OpenWorkbook(string path)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };

            process.Start();
        }
    }
}