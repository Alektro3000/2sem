using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ColorHomes
{
    internal class Program
    {
        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        static int GetColor(int col, int width, int height, int colors)
        {
            if (height > width)
                Swap(ref height, ref width);

            int sum = 0;
            
            //11223
            //12233
            //22333

            for(int i = col; i< width; i+= colors)
                sum += Math.Min(height,i+1);

            int n1 = (height - col - 2 + colors) / colors;
            sum += ((col + 1) * 2 + colors * (n1 - 1)) / 2 * n1;
            
            int begin2 = n1 * colors + col;
            int n2 = (width - 1 - begin2 - 1 + colors) / colors;
            sum += n2 * height;

            int begin3 = (col - width % colors + colors + 1) % colors;
            int n3 = (height - begin3 - 1 + colors) / colors;
            sum += height * n3 - (begin3 * 2 + colors * (n3 - 1)) / 2 * n3;

            return sum;
        }
        static int[] GetColors(int width, int height, int colors)
        {
            int[] res = new int[colors]; 
            for(int i = 0; i < colors; i++)
                res[i] = GetColor(i, width, height, colors);
            return res;
        }
        static void Main(string[] args)
        {
            int[] arr = [7, 7, 4];
            int height = arr[0];// int.Parse(Console.ReadLine());
            int width = arr[1];//int.Parse(Console.ReadLine());
            int c = arr[2];//int.Parse(Console.ReadLine());

            for (int j = 0; j < c; j++)
            {
                Console.WriteLine(GetColor(j, height, width, c));
            }
        }
    }
}
//2 2 3 : 1 2 1
//7 7 4 : 12 12 13 12
//6 7 4 : 10 11 11 10
//5 7 4 : 9 9 9 8
//
