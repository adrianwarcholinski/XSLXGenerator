using Model;

namespace XLSXManagement
{
    public static class XLSXWriter
    {
        public static void WriteList(ListType type, TeklaList list, string path)
        {
            switch (type)
            {
                case ListType.Material:
                case ListType.Delivery:
                {
                    MaterialDeliveryXLSXWriter.WriteMaterialList(list, path);
                    break;
                }

                case ListType.Structural:
                {
                    StructuralXLSXWriter.WriteStructuralList(list, path);
                    break;
                }
            }
        }
    }
}