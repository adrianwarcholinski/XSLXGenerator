using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace XLSXManagement
{
    internal class SheetFactory
    {
        public static ISheet CreateSheet(string title)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(title);

            return sheet;
        }
    }
}