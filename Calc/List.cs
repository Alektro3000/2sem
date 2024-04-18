using System.Collections;

namespace Calc
{
    public struct ListNode<T> : IEnumerator<ListNode<T>>
    {
        private int Index;
        private List<T> ParentList;

        internal ListNode(int id, List<T> List)
        {
            Index = id;
            ParentList = List;
        }

        ListNode<T> IEnumerator<ListNode<T>>.Current => this;
        object IEnumerator.Current => this;
        void IDisposable.Dispose()  { ParentList = null; }
        bool IEnumerator.MoveNext()
        {
            if (IsLast())
                return false;
            this++;
            return true;
        }
        public static implicit operator T(ListNode<T> Node) => Node.Item;
        void IEnumerator.Reset() => Index = -2;

        public bool IsLast() => Index == -2 ? ParentList.Begin == -1 : (ParentList.IsLastId(Index));
        public bool IsPostLast() => Index == -1;
        public bool IsPreBegin() => Index == -2;
        public static ListNode<T> operator ++(ListNode<T> Inp)
        {
            if (Inp.Index == -1)
                throw new IndexOutOfRangeException();

            if (Inp.Index == -2)
            {
                Inp.Index = Inp.ParentList.Begin;
                return Inp;
            }

            Inp.Index = Inp.ParentList.Links[Inp.Index];
            return Inp;
        }
        //Получение данных на которую указывает итератор
        public T Item
        {
            get
            {
                if (Index < 0)
                    throw new IndexOutOfRangeException();
                return ParentList.Elements[Index];
            }
            set
            {
                if (Index < 0)
                    throw new IndexOutOfRangeException();
                ParentList.Elements[Index] = value;
            }
        }
        public void SetItem(T newItem) => ParentList.Elements[Index] = newItem;
        

        //Добавляет элемент сразу после итератора
        public ListNode<T> AddAfter(T Element)
        {
            if(IsPostLast())
                throw new IndexOutOfRangeException();

            if (IsPreBegin())
                return ParentList.AddFront(Element);
            return ParentList.AddAfter(Index, Element);
        }
        //Удаляет элемент сразу после итератора
        public void RemoveAfter()
        {
            if (IsLast() || IsPostLast())
                throw new IndexOutOfRangeException();
            if (IsPreBegin())
                ParentList.RemoveFront();
            else
                ParentList.RemoveAfter(Index);
        }
        //Удаляет N элементов после итератора
        public void RemoveNAfter(int n)
        {
            for (; n > 0; n--)
                RemoveAfter();
        }
        //Возвращает итератор продвинутый на n шагов вперёд
        public ListNode<T> Advanced(int n)
        {
            if (n < 0)
                throw new Exception();
            var it = this;
            for (; n > 0; n--)
                it++;
            return it;
        }
        public static ListNode<T> operator +(ListNode<T> Inp, int Advance)
        {
            return Inp.Advanced(Advance);
        }
        //Находит расстояние до итератора end
        public int DistanceTo(ListNode<T> end)
        {
            int i = 0;
            for (var it = this; it.Index != end.Index; it++)
                i++;
            return i;
        }
    }
    public class List<T> : IEnumerable<ListNode<T>>
    {
        internal int[] Links;
        internal T[] Elements;
        internal int Begin;
        private int Free;

        public List() {
            Links = [];
            Elements = [];
            Begin = -1;
            Free = 0;
        }
        //Добавить в начало списка
        public ListNode<T> AddFront(T newElement)
        {
            int NewNode = AllocMemory();

            Links[NewNode] = Begin;
            Begin = NewNode;

            Elements[NewNode] = newElement;

            return MakeIterator(NewNode);
        }
        
        internal ListNode<T> AddAfter(int id, T newElement)
        {
            int NewNode = AllocMemory();

            Links[NewNode] = Links[id];
            Links[id] = NewNode;

            Elements[NewNode] = newElement;

            return MakeIterator(NewNode);
        }
        internal void RemoveAfter(int id)
        {
            if (IsLastId(id))
                throw new Exception();

            int FreeNode = Links[id];
            Links[id] = Links[FreeNode];
            MarkAsFree(FreeNode);
        }
        //Удалить начальный элемент
        public void RemoveFront()
        {
            int id = Begin;
            Begin = Links[Begin];
            MarkAsFree(id);
        }

        private void MarkAsFree(int id)
        {
            Links[id] = Free;
            Free = id;
        }
        private int AllocMemory()
        {
            if (Free == Links.Length)
                Resize(10+Free * 2);
            int NewNode = Free;
            Free = Links[Free];
            return NewNode;
        }
        private void Resize(int NewSize)
        {
            int PrevSize = Links.Length;
            Array.Resize(ref Links, NewSize);
            Array.Resize(ref Elements, NewSize);

            for(int i = PrevSize; i < NewSize; i++)
                Links[i] = i+1;
        }
        internal bool IsLastId(int id)
        {
            return Links[id] == -1;
        }
        internal ListNode<T> MakeIterator(int id) => new ListNode<T>(id,this);
        public int Size
        {
            get {
                int res = 1;
                int it = Begin;
                while (!IsLastId(it))
                {
                    it = Links[it];
                    res++;
                }
                return res;
            }
        }
        public int Length
        {
            get => Size;
        }
        public bool IsEmpty() => Links[Begin] == -1;
        public bool IsSizeLessThan(int size)
        {
            int res = 1;
            int it = Begin;
            while (!IsLastId(it))
            {
                it = Links[it];
                res++;
                if (size == res)
                    return false;
            }
            return true;
        }
        public ListNode<T> BeginNode
        {
            get => MakeIterator(Begin);
        }
        public ListNode<T> PreBeginNode
        {
            get => new ListNode<T>(-2, this);
        }
        public IEnumerator<ListNode<T>> GetEnumerator() => PreBeginNode;

        IEnumerator IEnumerable.GetEnumerator() => PreBeginNode;
        public override string ToString()
        {
            string res = "";
            foreach(var it in this)
                res += it.Item.ToString();
            return res;
        }
        
    }
}
