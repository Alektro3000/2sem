using System.Collections.Generic;
using System.Media;

namespace Game2048
{
    public struct GameTransition
    {
        public int[] State;
        public int[] Moves;
        public SharpDX.Vector2 Offset;
        public int NewPinPosition;
        public GameTransition(int[] state, int[] move, SharpDX.Vector2 offset)
        {
            State = state;
            Moves = move;
            Offset = offset;
            NewPinPosition = -1;
        }
        public void SetUpPin(int PinPosition)
        {
            NewPinPosition = PinPosition;
        }
    }


    public partial class Form : System.Windows.Forms.Form
    {
        public int[] GameState;
        Stack<GameTransition> GameTransitions;
        int CellCount = 4;
        public Form()
        {
            InitializeForm();
            Game.MouseDown += (object? sender, MouseEventArgs e) => MousePoint = new Point(e.X, e.Y);
            Game.MouseUp += (object? sender, MouseEventArgs e) => MousePoint = new Point(-1, -1);
            Game.MouseMove += Game_MouseMove;
        }

        Point MousePoint = new Point(-1, -1);
        private void Game_MouseMove(object? sender, MouseEventArgs e)
        {
            if (MousePoint.X == -1)
                return;
            var Distance = e.Location - (Size)MousePoint;
            //���� �������� ���� ��������� ����� �� ������ ��� � ���������� ��������� ����
            if (Distance.X * Distance.X + Distance.Y * Distance.Y > 1000)
            {
                if (Math.Abs(Distance.X) > Math.Abs(Distance.Y))
                    Play(false, Distance.X > 0);
                else
                    Play(true, Distance.Y > 0);

                MousePoint = new Point(-1, -1);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateCellCount();
        }
        void Play(bool IsVert, bool IsPos)
        {
            var PrevState = GameState.ToArray();

            int n = CellCount;

            int[] moves = new int[n * n];
            for (int i = 0; i < n * n; i++)
                moves[i] = i;

            int[] Affect = new int[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (IsVert)
                        if (IsPos)
                            Affect[j] = i + (n - j - 1) * n;
                        else
                            Affect[j] = i + j * n;
                    else
                    {
                        if (IsPos)
                            Affect[j] = i * n + (n - j - 1);
                        else
                            Affect[j] = i * n + j;
                    }
                }
                MoveCells(GameState, moves, Affect);
            }
            GameTransition transition = new GameTransition(PrevState, moves,
                (IsPos ? 1 : -1) * new SharpDX.Vector2(IsVert ? 0 : 1, IsVert ? 1 : 0));

            Game.NextAnimation(PrevState, moves, GameState, transition.Offset);

            bool NoMovements = Enumerable.SequenceEqual(PrevState, GameState);

            int Val = AddCell();

            if (Val == -1)
            {
                if (NoMovements)
                    return;
                GameTransitions.Push(transition);
                return;
            }

            Game.NewPinAnimation(GameState[Val], Val, NoMovements);
            transition.SetUpPin(Val);
            GameTransitions.Push(transition);

            ScoreText.Text = "Score: " + GameState.Sum().ToString();
        }

        //����������� ����� �������� ������ Affected(�� ������� �������� � �������)
        //������ �������� � States �� �����
        //������ �������� Move �� �������� ���� ����� ���������� ������
        //���������� ������ ������� ��������� � Affected
        static void MoveCells(int[] States, int[] Move, int[] Affected)
        {
            List<int>[] MovedFrom = new List<int>[Affected.Length];
            MovedFrom[0] = [0];
            for (int i = 1; i < Affected.Length; i++)
            {
                MovedFrom[i] = [i];
                for (int j = i; j > 0; j--)
                {
                    int cur = Affected[j];
                    int prev = Affected[j - 1];
                    if (States[prev] != States[cur] && States[prev] != 0)
                        break;

                    foreach (var a in MovedFrom[j])
                        Move[Affected[a]] = prev;

                    MovedFrom[j - 1].AddRange(MovedFrom[j]);
                    MovedFrom[j].Clear();

                    if (States[prev] == States[cur])
                        States[prev] = States[cur] * 2;
                    else
                        States[prev] = States[cur];
                    States[cur] = 0;

                }
            }
        }


        int AddCell()
        {
            //������� ���������� ������� �����
            int count = GameState.Count(x => x == 0);

            //���� ������� ��� �� �������
            if (count == 0)
                return -1;

            //�������� ���������
            int num = Random.Shared.Next(count);
            //������� ��� ������ � �������
            for (int i = 0; i < GameState.Length; i++)
            {
                if (GameState[i] == 0)
                    num--;

                if (num == -1)
                {
                    num--;
                    //����������� ��������� ����� �������� �������
                    GameState[i] = Random.Shared.NextSingle() > 0.9f ? 4 : 2;
                    return i;
                }
            }
            return -1;
        }
        //��������� ����� ����
        private void FinishButton_Click(object sender, EventArgs e)
        {
            GameState = new int[CellCount*CellCount];
            Play(false, false);
        }
        //��������� ��������� ����
        private void BackButton_Click(object sender, EventArgs e)
        {
            if (GameTransitions.Count == 0) return;
            var transition = GameTransitions.Pop();
            Game.NextAnimation(transition.State, transition.Moves, GameState, transition.Offset);

            if (transition.NewPinPosition != -1)
                Game.NewPinAnimation(GameState[transition.NewPinPosition], transition.NewPinPosition, true);

            Game.InverseAnimation();
            GameState = transition.State;

            ScoreText.Text = "Score: " + GameState.Sum().ToString();

        }
        //��������� ���������� ���������� �����
        private void button1_Click(object sender, EventArgs e)
        {
            if (CellCount == 15)
                return;
            CellCount++;
            UpdateCellCount();
        }

        //��������� ���������� ���������� �����
        private void DecreaseCount_Click(object sender, EventArgs e)
        {
            if (CellCount == 1)
                return;
            CellCount--;
            UpdateCellCount();
        }

        private void UpdateCellCount()
        {
            Game.UpdateGrid(CellCount);
            GameState = new int[CellCount * CellCount];
            GameTransitions = [];
            Play(false, false);
        }
    }
}
