using System.Collections.Generic;

namespace Model.Extensions
{
    public static class ListExtensions
    {
        public static void UpdateElementAt<T>(this List<T> list, int index, T newElement)
        {
            list.Insert(index, newElement);
            list.RemoveAt(index + 1);
        }

        public static void InsertAndRemoveLastElement<T>(this List<T> list, int index, T element)
        {
            list.Insert(index, element);
            list.RemoveAt(list.Count - 1);
        }
    }
}