using Model;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileManagement
{
    public static class ListReader
    {
        public static AbstractList ReadList(ListType type, string path)
        {
            string content = GetFileContent(path);
            switch (type)
            {
                case ListType.Structural:
                    return new StructuralList(content);

                default:
                    return new TeklaList(content);
            }
        }

        private static string GetFileContent(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return File.ReadAllText(path, Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage));
        }
    }
}