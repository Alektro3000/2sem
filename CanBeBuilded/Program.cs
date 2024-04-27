using System.Collections.Generic;

namespace CanBeBuilded
{
    internal class Program
    {
        static Dictionary<char, int> Analyze(string A)
        {
            Dictionary<char, int> Analyzed = [];
            for (int i = 0; i < A.Length; i++)
                if (Analyzed.ContainsKey(Char.ToLower(A[i])))
                    Analyzed[Char.ToLower(A[i])]++;
                else
                    Analyzed[Char.ToLower(A[i])] = 1;
            
            return Analyzed;
        }
        static bool IsBSubstrOfA(string A, string B)
        {
            Dictionary<char, int> AnalyzedA = Analyze(A);
            Dictionary<char, int> AnalyzedB = Analyze(B);
            foreach (var Let in AnalyzedB.Keys)
                if (!AnalyzedA.ContainsKey(Let) || AnalyzedA[Let] < AnalyzedB[Let])
                    return false;
            return true;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(IsBSubstrOfA(Console.ReadLine(), Console.ReadLine()));
        }
    }
}
