namespace Pathfinder
{
    public class Index
    {
        private int _current = 0;
        private int _min;
        private int _max;
        public int Current { get { return _current; } }
        public int Min { get { return _min; } set { _min = value; } }
        public int Max { get { return _max; } set { _max = value; } }

        public Index()
        {
            _min = 0;
            _max = 9;
        }

        public Index(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public void Increment()
        {
            _current = (_current < _max) ? _current + 1 : _min;
        }

        public void Decrement()
        {
            _current = (_current > _min) ? _current - 1 : _max;
        }

        public void Reset()
        {
            _current = 0;
        }

        public void Set(int index)
        {
            _current = (index > _max)? _max: (index < _min)? _min : 0;
        }
    }
}
