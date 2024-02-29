using System.Dynamic;
using System.Numerics;

namespace SortingLines
{
    public struct Point
    {
        public double x;
        public double y;
        public Point(double x1, double y1)
        {
            x = x1;
            y = y1;
        }
    }
    public struct Line
    {
        public Point Begin;
        public Point End;
        public double Width;
        public ConsoleColor Color;
        public Line(Point begin, Point end, double width, ConsoleColor color)
        {
            Begin = begin;
            End = end;
            Width = width;
            Color = color;
        }
        public double LengthSquared 
        {
            get
            {
                var y = Begin.y - End.y;
                var x = Begin.x - End.x;
                return x * x + y * y;
            }
        }
        public double Length
        {
            get
            {
                return Math.Sqrt(LengthSquared);
            }
        }
        public static bool operator >(Line p1, Line p2)
        {
            return p1.LengthSquared > p2.LengthSquared;
        }
        public static bool operator <(Line p1, Line p2)
        {
            return p1.LengthSquared < p2.LengthSquared;
        }
    }
    internal class Program
    {
        static public void QuickSort(Line[] Lines, int begin, int end)
        {
            if (end - begin == 0)
                return;
            var center = Lines[(end + begin) / 2];
            int endIndex = end;
            int beginIndex = begin;
            while (beginIndex <= endIndex)
            {
                while(Lines[endIndex] > center)
                    endIndex--;
                while (Lines[beginIndex] < center)
                    beginIndex++;
                if (beginIndex >= endIndex)
                    break;
                var a = Lines[beginIndex];
                Lines[beginIndex] = Lines[endIndex];
                Lines[endIndex] = a;
                beginIndex++;
                endIndex--;
            }
            QuickSort(Lines, begin, endIndex);
            QuickSort(Lines, endIndex+1, end);
        }
        static public void QuickSort(Line[] Lines)
        {
            QuickSort(Lines,0, Lines.Length-1);
        }
        static void Main(string[] args)
        {
            Line[] lines = {new Line(new Point(0,0), new Point(1, 1), 0.1, ConsoleColor.Magenta),
                            new Line(new Point(1,2), new Point(1, 2), 0.2, ConsoleColor.Black),
                            new Line(new Point(1,3), new Point(1, 2), 0.3, ConsoleColor.Green),
                                };
            QuickSort(lines);

            foreach(Line line in lines)
                Console.WriteLine($"Length: {Math.Round(line.Length,2)}; Width: {line.Width}; Color: {line.Color}");
        }
    }
}
