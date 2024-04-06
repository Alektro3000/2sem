using System;
using System.ComponentModel.DataAnnotations;

namespace KindaSort
{
    internal class Program
    {
        static void QuickKindaSort(decimal[] arr, int begin, int end, int k)
        {
            if (end == begin)
                return;
            int i = begin;
            int j = end;
            decimal m = arr[(i + j) / 2];
            while (i <= j)
            {
                while (arr[i] < m)
                    i++;
                while (arr[j] > m)
                    j--;
                if (i >= j)
                    break;
                var temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
                i++;
                j--;
            }
            if (k > j)
                QuickKindaSort(arr, j + 1, end, k);
            QuickKindaSort(arr, begin, j, k);
        }
        static void Main(string[] args)
        {
            int k = int.Parse(Console.ReadLine());
            int n = int.Parse(Console.ReadLine());
            decimal[] arr= new decimal[n];
            for(int i = 0; i <  n; i++)
                arr[i] = -(decimal.Parse(Console.ReadLine()));
            QuickKindaSort(arr, 0, n - 1, k);
            for(int i = 0; i<k; i++)
                Console.WriteLine(-arr[i]);
        }
    }
}
