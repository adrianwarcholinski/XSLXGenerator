using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Model.Material;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace XLSXManagement.Material
{
    public static class MaterialXLSXWriter
    {
        public static void WriteMaterialList(MaterialList list, string path)
        {
            ISheet sheet = GetSheet("lista", out IWorkbook workbook);
            WriteWorkbook(workbook, path);
        }

        private static ISheet GetSheet(string name, out IWorkbook workbook)
        {
            workbook = new XSSFWorkbook();
            return workbook.CreateSheet(name);
        }

        private static void WriteWorkbook(IWorkbook workbook, string path)
        {
            using FileStream stream = File.Open(path, FileMode.Create, FileAccess.ReadWrite);
            workbook.Write(stream);

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };

            process.Start();
        }
    }
}