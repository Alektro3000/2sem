namespace HanoTower
{
    internal class HanoSolution
    {

        public struct ScreenShot
        {
            public int Begin;
            public int End;
            public override string ToString()
            {
                return Begin +"=>"+End;
            }
            public ScreenShot(int begin, int end)
            {
                Begin = begin;
                End = end;
            }
            public ScreenShot Inverted() => new(End, Begin);
        }

        private List<ScreenShot> Movements = [];
        private Dictionary<Stack<int>, int> StacksIds = [];

        private void MoveOne(Stack<int> begin, Stack<int> end)
        {
            end.Push(begin.Pop());
            Movements.Add(new ScreenShot(StacksIds[begin], StacksIds[end]));
        }


        //Create Solution
        private void MoveRecursively(Stack<int> begin, Stack<int> free, Stack<int> end, int n)
        {
            if (n == 1)
            {
                MoveOne(begin, end);
                return;
            }

            MoveRecursively(begin, end, free, n - 1);
            MoveOne(begin, end);
            MoveRecursively(free, begin, end, n - 1);
        }

        public List<ScreenShot> Solve(int n)
        {
            Stack<int> begin = [];
            for(int i = n; i > 0; i--)
                begin.Push(i);

            Stack<int> free = [], end = [];

            StacksIds.Add(begin, 0);
            StacksIds.Add(free, 1);
            StacksIds.Add(end, 2);
            MoveRecursively(begin, free, end, begin.Count);
            return Movements;
        }

    }
}
