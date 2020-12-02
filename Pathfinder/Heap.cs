using System;

namespace Pathfinder
{
    public sealed class Heap<T> where T : IHeapItem<T>
    {
        private T[] _items;
        private int _size;

        private int Capacity { get { return _items.Length; } }
        public int Count { get { return _size; } }

        public Heap(int size)
        {
            _size = 0;
            _items = new T[size];
        }

        private int GetLeftChildIndex(int parentIndex) { return (parentIndex * 2) + 1; }
        private int GetRightChildIndex(int parentIndex) { return (parentIndex * 2) + 2; }
        private int GetParentIndex(int childIndex) { return (childIndex - 1) / 2; }

        private bool HasLeftChild(int index) { return GetLeftChildIndex(index) < _size; }
        private bool HasRightChild(int index) { return GetRightChildIndex(index) < _size; }
        private bool HasParent(int index) { return GetParentIndex(index) >= 0; }

        private T LeftChild(int index) { return _items[GetLeftChildIndex(index)]; }
        private T RightChild(int index) { return _items[GetRightChildIndex(index)]; }
        private T Parent(int index) { return _items[GetParentIndex(index)]; }

        private void Swap(T itemA, T itemB)
        {
            _items[itemA.HeapIndex] = itemB;
            _items[itemB.HeapIndex] = itemA;

            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }

        private void EnsureExtraCapacity()
        {
            if (_size >= Capacity)
            {
                T[] temp = new T[(Capacity * 2)];
                for (int i = 0; i < _size; i++)
                    temp[i] = _items[i];
                _items = temp;
            }
        }

        public void Add(T item)
        {
            EnsureExtraCapacity();
            item.HeapIndex = _size;
            _items[_size] = item;
            HeapifyUp(item);
            _size++;
        }

        public T Peek()
        {
            return _items[0];
        }

        public T RemoveFirst()
        {
            T item = _items[0];
            _size--;
            _items[0] = _items[_size];
            _items[0].HeapIndex = 0;
            HeapifyDown(_items[0]);
            return item;
        }

        public bool Contains(T item)
        {
            return Equals(_items[item.HeapIndex], item);
        }

        public void HeapifyDown(T item)
        {
            int index = item.HeapIndex;

            while (HasLeftChild(index))
            {
                int smallerChildIndex = GetLeftChildIndex(index);
                if (HasRightChild(index) && RightChild(index).CompareTo(LeftChild(index)) > 0)
                    smallerChildIndex = GetRightChildIndex(index);

                if (_items[index].CompareTo(_items[smallerChildIndex]) > 0)
                    break;

                Swap(_items[index], _items[smallerChildIndex]);
                index = smallerChildIndex;
            }
        }

        public void HeapifyUp(T item)
        {
            int index = item.HeapIndex;
            while (HasParent(index) && Parent(index).CompareTo(_items[index]) < 0)
            {
                Swap(Parent(index), _items[index]);
                index = GetParentIndex(index);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _size; i++)
                _items[i] = default;
            _size = 0;
        }
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}
