using System.Globalization;
using System.IO;
using System.Text;
using Model.Material;

namespace FileManagement.Material
{
    public static class MaterialListReader
    {
        public static MaterialList ReadMaterialList(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string content = File.ReadAllText(path, Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage));

            return new MaterialList(content);
        }
    }
}