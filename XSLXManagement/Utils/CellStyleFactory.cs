using NPOI.SS.UserModel;

namespace XLSXManagement.Utils
{
    internal static class CellStyleFactory
    {
        private static ICellStyle _borderStyle;

        private static ICellStyle _centerAlignmentStyle;
        private static ICellStyle _centerAlignmentStyle0DecimalPts;
        private static ICellStyle _centerAlignmentStyle1DecimalPts;
        private static ICellStyle _centerAlignmentStyle2DecimalPts;

        private static ICellStyle _boldFontStyle;

        private static ICellStyle _summaryStyle;
        private static ICellStyle _summaryStyle0DecimalPts;
        private static ICellStyle _summaryStyle1DecimalPts;
        private static ICellStyle _summaryStyle2DecimalPts;

        private static ICellStyle _finalSummaryStyle;
        private static ICellStyle _finalSummaryStyle0DecimalPts;
        private static ICellStyle _finalSummaryStyle1DecimalPts;
        private static ICellStyle _finalSummaryStyle2DecimalPts;

        private static IDataFormat _0DecimalPtsFormat;
        private static IDataFormat _1DecimalPtsFormat;
        private static IDataFormat _2DecimalPtsFormat;

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

        public static ICellStyle CreateCenterAlignmentStyle0DecimalPts(IWorkbook workbook)
        {
            if (_centerAlignmentStyle0DecimalPts != null)
            {
                return _centerAlignmentStyle0DecimalPts;
            }

            _centerAlignmentStyle0DecimalPts = workbook.CreateCellStyle();
            _centerAlignmentStyle0DecimalPts.CloneStyleFrom(CreateCenterAlignmentStyle(workbook));
            _centerAlignmentStyle0DecimalPts.DataFormat = Create0DecimalPtsDataFormat(workbook);

            return _centerAlignmentStyle0DecimalPts;
        }

        public static ICellStyle CreateCenterAlignmentStyle1DecimalPts(IWorkbook workbook)
        {
            if (_centerAlignmentStyle1DecimalPts != null)
            {
                return _centerAlignmentStyle1DecimalPts;
            }

            _centerAlignmentStyle1DecimalPts = workbook.CreateCellStyle();
            _centerAlignmentStyle1DecimalPts.CloneStyleFrom(CreateCenterAlignmentStyle(workbook));
            _centerAlignmentStyle1DecimalPts.DataFormat = Create1DecimalPtsDataFormat(workbook);

            return _centerAlignmentStyle1DecimalPts;
        }

        public static ICellStyle CreateCenterAlignmentStyle2DecimalPts(IWorkbook workbook)
        {
            if (_centerAlignmentStyle2DecimalPts != null)
            {
                return _centerAlignmentStyle2DecimalPts;
            }

            _centerAlignmentStyle2DecimalPts = workbook.CreateCellStyle();
            _centerAlignmentStyle2DecimalPts.CloneStyleFrom(CreateCenterAlignmentStyle(workbook));
            _centerAlignmentStyle2DecimalPts.DataFormat = Create2DecimalPtsDataFormat(workbook);

            return _centerAlignmentStyle2DecimalPts;
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

        public static ICellStyle CreateSummaryStyle0DecimalPts(IWorkbook workbook)
        {
            if (_summaryStyle0DecimalPts != null)
            {
                return _summaryStyle0DecimalPts;
            }

            _summaryStyle0DecimalPts = workbook.CreateCellStyle();
            _summaryStyle0DecimalPts.CloneStyleFrom(CreateSummaryStyle(workbook));
            _summaryStyle0DecimalPts.DataFormat = Create0DecimalPtsDataFormat(workbook);

            return _summaryStyle0DecimalPts;
        }
        public static ICellStyle CreateSummaryStyle1DecimalPts(IWorkbook workbook)
        {
            if (_summaryStyle1DecimalPts != null)
            {
                return _summaryStyle1DecimalPts;
            }

            _summaryStyle1DecimalPts = workbook.CreateCellStyle();
            _summaryStyle1DecimalPts.CloneStyleFrom(CreateSummaryStyle(workbook));
            _summaryStyle1DecimalPts.DataFormat = Create1DecimalPtsDataFormat(workbook);

            return _summaryStyle1DecimalPts;
        }

        public static ICellStyle CreateSummaryStyle2DecimalPts(IWorkbook workbook)
        {
            if (_summaryStyle2DecimalPts != null)
            {
                return _summaryStyle2DecimalPts;
            }

            _summaryStyle2DecimalPts = workbook.CreateCellStyle();
            _summaryStyle2DecimalPts.CloneStyleFrom(CreateSummaryStyle(workbook));
            _summaryStyle2DecimalPts.DataFormat = Create2DecimalPtsDataFormat(workbook);

            return _summaryStyle2DecimalPts;
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

        public static ICellStyle CreateFinalSummaryStyle0DecimalPts(IWorkbook workbook)
        {
            if (_finalSummaryStyle0DecimalPts != null)
            {
                return _finalSummaryStyle0DecimalPts;
            }

            _finalSummaryStyle0DecimalPts = workbook.CreateCellStyle();
            _finalSummaryStyle0DecimalPts.CloneStyleFrom(CreateFinalSummaryStyle(workbook));
            _finalSummaryStyle0DecimalPts.DataFormat = Create0DecimalPtsDataFormat(workbook);

            return _finalSummaryStyle0DecimalPts;
        }

        public static ICellStyle CreateFinalSummaryStyle1DecimalPts(IWorkbook workbook)
        {
            if (_finalSummaryStyle1DecimalPts != null)
            {
                return _finalSummaryStyle1DecimalPts;
            }

            _finalSummaryStyle1DecimalPts = workbook.CreateCellStyle();
            _finalSummaryStyle1DecimalPts.CloneStyleFrom(CreateFinalSummaryStyle(workbook));
            _finalSummaryStyle1DecimalPts.DataFormat = Create1DecimalPtsDataFormat(workbook);

            return _finalSummaryStyle1DecimalPts;
        }

        public static ICellStyle CreateFinalSummaryStyle2DecimalPts(IWorkbook workbook)
        {
            if (_finalSummaryStyle2DecimalPts != null)
            {
                return _finalSummaryStyle2DecimalPts;
            }

            _finalSummaryStyle2DecimalPts = workbook.CreateCellStyle();
            _finalSummaryStyle2DecimalPts.CloneStyleFrom(CreateFinalSummaryStyle(workbook));
            _finalSummaryStyle2DecimalPts.DataFormat = Create2DecimalPtsDataFormat(workbook);

            return _finalSummaryStyle2DecimalPts;
        }

        public static void ResetStyles()
        {
            _borderStyle = _centerAlignmentStyle = _centerAlignmentStyle0DecimalPts =
                _centerAlignmentStyle1DecimalPts = _centerAlignmentStyle2DecimalPts = _boldFontStyle = _summaryStyle =
                    _summaryStyle0DecimalPts =
                        _summaryStyle1DecimalPts = _summaryStyle2DecimalPts = _finalSummaryStyle =
                            _finalSummaryStyle0DecimalPts =
                                _finalSummaryStyle1DecimalPts = _finalSummaryStyle2DecimalPts = null;

            _0DecimalPtsFormat = _1DecimalPtsFormat = _2DecimalPtsFormat = null;
        }

        private static short Create0DecimalPtsDataFormat(IWorkbook workbook)
        {
            if (_0DecimalPtsFormat == null)
            {
                _0DecimalPtsFormat = workbook.CreateDataFormat();
            }

            return _0DecimalPtsFormat.GetFormat("#,##0");
        }

        private static short Create1DecimalPtsDataFormat(IWorkbook workbook)
        {
            if (_1DecimalPtsFormat == null)
            {
                _1DecimalPtsFormat = workbook.CreateDataFormat();
            }

            return _1DecimalPtsFormat.GetFormat("#,##0.0");
        }

        private static short Create2DecimalPtsDataFormat(IWorkbook workbook)
        {
            if (_2DecimalPtsFormat == null)
            {
                _2DecimalPtsFormat = workbook.CreateDataFormat();
            }

            return _2DecimalPtsFormat.GetFormat("#,##0.00");
        }
    }
}