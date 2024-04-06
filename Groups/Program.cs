using System.Diagnostics;
using System.Linq;

namespace Groups
{
    internal class Program
    {
        public struct SHuman
        {
            public string Surname;          // фамилия
            public string Firstname;        // имя
            public string Patronymic;       // отчество
            public int Year;                // год рождения
            public SHuman(string surname, string firstname, string patronymic, int year)
            {
                Surname = surname;
                Firstname = firstname;
                Patronymic = patronymic;
                Year = year;
            }
        }
        public struct DisjointSetUnion
        {
            int[] LeadersIds;
            int[] LeadersSizes;
            //Создайт систему непересекающихся множест из n множеств, каждое множество содержит 1 элемент
            public DisjointSetUnion(int n)
            {
                LeadersIds =new int[n];
                LeadersSizes = new int[n];
                for (int i = 0; i < n; i++)
                {
                    LeadersIds[i] = i;
                    LeadersSizes[i] = 1;
                }
            }
            //Получает Id лидера, для двух элементов находящихся в одном множестве результат функции одинаков
            public int GetLeaderId(int BeginId)
            {
                if (BeginId == LeadersIds[BeginId])
                    return BeginId;
                return LeadersIds[BeginId] = GetLeaderId(LeadersIds[BeginId]);
            }
            //Объединяет два множества
            public void Union(int First, int Second)
            {
                int IdI = GetLeaderId(First);
                int IdJ = GetLeaderId(Second);
                if (LeadersSizes[IdI] > LeadersSizes[IdJ])
                {
                    int temp = IdI;
                    IdI = IdJ;
                    IdJ = temp;
                }
                LeadersIds[IdI] = IdJ;
                LeadersSizes[IdJ] += LeadersSizes[IdI];

                //Просто для более удобной отладки
                LeadersSizes[IdI] = 0;
            }
            //Получает размер множества
            //Требует лидера множества(иначе UB)
            public int GetSetSize(int Leader)
            {
                return LeadersSizes[Leader];
            }
        }
        
        static List<List<SHuman>> Group(SHuman[] Persons)
        {
            int n = Persons.Length;

            //Словари для быстрого поиска с какой группой человек схож
            Dictionary<string, int> FirstName = new Dictionary<string, int>();
            Dictionary<string, int> Surname = new Dictionary<string, int>();
            Dictionary<string, int> Patronymic = new Dictionary<string, int>();
            Dictionary<int, int> Year = new Dictionary<int, int>();

            //Система непересекающихся множеств для быстрого объединения множеств
            var Set = new DisjointSetUnion(n);

            for (int i = 0; i < n; i++)
            {
                //Если одно из полей человека есть в словарях, то объединяем с этим множеством
                int LeaderToConnect;
                if (FirstName.TryGetValue(Persons[i].Firstname, out LeaderToConnect))
                    Set.Union(i, LeaderToConnect);

                if (Surname.TryGetValue(Persons[i].Surname, out LeaderToConnect))
                    Set.Union(i, LeaderToConnect);

                if (Patronymic.TryGetValue(Persons[i].Patronymic, out LeaderToConnect))
                    Set.Union(i, LeaderToConnect);

                if (Year.TryGetValue(Persons[i].Year, out LeaderToConnect))
                    Set.Union(i, LeaderToConnect);

                //Обновляем словари
                int Lead = Set.GetLeaderId(i);
                Year[Persons[i].Year] = Lead;
                FirstName[Persons[i].Firstname] = Lead;
                Patronymic[Persons[i].Patronymic] = Lead;
                Surname[Persons[i].Surname] = Lead;
            }

            //Формируем ответ
            int k = 0;
            List<List<SHuman>> Ans = new List<List<SHuman>>();
            Dictionary<int, int> AnsIndexes = new Dictionary<int, int>();
            for(int i = 0; i < n; i++)
            {
                int LeaderId = Set.GetLeaderId(i);
                if (!AnsIndexes.ContainsKey(LeaderId))
                {
                    AnsIndexes.Add(LeaderId, k);
                    Ans.Add(new List<SHuman>(Set.GetSetSize(LeaderId)));
                    k++;
                }
                Ans[AnsIndexes[LeaderId]].Add(Persons[i]);
            }

            return Ans;
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
            var ans = Group(group);
            foreach(var an in ans)
            {
                Console.WriteLine();
                foreach(var p in an)
                    Console.WriteLine(p.Surname);

            }
        }
    }
}
