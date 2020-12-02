using System;

namespace Pathfinder
{
    public class ArrayList<T>
    {
        private const int DEFAULT_CAPACITY = 16;

        private T[] _items;
        private int _capacity;
        private int _size;
        public int Length { get { return _size; } }
        public bool AllowDuplicates { get; set; }
        public T FirstEntry { get { return _items[0]; } }
        public T LastEntry { get { return _items[_size - 1]; } }

        public ArrayList()
        {
            AllowDuplicates = false;
            _size = 0;
            _capacity = DEFAULT_CAPACITY;
            _items = new T[DEFAULT_CAPACITY];
        }

        public ArrayList(int capacity)
        {
            AllowDuplicates = false;
            _size = 0;
            _capacity = capacity;
            _items = new T[_capacity];
        }
        public T this[int i]
        {
            get
            {
                if (i > _size)
                    throw new System.IndexOutOfRangeException();
                return (_items[i] != null)? _items[i] : default;
            }
            set
            {
                _items[i] = value;
            }
        }

        private void EnsureExtraCapacity()
        {
            if (_size >= _capacity)
            {
                _capacity *= 2;
                T[] temp = new T[_capacity];
                for (int i = 0; i < _size; i++)
                    temp[i] = _items[i];
                _items = temp;
            }
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_items[i].Equals(item))
                    return true;
            }
            return false;
        }

        private bool Contains(T item, out int index)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_items[i].Equals(item))
                {
                    index = i;
                    return true;
                }
            }
            index = 0;
            return false;
        }

        public void Add(T item)
        {
            if (!AllowDuplicates)
            {
                if (Contains(item))
                    return;
            }
            EnsureExtraCapacity();
            _items[_size] = item;
            _size++;
        }

        public void Remove(T item)
        {
            int index = 0;
            if (Contains(item, out index))
            {
                while (index < _size - 1)
                {
                    _items[index] = _items[index + 1];
                    index++;
                }
                _size--;
            }
        }

        public void Remove(int index)
        {
            while (index < _size - 1)
            {
                _items[index] = _items[index + 1];
                index++;
            }
            _size--;
        }

        public T RemoveFirst()
        {
            T temp = _items[0];
            Remove(0);
            return temp;
        }

        public void Clear()
        {
            for (int i = 0; i < _size; i++)
                _items[i] = default;
            _size = 0;
        }
    }
}
