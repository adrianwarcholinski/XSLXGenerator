using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace XLSXManagement.Utils
{
    internal class SheetFactory
    {
        public static ISheet CreateSheet(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Copy("naglowek.xlsx", path);

            IWorkbook workbook;

            using (FileStream stream = File.OpenRead(path))
            {
                workbook = new XSSFWorkbook(stream);
            }

            return workbook.GetSheetAt(0);
        }
    }
}