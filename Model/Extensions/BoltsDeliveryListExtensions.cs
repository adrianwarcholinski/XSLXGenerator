using Model.List;

namespace Model.Extensions
{
    public static class BoltsDeliveryListExtensions
    {
        private static BoltsDeliveryList _list;

        public static BoltsDeliveryList ConvertToBoltsDeliveryList(this AbstractList list)
        {
            _list = (BoltsDeliveryList) list;

            return _list;
        }
    }
}