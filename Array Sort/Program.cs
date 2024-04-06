namespace Array_Sort
{

    internal class Program
    {
        static int greatercounter = 0;
        static int lesscounter = 0;
        static bool IsGreater(int[] a, int[] b)
        {
            greatercounter++;
            if (a.Length > b.Length)
                return true;
            if (a.Length == b.Length && a.Sum() > b.Sum())
                return true;
            return false;
        }
        static bool IsLess(int[] a, int[] b)
        {
            lesscounter++;
            if (a.Length < b.Length)
                return true;
            if (a.Length == b.Length && a.Sum() < b.Sum())
                return true;
            return false;
        }

        static int Sort(int[][] array)
        {
            return Sort(array, 0, array.Length - 1);
        }
        static int Sort(int[][] array, int begin, int end)
        {
            if (end <= begin)
                return 1;
            int i = begin;
            int j = end;
            int[] center = array[(i + j) / 2];
            while (i <= j)
            {
                while (IsGreater(array[j], center))
                    j--;
                while (IsLess(array[i], center))
                    i++;

                if (i >= j)
                    break;
                var tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
                i++;
                j--;
            }
            return 1+ Sort(array, begin, j) + Sort(array, j + 1, end);

        }
        static void Main(string[] args)
        {
            int[][] arrTaxi = new int[10][];
            arrTaxi[0] = new int[] { 100, 289, 200, 101, 90, 230 };
            arrTaxi[1] = new int[] { 290, 300, 303, 120, 150 };
            arrTaxi[2] = new int[] { 80 };
            arrTaxi[3] = new int[] { 300, 60, 120, 400, 410 };
            arrTaxi[4] = new int[] { 60, 100, 40 };
            arrTaxi[5] = new int[] { 60, 160, 165, 120, 110, 230, 200, 30 };
            arrTaxi[6] = new int[] { 230, 200, 250, 100 };
            arrTaxi[7] = new int[] { 100, 209, 175, 100 };
            arrTaxi[8] = new int[] { 70, 120, 290 };
            arrTaxi[9] = new int[] { 90, 80, 105, 140, 120 };

            Console.WriteLine(Sort(arrTaxi));

            Console.WriteLine("great:"+greatercounter);
            Console.WriteLine("less:" + lesscounter);
        }
    }
}
