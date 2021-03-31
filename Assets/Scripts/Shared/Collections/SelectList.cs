using System;
using System.Collections.Generic;

namespace Shared.Collections
{
    public class SelectList<T>
    {
        private int _index;

        public SelectList(IList<T> items)
        {
            Items = items ?? throw new NullReferenceException();
        }

        public bool IsEmpty => Items.Count == 0;
        public bool IsFirstOrLastSelected => _index == 0 || _index == Count - 1;
        public IList<T> Items { get; }
        public int Count => Items.Count;

        public bool IsSelected(int index)
        {
            return _index == index;
        }

        public T GetSelected()
        {
            var item = IsEmpty ? default : Items[ClampIndex()];
            return item;
        }

        public void InsertAfterSelected(T item)
        {
            if (!IsEmpty) Items.Insert(ClampIndex() + 1, item);
        }

        public void RemoveSelected()
        {
            if (!IsEmpty) Items.RemoveAt(ClampIndex());
        }

        public void Select(int index)
        {
            _index = index;
        }

        public void SelectNext(bool right = true, bool loop = false)
        {
            if (right)
            {
                _index++;
                if (loop && _index >= Count) _index = 0;
            }
            else
            {
                _index--;
                if (loop && _index < 0) _index = Count;
            }
        }

        public void UpdateSelected(T item)
        {
            if (!IsEmpty) Items[ClampIndex()] = item;
        }

        private int ClampIndex()
        {
            if (_index >= Items.Count) _index = Items.Count - 1;
            if (_index < 0) _index = 0;
            return _index;
        }
    }
}