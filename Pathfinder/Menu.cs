namespace Pathfinder
{
    public class Menu<T> where T : IMenuItem
    {
        private T[] _items;
        private int _currentSelectionIndex;

        public T[] Items { get { return _items; } }

        /// <summary>
        /// Use to create a menu of items.
        /// </summary>
        /// <param name="items">Must be an array of objects using the IMenuItem interface.</param>
        public void Create(T[] items)
        {
            _items = items;
        }

        /// <summary>
        /// Reset the selected menu item to 0.
        /// </summary>
        public void Reset()
        {
            foreach (T item in _items)
                item.IsSelected = false;
            _currentSelectionIndex = 0;
            _items[_currentSelectionIndex].IsSelected = true;
        }

        /// <summary>
        /// Select a new menu item of an index either less than or more than the currently selected.
        /// The newly selected item will have its IsSelected property set to true and the previously selected will be set to false.
        /// </summary>
        /// <param name="input">Input +1 or -1 to step through the available menu items.</param>
        public void SelectNext(int input)
        {
            _items[_currentSelectionIndex].IsSelected = false;
            _currentSelectionIndex -= input;

            if (_currentSelectionIndex > _items.Length - 1)
                _currentSelectionIndex = 0;
            else if (_currentSelectionIndex < 0)
                _currentSelectionIndex = (_items.Length - 1);

            _items[_currentSelectionIndex].IsSelected = true;
        }

        /// <summary>
        /// Run the selected menu item's action.
        /// </summary>
        public void RunSelected()
        {
            _items[_currentSelectionIndex].Run();
        }
    }

    public interface IMenuItem
    {
        System.Action Run { get; set; }
        bool IsSelected { get; set; }
    }
}