using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public struct LexemLevel
    {
        public Lexem Lexem;
        public int Level;
        public LexemLevel(Lexem lexem, int level)
        {
            Lexem = lexem;
            Level = level;
        }
    }
    internal class SimpleParser
    {
        static public double Evaluate(List<Lexem> Lexems)
        {
            //Remove Last Element
            Lexems.PreBeginNode.Advanced(Lexems.Length - 1).RemoveAfter();

            List<int> levels = BuildLevelsList(Lexems);
            while (!levels.IsSizeLessThan(2))
            {
                var MaxIt = levels.BeginNode;
                for (var i = levels.BeginNode; !i.IsPostLast(); i++)
                    if (MaxIt.Item <= i.Item)
                        MaxIt = i;

                EvaluateBracket(MaxIt, Lexems, levels);
            }
            return Lexems.BeginNode.Item.Value;
        }
        static private List<int> BuildLevelsList(List<Lexem> Lexems)
        {
            List<int> ans = new List<int>();
            var levelIt = ans.PreBeginNode;
            int curLevel = 0;
            for (var i = Lexems.BeginNode; !i.IsPostLast(); i++)
            {
                bool levelRising = i.Item.Type == LexemType.OpenBracket ||
                    i.Item.Type == LexemType.Num;
                curLevel += levelRising ? 1 : i.Item.Type == LexemType.Unar ? 0 : -1;
                levelIt = levelIt.AddAfter(curLevel);
            }
            return ans;
        }
        static private void EvaluateBracket(ListNode<int> MaxIt, List<Lexem> Lexems, List<int> Levels)
        {
            //Return to beginning of bracket
            int id = Levels.PreBeginNode.DistanceTo(MaxIt) - 2;

            //Указатели на элемент перед началом выражения
            ListNode<Lexem> LexemIt; 
            ListNode<int> LevelIt;
            //Результат выражения
            Lexem EvalResult;
            int Level = MaxIt.Item-1;

            //Обработка пустых скобок пример: "( 1 )"
            if ((Lexems.PreBeginNode + id + 1).Item.Type == LexemType.OpenBracket)
            {
                LexemIt = Lexems.PreBeginNode + id;
                LexemIt.RemoveAfter();//Удаляем (
                LexemIt++;//Передвигаемся на число
                LexemIt.RemoveAfter();//Удаляем )

                LevelIt = Levels.PreBeginNode + id;
                LevelIt.RemoveAfter();//Удаляем (
                LevelIt++;//Передвигаемся на число
                LevelIt.RemoveAfter();//Удаляем )
                LevelIt.Item = Level; //Уменьшаем уровень числа
                return;
            }

            //Обработка унарных операций пример: "( sin 0 )"
            if ((Lexems.PreBeginNode + id + 1).Item.Type == LexemType.Unar)
            {
                LexemIt = Lexems.PreBeginNode + (id-1);
                LevelIt = Levels.PreBeginNode + (id-1);

                EvalResult = new(Program.Eval(
                    (LexemIt + 3).Item.Value, 
                    (LexemIt + 2).Item.UnarType));

                LevelIt.RemoveNAfter(4);
                LexemIt.RemoveNAfter(4);

                LevelIt.AddAfter(Level);
                LexemIt.AddAfter(EvalResult);

                return;
            }
            //Обработка бинарных операторов "(2+2)"
            LexemIt = Lexems.PreBeginNode + (id-2);
            LevelIt = Levels.PreBeginNode + (id-2);

            EvalResult = new(Program.Eval(
                (LexemIt+2).Item.Value, 
                (LexemIt+4).Item.Value,
                (LexemIt+3).Item.Type));

            LevelIt.RemoveNAfter(5);
            LexemIt.RemoveNAfter(5);

            LevelIt.AddAfter(Level);
            LexemIt.AddAfter(EvalResult);
            return;
        }

    }
}
