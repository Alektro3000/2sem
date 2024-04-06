namespace Rect
{
    internal class Program
    {
        struct Rect
        {
            //Нижняя граница по X(самая левая)
            private double LowX;
            //Нижняя граница по X(самая левая)
            public double LowerXBound
            {
                get { return LowX; }
            }
            //Верхняя граница по X(самая правая)
            private double HighX;
            //Верхняя граница по X(самая правая)
            public double UpperXBound
            {
                get { return HighX; }
            }
            //Нижняя граница по Y(самая нижняя)
            private double LowY;
            //Нижняя граница по Y(самая нижняя)
            public double LowerYBound
            {
                get { return LowY; }
            }

            //Верхняя граница по Y(самая верхняя)
            private double HighY;
            //Верхняя граница по Y(самая верхняя)
            public double UpperYBound
            {
                get { return HighY; }
            }

            public Rect(double lowX, double highX, double lowY, double highY)
            {
                if (lowX > highX)
                {
                    double temp = lowX;
                    lowX = highX;
                    highX = temp;
                }
                if (lowY > highY)
                {
                    double temp = lowY;
                    lowY = highY;
                    highY = temp;
                }
                LowX = lowX;
                HighX = highX;
                LowY = lowY;
                HighY = highY;
            }
            public static Rect GetBoundingBox(params Rect[] rects)
            {
                var ans = new Rect();
                foreach(Rect rect in rects)
                {
                    if (ans.LowX > rect.LowX)
                        ans.LowX = rect.LowX;
                    if (ans.LowY > rect.LowY)
                        ans.LowY = rect.LowY;
                    
                    if (ans.HighX < rect.HighX)
                        ans.HighX = rect.HighX;
                    if (ans.HighY < rect.HighY)
                        ans.HighY = rect.HighY;
                }
                return ans;
            }
        }
        static void Main(string[] args)
        {
            Rect[] rects = { 
                    new (0, 1, 0, 1), 
                    new (0, 1, 0, 2), 
                    new (0, 2, 0, 1), 
                    new (0, 1, -1, 1) };
            Rect BoundingBox = Rect.GetBoundingBox(rects);
            Console.WriteLine($"X: {BoundingBox.LowerXBound}, {BoundingBox.UpperXBound}; " +
                $"Y: {BoundingBox.LowerYBound}, {BoundingBox.UpperYBound}");
        }
    }
}
