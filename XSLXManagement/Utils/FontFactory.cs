using NPOI.SS.UserModel;

namespace XLSXManagement
{
    internal class FontFactory
    {
        public static IFont CreateBoldFont(IWorkbook workbook)
        {
            IFont font = workbook.CreateFont();
            font.Boldweight = (short) FontBoldWeight.Bold;
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";

            return font;
        }
    }
}