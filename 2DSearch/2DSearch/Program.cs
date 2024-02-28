using System.Formats.Asn1;

namespace _2DSearch
{
    internal class Program
    {
        static int[] FindSortedNorm(int[,] InpArray, int Val)
        {
            int n = InpArray.GetLength(1);
            int l = -1, r = InpArray.Length;
            while (r - l > 1)
            {
                int m = (l + r) >> 1;
                if (InpArray[m / n, m % n] < Val)
                    l = m;
                else
                    r = m;
            }
            int IndX = r / n;
            int IndY = r % n;
            if ((r != InpArray.Length) && (Val == InpArray[IndX, IndY]))
                return [IndX, IndY];
            else
                return [];// (r != InpArray.Length) && (Val == InpArray[IndX, IndY]);
        }
        static void Main(string[] args)
        {
            int[,] arr = { { 2, 6, 7, 9, 9, 14 }, { 18, 20, 26, 26, 40, 40 }, { 44, 47, 50, 51, 55, 62 } };
            // число для поиска2
            int num = 2;
            var a = FindSortedNorm(arr, num);
            if (a.Length == 2)
            {
                Console.WriteLine($"{a[0]} {a[1]}");
            }
            else
            {
                Console.WriteLine("Not Found");
            }
        }
    }
}
