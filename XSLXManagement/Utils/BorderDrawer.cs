using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace XLSXManagement.Utils
{
    public class BorderDrawer
    {
        private static int BORDER_THICKNESS = 2;
        
        public static void drawBorders(ISheet sheet, int firstDataRow, int lastDataRow, int lastDataColumn)
        {
            CellRangeAddress firstDataEntryAddress = new CellRangeAddress(firstDataRow, firstDataRow, 0, lastDataColumn);
            RegionUtil.SetBorderTop(BORDER_THICKNESS, firstDataEntryAddress, sheet, sheet.Workbook);
                
            CellRangeAddress lastDataEntryAddress = new CellRangeAddress(lastDataRow, lastDataRow, 0, lastDataColumn);
            RegionUtil.SetBorderBottom(BORDER_THICKNESS, lastDataEntryAddress, sheet, sheet.Workbook);
            
            CellRangeAddress lastColumnAddress = new CellRangeAddress(firstDataRow - 1, lastDataRow, lastDataColumn, lastDataColumn);
            RegionUtil.SetBorderRight(BORDER_THICKNESS, lastColumnAddress, sheet, sheet.Workbook);
        }
    }
}