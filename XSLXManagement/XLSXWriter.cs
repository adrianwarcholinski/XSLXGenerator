using Model;

namespace XLSXManagement
{
    public static class XLSXWriter
    {
        public static void WriteList(ListType type, AbstractList list, string path)
        {
            switch (type)
            {
                case ListType.Material:
                {
                    MaterialDeliveryXLSXWriter.WriteMaterialList(list, path);
                    break;
                }

                case ListType.Delivery:
                {
                    MaterialDeliveryXLSXWriter.WriteMaterialList(list.ConvertToDeliveryList(), path);
                    break;
                }

                case ListType.Structural:
                {
                    StructuralXLSXWriter.WriteStructuralList(list.ConvertToStructuralList(), path);
                    break;
                }
            }
        }
    }
}