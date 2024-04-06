using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Flats
{
    internal class Program
    {
        public struct Flat
        {
            public Flat(int cost, List<int> roomAreas, int kitchenArea, int otherArea)
            {
                Cost = cost;
                RoomAreas = roomAreas;
                KitchenArea = kitchenArea;
                OtherArea = otherArea;
            }
            public int Cost;
            public List<int> RoomAreas;
            public int KitchenArea;
            public int OtherArea;
            public int RoomCount
            {
                get => RoomAreas.Count;
            }
            public int TotalArea
            {
                get => RoomAreas.Sum() + KitchenArea + OtherArea;
            }
        }
        //Частично сортирует массив, выполняется за O(n)
        public static Flat FindKflat(List<Flat> flats, int k)
        {
            int begin = 0;
            int end = flats.Count - 1;
            while (end != begin)
            {
                int beginIt = begin;
                int endIt = end;
                int m = flats[(beginIt + endIt) / 2].Cost;
                while (beginIt <= endIt)
                {
                    while (flats[beginIt].Cost < m)
                        beginIt++;
                    while (flats[endIt].Cost > m)
                        endIt--;
                    if (beginIt >= endIt)
                        break;
                    var temp = flats[beginIt];
                    flats[beginIt] = flats[endIt];
                    flats[endIt] = temp;
                    beginIt++;
                    endIt--;
                }
                if (k > endIt)
                    begin = endIt + 1;
                else
                    end = endIt;
            }
            return flats[begin];
        }

        public static int GetMedianCost(List<Flat> flats, int RoomCount, int MinArea)
        {
            //Отбираем подходящие квартиры
            List<Flat> filtered = [];
            foreach(var flat in flats)
                if(flat.TotalArea > MinArea && flat.RoomCount == RoomCount)
                    filtered.Add(flat);
            var ans = Enumerable.OrderBy(filtered, x => x.Cost).ToList()[filtered.Count/2].Cost;

            return FindKflat(filtered, filtered.Count / 2).Cost;
        }
        static void Main(string[] args)
        {
            List<Flat> a = [new(100, [15,10,20],10,3),
                            new(150, [15,10,20],10,3),
                            new(200, [15,10,20],10,3),
                            new(050, [15,10,20],10,3),
                            new(000, [15,10,20],10,3),
                            new(151, [30,10,20],10,3),
                            new(152, [30,1,2,3],10,3),];
            Console.WriteLine(GetMedianCost(a,3,40));
        }
    }
}
