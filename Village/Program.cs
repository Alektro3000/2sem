﻿using System.Drawing;

namespace Village
{
    internal class Program
    {
        public struct HashSetTable
        {
            public string[] Data;
            public int Collisions;
            public int Count;
            public int Advance = 5;
            public HashSetTable(int Size = 256, int NewAdvance = 5)
            {
                Data = new string[Size];
                Collisions = 0;
                Count = 0;
                Advance = NewAdvance;
            }
            public HashSetTable(string[] Table, int Size = 256, int NewAdvance = 5) : this(Size,NewAdvance)
            {
                foreach (var a in Table)
                    Add(a);
            }
            public void Add(string Str)
            {
                int k = EnHash(Str);
                while (Data[k] != null)
                {
                    k = (k + Advance) % 256;
                    Collisions++;
                }
                Count++;
                Data[k] = Str;
            }

            public int EnHash(string Input)
            {
                return GetHash(Input.ToUpper());
            }
            public int GetHash(string strS)
            {
                int res = 0;
                foreach (char c in strS)
                    res += c;
                res = res % Data.Length;
                return res;
            }
        }


        static string[] arrTowns = { "Абросимовка", "Аксентьево", "Антониха", "Ануфриево", "Бабино", "Базарова Горка", "Барзаниха", "Басутино", "Башево", "Безуни", "Белавино", "Береговая-Коломенка", "Березицы", "Березник", "Бобино", "Бобовик", "Бобровик", "Болонье", "Большие Новоселицы", "Большие Семерицы", "Большое Обречье", "Большое-Фофанково", "Большой Вязник", "Большой Каменник", "Большой Чернец", "Боровичи", "Бортник", "Ботково", "Брызгово", "Буреги", "Быково", "Валугино", "Васильево", "Вашнево", "Вересимовка", "Вересовка", "Верховское", "Вилачево", "Вишма", "Владыкино", "Власиха", "Водоси", "Волгино", "Волок", "Волосово", "Выглядово", "Высоко", "Выставка", "Вятерево", "Гайново", "Глининец", "Гоголины", "Горка", "Горушка", "Горы", "Греблошь", "Громово", "Давыдово", "Девкино", "Денесино", "Дерева", "Деревково", "Дерягино", "Доманино", "Дубки", "Дубовики", "Дубровки", "Дымово", "Егла", "Елеково", "Еремеево", "Ерюхино", "Жаворонково", "Жадины", "Железково", "Желомля", "Жеребятниково", "Жихново", "Загорье", "Задорье", "Заклеп", "Залезенка", "Замостье", "Заполек", "Заречье", "Засородье", "Зихново", "Знаменка", "Золотово", "Иванково", "Ивашево", "Иевково", "Изонино", "Иловатик", "Исаиха", "Каменец", "Каменное", "Карманово", "Карпово", "Качалово", "Кировский", "Клементьево", "Клопчиха", "Княжна", "Князево", "Козлово", "Коммунарка", "Кончанско-Суворовское", "Коровкино", "Косарево", "Косунские Горы", "Котельниково", "Круппа", "Крюкшино", "Кузово", "Кураково", "Лазарево", "Лазница", "Лебедка", "Левково", "Лединка", "Лесное Задорье", "Липовец", "Лопатьево", "Лудилово", "ЛУКА", "Лыткино", "Любони", "Люля", "Лягуново", "Мазихина Горка", "Маклаково", "Малое Обречье", "Малое-Фофанково", "Малые Семерицы", "Малый Каменник", "Малый Чернец", "Марьинское", "Машкино", "Межуричье", "Миноха", "Михалево", "Михалино", "Мишино", "Мнево", "Молоденово", "Молчановка", "Мощеник", "Мышлячье", "Нальцы", "Наумовское", "Низино", "Никиришино", "Никитино", "Новоселицы", "Новые Короли", "Овинчуха", "Овсяниково", "Овсянниково", "Озерево", "Окладнево", "Опеченский Посад", "Опочно", "Орехово", "Осиновец", "Остров", "Павловка", "Павлушково", "Панево", "Папорть", "Передки", "Перелоги", "Перелучи", "Перхово", "Петровское", "Петухово", "Пирусс", "Плавково", "Плосково", "Подборье", "Поддубье", "Подол", "Полоное", "Починная Сопка", "Приозерье", "Прошково", "Прудищи", "Прудник", "Пукирево", "Путлино", "Пятница", "Райцы", "Раменье", "Речка", "Ровное", "Родишкино", "Рудно", "Румянцева Горка", "Рябиновка", "Садовка", "Саково", "Семеновское", "Сергейково", "Сестренки", "Сивцово", "Сидорково", "Скреплева Горушка", "Соинское", "Соколово", "Солнечная", "Солоно", "Сопины", "Сорокино", "Спасское", "Староселье", "Степаново", "Стрелка", "Стучево", "Сутоко-Рядок", "Сушеревка", "Сычево", "Талицы", "Тепецкое" };
        static void Main(string[] args)
        {
            var Table = new HashSetTable(arrTowns);
            Console.WriteLine("Коллизии:" + Table.Collisions);
            Console.WriteLine("Индекс 4:" + Table.Data[4]);
            Console.WriteLine("Всего:" + Table.Count);
        }
    }
}
