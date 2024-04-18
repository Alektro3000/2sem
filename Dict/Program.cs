using System.Drawing;

namespace Dict
{
    internal class Program
    {
        public class HashTable
        {
            private List<string>[] Data;
            private int size = 256;
            public HashTable()
            {
                Data = new List<string>[size];
                for(int i =  0; i < size; i++)
                    Data[i] = new List<string>();
            }
            private const long hashIncr = 1000000009;
            private const long hashModulo = 1000000007;
            private int Count = 0;
            public int EvalHash(string Val)
            {
                long hash = 0;
                foreach(var a in Val)
                    hash = (hash*hashIncr+a)% hashModulo;

                return (int)hash % size;
            }
            public int GetHash(string Val)
            {
                return EvalHash(Path.GetFileName(Val));
            }
            public string GetValue(string Val)
            {
                var a = Data[GetHash(Val)];
                foreach(var b in a)
                    if (Path.GetFileName(b) == Val)
                        return b;
                
                throw new IndexOutOfRangeException();
            }
            public void AddValue(string Val)
            {
                Count++;
                Data[GetHash(Val)].Add(Val);
                if ((float)Count / size > 4f)
                    Rehash();
            }
            public void Rehash()
            {
                size *= 2;
                Count = 0;
                List<string> list = new List<string>();
                foreach (var lilList in Data)
                {
                    list.AddRange(lilList);
                    lilList.Clear();
                }
                Data = new List<string>[size];
                for (int i = 0; i < size; i++)
                    Data[i] = new List<string>();

                foreach (string key in list)
                    AddValue(key);
            }
        }
        static public bool Recursion(HashTable table, string directory)
        {
            try
            {
                bool fl = true;

                foreach (string dir in Directory.EnumerateDirectories(directory))
                    fl &= Recursion(table, dir);

                foreach (string file in Directory.EnumerateFiles(directory))
                    table.AddValue(file);
                return fl;
            }
            catch (FileNotFoundException expection)
            {
                return false;
            }
            catch (AccessViolationException expection)
            {
                return false;
            }
        }
        static void Main(string[] args)
        {
            HashTable table = new HashTable();
            Console.WriteLine("Введите путь папки в которой будут искаться файлы");
            string dir = Console.ReadLine();
            if (!Recursion(table, dir))
            {
                Console.WriteLine("Внимание некоторые файлы были защищены, их чтение невозможно");
            }
            while(true)
            {
                Console.WriteLine("1 - следующий файл, 2 - выход");
                string inp = Console.ReadLine();
                if (inp == "2")
                    break;
                Console.WriteLine("Введите файл");
                inp = Console.ReadLine();
                try
                {
                    Console.WriteLine(table.GetValue(inp));
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("Файл не найден");
                }
            }
        }
    }
}
