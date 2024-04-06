using System.Drawing;

namespace Test
{
    internal class Program
    {
        void Merge(LinkedList<int> main, LinkedList<int> sub)
        {
            var a = main.First;
            var b = main.Last;
            var c = sub.First;
            while(a != b)
            {
                if(c.Value > a.Value)
                {
                    var d = c.Next;
                    main.AddAfter(a, c);

                    c = d;
                    
                }
                a = a.Next;
                
            }
            
        }
        public struct Point
        {
            int[] z;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            LinkedList<string> list = new LinkedList<string>();
            Dictionary<Point, int> a = new Dictionary<int[], int>();
            a.Add(new int[2], 2);
            a.Add(new int[2], 2);
            foreach (var i in a)
                Console.WriteLine(i.Key);

        }
    }
}
