using System.Dynamic;

namespace Crossword
{
    internal class Program
    {
        const int Alf = 33;
        static int LetToInt(char letter)
        {
            var let = char.ToLower(letter);
            if (let == 'ё')
                return 32;
            return let - 'а';
        }
        static void SwapInArray<T>(T[] Arr, int idl, int idr)
        {
            T t = Arr[idl];
            Arr[idl] = Arr[idr];
            Arr[idr] = t;
        }

        static Queue<int> SolveRecCrossword((ulong, int)[] ulWords, int n, ulong[] uVerWord, ulong[] uVerWordPrefix, int m)
        {
            if (m == 1)
            {
                for (int i = 0; i < n; i++)
                    if ((uVerWord[m - 1] & ulWords[i].Item1) != 0)
                        return new Queue<int>([ulWords[i].Item2]);
                return [];
            }

            int end = n - 1;
            
            //Deleting words that doesn't have any letters equal with rest of verWord
            for (int i  = 0; i <= end; i++)
                while (((uVerWordPrefix[m - 1] & ulWords[i].Item1) == 0) && (i <= end))
                {
                    SwapInArray(ulWords, i, end);
                    end--;
                }
            
            if (m > end+1)
                return [];

            
            for (int i = 0; i <= end; i++)
                if ((uVerWord[m-1] & ulWords[i].Item1) != 0)
                {
                    SwapInArray(ulWords, i, end);

                    var ans = SolveRecCrossword(ulWords, end, uVerWord, uVerWordPrefix, m - 1);

                    SwapInArray(ulWords, i, end);

                    if (ans.Count != 0)
                    {
                        ans.Enqueue(ulWords[i].Item2); 
                        return ans;
                    }
                }
            return [];

        }

        static int[] SolveCrossword(string[] words, string verWord)
        {
            int m = verWord.Length;
            int n = words.GetLength(0);

            ulong[] uVerWord = new ulong[m];
            for (int i = 0; i < m; i++)
                uVerWord[i] = 1ul << LetToInt(verWord[i]);

            //Used for fast cheking is letter exist on range from begin to i
            ulong[] uVerWordPrefix = new ulong[m];
            uVerWordPrefix[0] = uVerWord[0];
            for (int i = 1; i < m; i++)
                uVerWordPrefix[i] = uVerWord[i] | uVerWordPrefix[i-1];

            (ulong, int)[] ulWords = new (ulong, int)[n];
            for (int i = 0; i < n; i++)
            {
                ulWords[i].Item2 = i;
                foreach (var let in words[i])
                    ulWords[i].Item1 |= 1ul << LetToInt(let);
            }
            var res = SolveRecCrossword(ulWords, n, uVerWord, uVerWordPrefix, m);
            
            int[] ans = new int[m];
            for(int i = 0; i < m; i++)
                ans[i] = res.Dequeue();

            return ans;
        }
        static void DrawCrossword(string[] words, string verWord)
        {
            int[] corWords = SolveCrossword(words, verWord);

            if (corWords.Length == 0)
            {
                Console.WriteLine("Can't build crossword");
                return;
            }

            int max = 0;
            int n = words.GetLength(0);

            int m = verWord.Length;
            int[] offsets = new int[m];
            for (int i = 0; i < m; i++)
            {
                var letter = verWord[i];
                offsets[i] = words[corWords[i]].IndexOf(letter);

                //try another register if failed
                if (offsets[i] < 0)
                    offsets[i] = words[corWords[i]].IndexOf(
                        char.IsUpper(letter) ? char.ToLower(letter) : char.ToUpper(letter));

                if (max < offsets[i])
                    max = offsets[i];
            }
            for (int i = 0; i < m; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(words[corWords[i]].Substring(0, offsets[i]).PadLeft(max));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(words[corWords[i]][offsets[i]]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(words[corWords[i]].Substring(offsets[i] + 1));
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        static void Main(string[] args)
        {
            string[] words = ["многобуковд", "амногобуковд", "К", "К", "К", "К", "К"];


            DrawCrossword(words, "ад");
        }
    }
}
