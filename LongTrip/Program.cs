namespace LongTrip
{
    internal class Program
    {
        static (int, int)[] offsets = [(0, 1), (0, -1), (1, 0), (-1, 0)];

        static List<(int,int)> Longest(int[,] Arr)
        {
            List<(int, (int, int))> SortedArray = [];
            for (int i = 0; i < Arr.GetLength(0); i++)
                for (int j = 0; j < Arr.GetLength(1); j++)
                    SortedArray.Add((Arr[i, j], (i, j)));
            SortedArray.Sort();

            int[,] LevelArr = new int[Arr.GetLength(0), Arr.GetLength(1)];
            (int,int)[,] ParentArr = new (int, int)[Arr.GetLength(0), Arr.GetLength(1)];

            foreach (var a in SortedArray)
                foreach (var off in offsets)
                {
                    (int, int) pos = (off.Item1 + a.Item2.Item1, off.Item2 + a.Item2.Item2);
                    if (pos.Item2 < Arr.GetLength(1) &&
                        pos.Item2 >= 0 &&
                        pos.Item1 < Arr.GetLength(0) &&
                        pos.Item1 >= 0)
                        if (Arr[pos.Item1, pos.Item2] > a.Item1)
                            if (LevelArr[pos.Item1,pos.Item2] <= LevelArr[a.Item2.Item1, a.Item2.Item2])
                            {
                                LevelArr[pos.Item1, pos.Item2] = 1+LevelArr[a.Item2.Item1, a.Item2.Item2];
                                ParentArr[pos.Item1, pos.Item2] = a.Item2;
                            }
                }
            (int, int) Max = (0,0);
            for (int i = 0; i < Arr.GetLength(0); i++)
                for (int j = 0; j < Arr.GetLength(1); j++)
                    if (LevelArr[Max.Item1, Max.Item2] < LevelArr[i, j])
                        Max = (i, j);
                    

            List<(int,int)> output = [];
            while (LevelArr[Max.Item1,Max.Item2] != 0)
            {
                output.Add(Max);
                Max = ParentArr[Max.Item1, Max.Item2];
            }
            output.Add(Max);
            output.Reverse();
            return output;
        }


        static void Main(string[] args)
        {
            int[,] a = { { 2, 5, 1, 0 },{ 3, 3, 1, 9 },{ 4, 4, 7, 8 } };
            var b =  Longest(a);
            foreach(var pa in b)
                Console.WriteLine(pa.Item1+" "+pa.Item2 + " " + a[pa.Item1,pa.Item2]);
        }
    }
}
