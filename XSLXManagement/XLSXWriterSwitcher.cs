using Model;
using Model.Extensions;
using Model.List;
using XLSXManagement.WriteDataStrategy;
using XLSXManagement.WriteHeaderStrategy;

namespace XLSXManagement
{
    public static class XLSXWriterSwitcher
    {
        public static void WriteList(ListType type, AbstractList list, string path)
        {
            switch (type)
            {
                case ListType.Material:
                    XLSXWriter.WriteList(list, path, new MaterialDeliveryStrategy(), new HeaderStrategy(), type);
                    break;

                case ListType.Delivery:
                    XLSXWriter.WriteList(list.ConvertToDeliveryList(), path, new MaterialDeliveryStrategy(), new HeaderStrategy(), type);
                    break;

                case ListType.Structural:
                    XLSXWriter.WriteList(list.ConvertToStructuralList(), path, new StructuralStrategy(), new HeaderStrategy(), type);
                    break;

                case ListType.BoltsDelivery:
                    XLSXWriter.WriteList(list.ConvertToBoltsDeliveryList(), path, new BoltsDeliveryStrategy(), new HeaderStrategy(), type);
                    break;
            }
        }
    }
}