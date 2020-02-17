using Model;
using Model.List;
using NPOI.SS.UserModel;

namespace XLSXManagement.WriteHeaderStrategy
{
    public class HeaderStrategy : IXLSXWriteHeaderStrategy
    {
        public void FormatHeader(AbstractList list, ISheet sheet, ListType listType)
        {
            for (int rowNum = sheet.FirstRowNum; rowNum < sheet.LastRowNum; rowNum++)
            {
                IRow row = sheet.GetRow(rowNum);
                for (int cellNum = row.FirstCellNum; cellNum < row.LastCellNum; cellNum++)
                {
                    ICell cell = row.GetCell(cellNum);
                    string originalContent = cell.StringCellValue;

                    string newContent = originalContent.Replace("<rodzaj listy>", GetListTypeString(listType))
                        .Replace("<data>", list.Header.Date)
                        .Replace("<nr projektu>", list.Header.No);

                    cell.SetCellValue(newContent);
                }

                sheet.Workbook.SetSheetName(0, GetListTypeString(listType));
            }
        }

        private string GetListTypeString(ListType type)
        {
            switch (type)
            {
                case ListType.Delivery:
                    return "Lista wysyłkowa";

                case ListType.BoltsDelivery:
                    return "Lista śrub";

                case ListType.Structural:
                    return "Lista strukturalna";

                case ListType.Material:
                    return "Lista materiałowa";

                default:
                    return string.Empty;
            }
        }
    }
}