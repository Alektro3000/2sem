using System.ComponentModel.DataAnnotations;
using static Polygon.Program;

namespace Polygon
{
    internal class Program
    {
        public struct Node
        {
            public double X;
            public double Y;
            
            public Node(double x, double y)
            {
                X = x;
                Y = y;
            }

            public double GetDistanceSquared(Node anotherNode)
            {
                return (anotherNode.X - X) * (anotherNode.X - X) + (anotherNode.Y - Y) * (anotherNode.Y - Y);
            }
        }
        public struct Polygon
        {
            public Node[] Nodes;
            public bool Fill;
            public int Width; 
            public ConsoleColor Color; 
            public Polygon(Node[] nodes, int width, ConsoleColor color = ConsoleColor.White, bool fill = false)
            {
                Nodes = nodes;
                Fill = fill;
                Width = width;
                Color = color;
            }
            public bool IsEquilateralTriangle()
            {
                if (Nodes.Length != 3 )
                    return false;

                double length = double.Round(Nodes[0].GetDistanceSquared(Nodes[2]),4);

                for (int i = 0; i < Nodes.Length - 1; i++)
                {
                    
                    var dist = double.Round(Nodes[i].GetDistanceSquared(Nodes[i + 1]),4);
                    if (dist != length)
                        return false;
                }
                return true;
            }
            public bool IsWidthEqualTo(int width)
            {
                return Width == width;
            }

        }
        static int CountOfCorrectPolygons(Polygon[] polygons, int width)
        {
            int ans = 0;
            foreach (var polygon in polygons)
                if (polygon.IsEquilateralTriangle() && polygon.IsWidthEqualTo(width))
                    ans++;
            return ans;

        }
        static void Main(string[] args)
        {
            Polygon[] polygons = { 
                new Polygon([new Node(0,0), new Node(0, 1), new Node(Math.Sqrt(3)/2, 0.5)],1),
                new Polygon([new Node(0,0), new Node(0, 1), new Node(Math.Sqrt(3)/2, 0.5)],2),
                new Polygon([new Node(0,0.25), new Node(0, 1), new Node(Math.Sqrt(3)/2, 0.5)],1) };
            Console.Write(CountOfCorrectPolygons(polygons, 1));
        }
    }
}
