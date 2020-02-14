using NPOI.SS.UserModel;
using System.Globalization;
using System.Linq;

namespace XLSXManagement
{
    internal class SheetUtils
    {
        public static IRow CreateRow(ISheet sheet)
        {
            return sheet.CreateRow(sheet.LastRowNum + 1);
        }

        public static string SeparateThousands(string number, bool addTrailingZero)
        {
            double d = double.Parse(number, NumberStyles.Any, CultureInfo.InvariantCulture);
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            string str = d.ToString("#,0.00", nfi).Replace(".", ",").Replace(",00", "");

            if (str.Last() == '0' && str.Contains(","))
            {
                str = str.Remove(str.Length - 1, 1);
            }

            if (!str.Contains(",") && addTrailingZero)
            {
                str += ",0";
            }

            return str;
        }
    }
}