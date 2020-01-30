using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace XLSXManagement.Material
{
    public static class MaterialXLSXWriter
    {
        public static void WriteMaterialList(string path)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("lista");

            using (FileStream stream = File.Open(path, FileMode.Create, FileAccess.ReadWrite))
            {
                workbook.Write(stream);

                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                };

                process.Start();
            }
        }

        private static string CutFileExtension(string fileName)
        {
            string extension = fileName.Split(".").Last();
            return fileName.Replace($".{extension}", "");
        }
    }
}