namespace Sorting
{
    public interface IStepSort
    {
        /// <summary>
        /// Sorts an array with key selector
        /// </summary>
        /// <typeparam name="TSource">Array element type</typeparam>
        /// <typeparam name="TKey">Key element type</typeparam>
        /// <param name="array">Source array</param>
        /// <param name="selector">Method to extract key from element</param>
        /// <param name="reverse">Descending sort?</param>
        /// <param name="comparer">Method to compare keys</param>
        /// <param name="onStep">Method called each sorting step</param>
        void Sort<TSource, TKey>(TSource[] array, Func<TSource, TKey> selector, bool reverse = false, Comparison<TKey>? comparer = null, Action<int>? onStep = null);
    }
}