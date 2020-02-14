using Model;

namespace XLSXManagement
{
    public static class XLSXWriterSwitcher
    {
        public static void WriteList(ListType type, AbstractList list, string path)
        {
            switch (type)
            {
                case ListType.Material:
                    XLSXWriter.WriteList(list, path, new MaterialDeliveryStrategy());
                    break;

                case ListType.Delivery:
                    XLSXWriter.WriteList(list.ConvertToDeliveryList(), path, new MaterialDeliveryStrategy());
                    break;

                case ListType.Structural:
                    XLSXWriter.WriteList(list.ConvertToStructuralList(), path, new StructuralStrategy());
                    break;

                case ListType.BoltsDelivery:
                    XLSXWriter.WriteList(list.ConvertToStructuralList(), path, new StructuralStrategy());
                    break;
            }
        }
    }
}