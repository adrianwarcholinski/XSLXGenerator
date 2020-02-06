using Model;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileManagement.Material
{
    public static class ListReader
    {
        public static TeklaList ReadMaterialList(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string content = File.ReadAllText(path, Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage));

            return new TeklaList(content);
        }
    }
}