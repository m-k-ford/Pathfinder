namespace Pathfinder
{
    public class Enumerator<T>
    {
        protected T[] _items;
        protected int _current = -1;
        public virtual T[] Items { get { return _items; } set { _items = value; } }
        public bool Continue { get { return _current < _items.Length; } }

        public virtual T Next()
        {
            _current++;
            return _items[_current];
        }

        public virtual void Reset()
        {
            _current = -1;
        }
    }

    public class CartographerEnumerator<T> : Enumerator<T>
    {
        private int[] _default = new int[] { 1, 2, 4, 7, 6, 5, 3, 0 };
        private int[] _sequence;

        public int Current { get { return _current; } }
        public T Previous { get; set; }

        public override T[] Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = new T[value.Length];
                for (int i = 0; i < value.Length; i++)
                    _items[i] = value[_default[i]];
                _sequence = _default;
            }
        }

        public override T Next()
        {
            Previous = Peek();
            _current++;
            return _items[_sequence[_current]];
        }

        public T Peek()
        {
            return _items[_sequence[_current]];
        }

        public void RotateTo(Direction start)
        {
            int index = 0;
            for (int i = 0; i < _sequence.Length; i++)
            {
                if (i == (int)start)
                {
                    index = i;
                    break;
                }
            }
            for (int i = 0; i < (index + 1); i++)
            {
                int temp = _sequence[0];
                for (int j = 0; j < (_sequence.Length - 1); j++)
                    _sequence[j] = _sequence[j + 1];
                _sequence[_sequence.Length] = temp;
            }
        }
    }
}
