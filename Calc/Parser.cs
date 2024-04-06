namespace Calc
{
    internal class Parser
    {
        static int[,] OperationTable = {
        {6,1,1,1,1,1,1,5,1},
        {5,1,1,1,1,1,1,3,1},
        {4,1,2,2,1,1,1,4,1},
        {4,1,2,2,1,1,1,4,1},
        {4,1,4,4,2,2,1,4,1},
        {4,1,4,4,2,2,1,4,1},
        {4,1,4,4,4,4,2,4,1},
        {5,5,5,5,5,5,5,5,5},
        {7,1,7,7,1,1,1,7,1},};
        static int GetTableValue(LexemType Previous, LexemType Current)
        {
            return OperationTable[(int)Previous - 1, (int)Current - 1];
        }
        static public double Evaluate(List<Lexem> Lexems)
        {
            Stack<Lexem> T = new(1);
            T.Push(new(LexemType.End));
            Stack<double> E = new(1);

            var it = Lexems.BeginNode;
            while (true)
            {
                if (it.Item.Type == LexemType.Num)
                {
                    E.Push(it.Item.Value);
                    it++;
                    continue;
                }

                bool fl = false;
                switch (GetTableValue(T.Top.Type, it.Item.Type))
                {
                    case 1:
                        T.Push(it.Item);
                        it++;
                        break;
                    case 2:
                        E.Push(Program.EvalInv(E.Pop(), E.Pop(), T.Pop().Type));
                        T.Push(it.Item);
                        it++;
                        break;
                    case 3:
                        T.Pop();
                        it++;
                        break;
                    case 4:
                        E.Push(Program.EvalInv(E.Pop(), E.Pop(), T.Pop().Type));
                        break;
                    case 5:
                        throw new Exception();
                    case 6:
                        fl = true;
                        break;
                    case 7:
                        E.Push(Program.Eval(E.Pop(),T.Pop().UnarType));
                        break;
                }
                if (fl)
                    break;
            }
            return E.Pop();
        }
    }
}
