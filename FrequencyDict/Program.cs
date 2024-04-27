namespace FrequencyDict
{
    internal class Program
    {


        static void Main(string[] args)
        {
            string text = "Обратившись к действительности, Гнор пытался некоторое время" +
            " превратить свои неполные двадцать лет в двадцать один. Вопрос о " +
                "совершеннолетии стоял для него ребром: очень молодым людям, когда они" +
" думают жениться на очень молодой особе, принято чинить разные " +
"препятствия. Гнор обвел глазами прекрасную обстановку комнаты, в " +
"которой жил около месяца. Ее солидная роскошь по отношению к нему " +
"была чем - то вроде надписи, вывешенной над конторкой дельца: " +
"сутки имеют двадцать четыре часа. На языке Гнора это звучало так: " +
"у нее слишком много денег.";
            char[] Sep = [' ', ';', '.', ':', ',', '/', '!', '?', '-'];

            Dictionary<string, int> A = new Dictionary<string, int>();
            foreach (var item in text.Split(Sep, StringSplitOptions.RemoveEmptyEntries))
            {
                string h = item.ToLower();
                if (A.ContainsKey(h))
                    A[h]++;
                else
                    A[h] = 1;
            }

            while (true)
            {
                string inp = Console.ReadLine().ToLower();
                Console.WriteLine(A.ContainsKey(inp) ? A[inp] : "Слова нет");
            }
        }
    }
}
