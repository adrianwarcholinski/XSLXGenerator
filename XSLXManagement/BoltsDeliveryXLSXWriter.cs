using Model;
using NPOI.SS.UserModel;

namespace XLSXManagement
{
    internal class BoltsDeliveryXLSXWriter
    {
        private static IWorkbook _workbook;
        private static ISheet _sheet;
        private static StructuralList _list;

        private static ICellStyle _borderStyle;
        private static ICellStyle _centerAlignmentStyle;
        private static ICellStyle _boldStyle;
        private static ICellStyle _summaryStyle;
        private static ICellStyle _finalSummaryStyle;

        public static void WriteDeliveryList(TeklaList list, string path)
        {
            int x = 3;
        }
    }
}