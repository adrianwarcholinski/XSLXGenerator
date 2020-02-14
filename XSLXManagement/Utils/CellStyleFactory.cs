using NPOI.SS.Formula.Udf;
using NPOI.SS.UserModel;

namespace XLSXManagement
{
    internal static class CellStyleFactory
    {
        private static ICellStyle _borderStyle;
        private static ICellStyle _centerAlignmentStyle;
        private static ICellStyle _boldFontStyle;
        private static ICellStyle _summaryStyle;
        private static ICellStyle _finalSummaryStyle;

        public static ICellStyle CreateBorderStyle(IWorkbook workbook)
        {
            if (_borderStyle != null)
            {
                return _borderStyle;
            }

            _borderStyle = workbook.CreateCellStyle();
            _borderStyle.BorderTop = BorderStyle.Thin;
            _borderStyle.BorderRight = BorderStyle.Thin;
            _borderStyle.BorderBottom = BorderStyle.Thin;
            _borderStyle.BorderLeft = BorderStyle.Thin;

            return _borderStyle;
        }

        public static ICellStyle CreateCenterAlignmentStyle(IWorkbook workbook)
        {
            if (_centerAlignmentStyle != null)
            {
                return _centerAlignmentStyle;
            }

            _centerAlignmentStyle = workbook.CreateCellStyle();
            _centerAlignmentStyle.CloneStyleFrom(CreateBorderStyle(workbook));
            _centerAlignmentStyle.Alignment = HorizontalAlignment.Center;
            _centerAlignmentStyle.VerticalAlignment = VerticalAlignment.Center;

            return _centerAlignmentStyle;
        }

        public static ICellStyle CreateBoldFontStyle(IWorkbook workbook)
        {
            if (_boldFontStyle != null)
            {
                return _boldFontStyle;
            }

            _boldFontStyle = workbook.CreateCellStyle();
            _boldFontStyle.CloneStyleFrom(CreateCenterAlignmentStyle(workbook));
            _boldFontStyle.SetFont(FontFactory.CreateBoldFont(workbook));

            return _boldFontStyle;
        }

        public static ICellStyle CreateSummaryStyle(IWorkbook workbook)
        {
            if (_summaryStyle != null)
            {
                return _summaryStyle;
            }

            _summaryStyle = workbook.CreateCellStyle();
            _summaryStyle.CloneStyleFrom(CreateBoldFontStyle(workbook));
            _summaryStyle.FillForegroundColor = IndexedColors.Aqua.Index;
            _summaryStyle.FillPattern = FillPattern.SolidForeground;

            return _summaryStyle;
        }

        public static ICellStyle CreateFinalSummaryStyle(IWorkbook workbook)
        {
            if (_finalSummaryStyle != null)
            {
                return _finalSummaryStyle;
            }

            _finalSummaryStyle = workbook.CreateCellStyle();
            _finalSummaryStyle.CloneStyleFrom(CreateSummaryStyle(workbook));
            _finalSummaryStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;

            return _finalSummaryStyle;
        }

        public static void ResetStyles()
        {
            _borderStyle = _centerAlignmentStyle = _boldFontStyle = _summaryStyle = _finalSummaryStyle = null;
        }
    }
}