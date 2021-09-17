using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelProgrammingSamples
{
    public static class ArrayOrdering
    {
        public static int[] SerialQuickSort(int[] toSort) => 
            SerialQuickSort(toSort, 0, toSort.Length - 1);

        //Merge sort impl - serial
        private static int[] SerialQuickSort(int[] toSort, int leftStart, int rightEnd)
        {
            var left = leftStart;
            var right = rightEnd;
            var pivot = toSort[(leftStart + rightEnd) / 2];
            do
            {
                while (toSort[left] < pivot && left < rightEnd)
                    left++;
                while (pivot < toSort[right] && right > leftStart)
                    right--;
                if (left <= right)
                {
                    (toSort[left], toSort[right]) = (toSort[right], toSort[left]);
                    left++;
                    right--;
                }
            } while (left < right);

            if (right > leftStart) SerialQuickSort(toSort, leftStart, right);
            if (left < rightEnd) SerialQuickSort(toSort, left, rightEnd);

            return toSort;
        }

        public static async Task<int[]> ParallelQuickSort(int[] arr) => 
            await QuickSort(arr, 0, arr.Length - 1);

        private static async Task<int[]> QuickSort(int[] arr, int left, int right)
        {
            if (right <= left) return arr;
            int lt = left;
            int gt = right;
            var pivot = arr[left];
            int i = left + 1;
            while (i <= gt)
            {
                int cmp = arr[i].CompareTo(pivot);
                if (cmp < 0)
                    Swap(arr, lt++, i++);
                else if (cmp > 0)
                    Swap(arr, i, gt--);
                else
                    i++;
            }
		
            var t1 = Task.Run(() => QuickSort(arr, left, lt - 1));
            var t2 = Task.Run(() => QuickSort(arr, gt + 1, right));

            await Task.WhenAll(t1, t2).ConfigureAwait(false);
            
            return arr;
        }
        private static void Swap(int[] a, int i, int j) => (a[i], a[j]) = (a[j], a[i]);

        public static int[] ShakerSort(int[] array)
        {
            for (var i = 0; i < array.Length / 2; i++)
            {
                var swapped = false;
                for (var j = i; j < array.Length - i - 1; j++)
                    if (array[j] < array[j + 1])
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                        swapped = true;
                    }

                for (var j = array.Length - 2 - i; j > i; j--)
                    if (array[j] > array[j - 1])
                    {
                        (array[j], array[j - 1]) = (array[j - 1], array[j]);
                        swapped = true;
                    }

                if (!swapped) break;
            }
            
            return array.Reverse().ToArray();
        }


        public static int[] MergeSort(int[] array)
        {
            var result = MergeSort(array, new int[array.Length], 0, array.Length - 1);
            return result;
        }

        private static int[] MergeSort(int[] array, int[] aux, int left, int right)
        {
            if (left == right) return array;
            int middleIndex = (left + right) / 2;
            MergeSort(array, aux, left, middleIndex);
            MergeSort(array, aux, middleIndex + 1, right);
            Merge(array, aux, left, right);

            for (int i = left; i <= right; i++)
            {
                array[i] = aux[i];
            }

            return array.Reverse().ToArray();
        }
        
        private static void Merge(int[] array, int[] aux, int left, int right)
        {
            int middleIndex = (left + right) / 2;
            int leftIndex = left;
            int rightIndex = middleIndex + 1;
            int auxIndex = left;
            while (leftIndex <= middleIndex && rightIndex <= right)
            {
                if (array[leftIndex] >= array[rightIndex])
                {
                    aux[auxIndex] = array[leftIndex++];
                }
                else
                {
                    aux[auxIndex] = array[rightIndex++];
                }
                auxIndex++;
            }
            while (leftIndex <= middleIndex)
            {
                aux[auxIndex] = array[leftIndex++];
                auxIndex++;
            }
            while (rightIndex <= right)
            {
                aux[auxIndex] = array[rightIndex++];
                auxIndex++;
            }
        }

        private static readonly List<Task> _tasks = new();

        public static int[] MergeSortParallel(int[] array, int[] aux, int left, int right)
        {
            if (left == right) return array;
            int middleIndex = (left + right) / 2;
            var t = new Task(() => MergeSortParallel(array, aux, left, middleIndex));
            _tasks.Add(t);
            t.Start();
            t = new Task(() =>MergeSortParallel(array, aux, middleIndex + 1, right));
            _tasks.Add(t);
            t.Start();
            Task.WaitAll(_tasks.ToArray(), TimeSpan.FromMinutes(1));
            
            Merge(array, aux, left, right);

            for (int i = left; i <= right; i++)
            {
                array[i] = aux[i];
            }

            return array;
        }

        public static int[] SortSequential(int[] toSort) =>
            toSort.OrderBy(x => x).ToArray();
        
        public static int[] SortParallel(int[] toSort) =>
            toSort.AsParallel().OrderBy(x => x).ToArray();
    }
}