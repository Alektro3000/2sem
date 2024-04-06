using System.Linq;

namespace PancakeSort
{
    internal class Program
    {
        //Реализация для массива:
        static void Reverse(List<int> arr, int n)
        {
            for (int i = 0; i < n / 2; i++)
            {
                var temp = arr[i];
                arr[i] = arr[n - 1 - i];
                arr[n - 1 - i] = temp;
            }
        }
        static void Sort(List<int> arr)
        {
            for (int k = arr.Count; k > 1; k--)
            {
                int mx = arr[0];
                int mxid = 0;
                for (int i = 1; i < k; i++)
                    if (arr[i] > mx)
                    {
                        mx = arr[i];
                        mxid = i;
                    }
                if (mxid + 1 == k)
                    continue;
                if (mxid != 0)
                    Reverse(arr, mxid + 1);
                Reverse(arr, k);
            }
        }

        //Реализация для списка:
        static void Reverse(LinkedListNode<int> iterator, LinkedListNode<int> sentinel)
        {
            //Проверка на то что мы не прошли мимо итератора для нечётного и чётного расстояния
            while(sentinel != iterator && iterator.Previous != sentinel)
            {
                var temp = iterator.Value;
                iterator.Value = sentinel.Value;
                sentinel.Value = temp;

                iterator = iterator.Next;
                sentinel = sentinel.Previous;
            }
        }
        static void Sort(LinkedList<int> arr)
        {
            //Указываем границу до которой мы будем идти
            var Sentinel = arr.Last;
            while(Sentinel != arr.First)
            {
                //Итератор для того чтобы дойти до границы
                var iterator = arr.First;
                //Узел с максимальным значением
                var mx = arr.First;
                //Итерируемся, мы начинаем сразу с "индекса" 1 
                while (iterator != Sentinel)
                {
                    //обновляем итератор
                    iterator = iterator.Next;
                    //Проверка на максимум
                    if (iterator.Value > mx.Value)
                        mx = iterator;
                }

                //Если максимум уже на месте нет смысла его менять
                if (mx != Sentinel)
                {
                    //Если максимум уже в начале списка нет смысла его доставать
                    if (mx != arr.First)
                        Reverse(arr.First, mx);
                    //Кладём на место
                    Reverse(arr.First, Sentinel);
                }
                //Обновляем итератор
                Sentinel = Sentinel.Previous;
            }
        }

        static void Main(string[] args)
        {
            LinkedList<int> arr = new LinkedList<int>([8, 7, 6, 5, 4, 4, 3, 2, 1]);
            Sort(arr);
            foreach(var a in arr)
                Console.WriteLine(a);
        }
    }
}
