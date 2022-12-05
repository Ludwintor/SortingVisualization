namespace Sorting
{
    public sealed class IntroSort : IStepSort
    {
        private const int SIZE_THRESHOLD = 16;

        /// <inheritdoc/>
        public void Sort<TSource, TKey>(TSource[] array, Func<TSource, TKey> selector, bool reverse = false,
                                        Comparison<TKey>? comparer = null, Action<int>? onStep = null)
        {
            if (comparer == null)
                comparer = Comparer<TKey>.Default.Compare;
            Comparison<TKey> comparison = reverse ? (x, y) => -1 * comparer(x, y) : comparer;
            int depthLimit = 2 * (int)Math.Floor(Math.Log10(array.Length));
            IntroSorting(array, 0, array.Length - 1, depthLimit, selector, comparison, onStep);
        }

        /// <summary>
        /// Sorts an array
        /// </summary>
        /// <typeparam name="TSource">Array element type</typeparam>
        /// <param name="array">Source array</param>
        /// <param name="reverse">Descending sort?</param>
        /// <param name="comparer">Method to compare array values</param>
        /// <param name="onStep">Method called each sorting step</param>
        public void Sort<TSource>(TSource[] array, bool reverse = false, Comparison<TSource>? comparer = null,
                                  Action<int>? onStep = null)
        {
            Sort(array, x => x, reverse, comparer, onStep);
        }

        private static void IntroSorting<TSource, TKey>(TSource[] array, int start, int end, int depthLimit, Func<TSource, TKey> selector,
                                                        Comparison<TKey> comparer, Action<int>? onStep)
        {
            if (end - start < SIZE_THRESHOLD)
            {
                InsertionSort(array, start, end, selector, comparer, onStep);
            }
            else if (depthLimit == 0)
            {
                HeapSort(array, start, end, selector, comparer, onStep);
            }
            else
            {
                depthLimit--;
                (int left, int right) = Partition(array, start, end, selector, comparer, onStep);
                IntroSorting(array, start, right, depthLimit, selector, comparer, onStep);
                IntroSorting(array, left, end, depthLimit, selector, comparer, onStep);
            }
        }

        private static void InsertionSort<TSource, TKey>(TSource[] array, int start, int end, Func<TSource, TKey> selector,
                                                         Comparison<TKey> comparer, Action<int>? onStep)
        {
            for (int i = start; i <= end; i++)
            {
                int current = i;
                onStep?.Invoke(current);
                while (current > start && comparer(selector(array[current]), selector(array[current - 1])) < 0)
                {
                    Swap(array, current, current - 1);
                    current--;
                    onStep?.Invoke(current);
                }
            }
        }

        private static void HeapSort<TSource, TKey>(TSource[] array, int start, int end, Func<TSource, TKey> selector,
                                            Comparison<TKey> comparer, Action<int>? onStep)
        {
            int heapCount = end - start + 1;
            for (int i = heapCount / 2 - 1; i >= 0; i--)
                Heapify(array, i, heapCount, start, selector, comparer, onStep);
            for (int i = heapCount - 1; i > 0; i--)
            {
                Swap(array, start, start + i);
                Heapify(array, 0, i, start, selector, comparer, onStep);
            }
        }

        private static void Heapify<TSource, TKey>(TSource[] array, int root, int heapCount, int start, Func<TSource, TKey> selector,
                                           Comparison<TKey> comparer, Action<int>? onStep)
        {
            int largest = root;
            int left = 2 * root + 1;
            int right = 2 * root + 2;

            if (left < heapCount && comparer(selector(array[start + left]), selector(array[start + largest])) > 0)
                largest = left;
            if (right < heapCount && comparer(selector(array[start + right]), selector(array[start + largest])) > 0)
                largest = right;
            onStep?.Invoke(start + largest);
            if (largest != root)
            {
                Swap(array, start + largest, start + root);
                Heapify(array, largest, heapCount, start, selector, comparer, onStep);
            }
        }

        private static (int, int) Partition<TSource, TKey>(TSource[] array, int start, int end, Func<TSource, TKey> selector,
                                                    Comparison<TKey> comparer, Action<int>? onStep)
        {
            int middle = start + (end - start) / 2;
            TKey pivot = selector(array[middle]);
            int left = start;
            int right = end;
            while (left <= right)
            {
                while (comparer(selector(array[left]), pivot) < 0)
                {
                    left++;
                    onStep?.Invoke(left);
                }
                while (comparer(selector(array[right]), pivot) > 0)
                {
                    right--;
                    onStep?.Invoke(right);
                }
                if (left <= right)
                {
                    Swap(array, left, right);
                    onStep?.Invoke(left);
                    left++;
                    right--;
                }
            }
            return (left, right);
        }

        private static void Swap<T>(T[] array, int lhs, int rhs)
        {
            (array[lhs], array[rhs]) = (array[rhs], array[lhs]);
        }
    }
}