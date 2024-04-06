using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupOfPasser_by
{
    class Program
    {
        public struct SHuman
        {
            public string Surname;          // фамилия
            public string Firstname;        // имя
            public string Patronymic;       // отчество
            public int Year;                // год рождения
            public SHuman(string surname, string firstname, string patronymic, int year)
            {
                this.Surname = surname;
                this.Firstname = firstname;
                this.Patronymic = patronymic;
                this.Year = year;
            }

        }
        static bool Equal(SHuman shuman1, SHuman shuman2)
        {
            return shuman1.Firstname == shuman2.Firstname || shuman1.Year == shuman2.Year
                || shuman1.Patronymic == shuman2.Patronymic || shuman1.Surname == shuman2.Surname;
        }

        static void Main(string[] args)
        {
            SHuman[] group = {new SHuman("Пушкин", "Александр", "Сергеевич", 1799),
                            new SHuman("Ломоносов", "Михаил", "Васильевич", 1711),
                            new SHuman("Тютчев", "Фёдор", "Иванович", 1803),
                            new SHuman("Суворов", "Александр", "Васильевич", 1729),
                            new SHuman("Менделеев", "Дмитрий", "Иванович", 1834),
                            new SHuman("Ахматова", "Анна", "Андреевна", 1889),
                            new SHuman("Володин", "Александр", "Моисеевич", 1919),
                            new SHuman("Мухина", "Вера", "Игнатьевна", 1889),
                            new SHuman("Верещагин", "Петр", "Петрович", 1834)};
            List<List<SHuman>> ans = StructGrouping(group);

            foreach (var an in ans)
            {
                Console.WriteLine();
                foreach (var p in an)
                    Console.WriteLine(p.Surname);

            }
        }

        private static List<List<SHuman>> StructGrouping(SHuman[] arr)
        {
            List<List<SHuman>> res = new List<List<SHuman>>();

            int[] colors = new int[arr.Length];

            int id = 1;


            for (int i = 0; i < arr.Length; i++)
                if (colors[i] == 0)
                    AddingNewGroup(arr, colors, i, id++);

            for (int i = 0; i < id; i++)
                res.Add([]);

            for (int i = 0; i < arr.Length; i++)
                res[colors[i] - 1].Add(arr[i]);

            return res;
        }

        private static void AddingNewGroup(SHuman[] arr, int[] colors, int index, int Color)
        {
            colors[index] = Color;

            for (int i = 0; i < arr.Count(); i++)
                if (Equal(arr[i], arr[index]))
                    if (colors[i] == 0)
                        AddingNewGroup(arr, colors, i, Color);

        }
    }
}
