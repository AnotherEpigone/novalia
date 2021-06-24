using SadRogue.Primitives.GridViews;
using System.Collections.Generic;

namespace Novalia.Extensions
{
    public static class IGridViewExtensions
    {
        public static T[] ToArray<T>(this IGridView<T> gridview)
        {
            var array = new T[gridview.Count];
            for (int i = 0; i < gridview.Count; i++)
            {
                array[i] = gridview[i];
            }

            return array;
        }

        public static IEnumerable<T> ToEnumerable<T>(this IGridView<T> gridview)
        {
            for (int i = 0; i < gridview.Count; i++)
            {
                yield return gridview[i];
            }
        }
    }
}
