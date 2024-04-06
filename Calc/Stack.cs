namespace Calc
{
    internal class Stack<T>
    {
        private int size;
        private T[] AllocatedMemory;
        public Stack()
        {
            AllocatedMemory = [];
            size = 0;
        }
        public Stack(int InitSize)
        {
            AllocatedMemory = new T[InitSize];
            size = 0;
        }
        public T Top
        {
            get => AllocatedMemory[size-1];
        }
        public T Pop()
        {
            size -= 1;
            return AllocatedMemory[size];
        }
        public void Clear()
        {
            size = 0;
        }
        public bool IsEmpty() => size == 0;
        public void Push(T NewElement)
        {
            if (size == AllocatedMemory.Length)
                Array.Resize(ref AllocatedMemory,10+size*2);
            AllocatedMemory[size] = NewElement;
            size += 1;
        }
        public int Size
        {
            get => size;
        }
    }
}
