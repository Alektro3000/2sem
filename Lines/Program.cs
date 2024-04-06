using System.Runtime.InteropServices.Marshalling;

namespace Lines
{
    internal class Program
    {
        static int[,] LinArray(int a, int b)
        {
            int mxDiagonal = Math.Min(a, b);
            int mxDimension = Math.Max(a, b);
            int[,] Result = new int[a, b];

            Result[0, 0] = 1;

            for (int j = 1; j < mxDiagonal; j++)
                Result[0, j] = Result[0, j-1] + j;

            for (int j = a; j < b; j++)
                Result[0, j] = Result[0, j - 1] + a;

            for (int i = 1; i < a; i++)
                for (int j = 0; j < mxDiagonal - i; j++)
                    Result[i, j] += Result[i - 1, j] + i + j + 1;

            if (b >= a)
                for (int i = 1; i < a; i++)
                    for (int j = a - i; j < b - i; j++)
                        Result[i, j] += Result[i - 1, j] + mxDiagonal + 1;
            else
                for (int j = 0; j < b; j++)
                    for (int i = b - j; i < a - j; i++)
                        Result[i, j] += Result[i - 1, j] + mxDiagonal;
            
            for (int i = Math.Max(1, a-b); i < a; i++)
                for (int j = mxDimension - i; j < b; j++)
                    Result[i, j] += Result[i - 1, j] + a + b - j - i;
            
            return Result;
        }

        static void Main(string[] args)
        {
            var a = LinArray(8,4);
            for(int i = 0; i < a.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j = 0; j < a.GetLength(1); j++)
                    Console.Write(a[i,j].ToString().PadLeft(2)+" ");
            }
            a = LinArray(4,8);
            for (int i = 0; i < a.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j = 0; j < a.GetLength(1); j++)
                    Console.Write(a[i, j].ToString().PadLeft(2) + " ");
            }
        }
    }
}
