using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Model
{
    public static class DeliveryListExtensions
    {
        private static AbstractList _list;
        private static StringColumn _weightColumn;
        private static StringColumn _quantityColumn;
        private static StringColumn _totalWeightColumn;

        public static AbstractList ConvertToDeliveryList(this AbstractList list)
        {
            _list = list;

            CalculateTotalWeight();

            PutAreaOnEnd();
            return _list;
        }

        private static void CalculateTotalWeight()
        {
            List<StringColumn> columns = _list.Columns;
            _weightColumn = FindColumn("Weight", out int index);
            _quantityColumn = FindColumn("No.", out _);
            _totalWeightColumn = new StringColumn("Waga całkowita (kg)");

            columns.Insert(index + 1, _totalWeightColumn);

            FillTotalWeight();
        }

        private static void FillTotalWeight()
        {
            DataChunk weightData = _weightColumn.Data.First();
            DataChunk quantityData = _quantityColumn.Data.First();
            DataChunk totalWeightData = _totalWeightColumn.Data.First();

            for (int i = 0; i < weightData.Entries.Count; i++)
            {
                double weight = double.Parse(weightData.Entries.ElementAt(i), NumberStyles.Any, CultureInfo.InvariantCulture);
                int quantity = int.Parse(quantityData.Entries.ElementAt(i));

                double totalWeight = weight * quantity;
                totalWeightData.AddEntry(totalWeight.ToString().Replace(",", "."));
            }

            CalculateTotalWeightSummary();
        }

        private static void CalculateTotalWeightSummary()
        {
            DataChunk data = _totalWeightColumn.Data.First();
            data.Summary = data.Entries.Sum(e => double.Parse(e, NumberStyles.Any, CultureInfo.InvariantCulture)).ToString().Replace(",", ".");
            _weightColumn.Data.First().Summary = null;
        }

        private static StringColumn FindColumn(string columnName, out int index)
        {
            StringColumn foundColumn = _list.Columns.First(column => column.Name.Contains(TranslateUtils.Translate(columnName)));
            index = _list.Columns.ToList().IndexOf(foundColumn);
            return foundColumn;
        }

        private static void PutAreaOnEnd()
        {
            StringColumn areaColumn = FindColumn("Area", out int index);
            _list.Columns.Remove(areaColumn);
            _list.Columns.Add(areaColumn);
        }
    }
}