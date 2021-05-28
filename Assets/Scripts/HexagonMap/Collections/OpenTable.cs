using System;

namespace HexagonMap.Collections
{
    public class OpenTable
    {
        private MapItem[] _array;
        private int _count;

        public OpenTable()
        {
            _array = new MapItem[8];
            _count = 0;
        }

        public void Clear()
        {
            Array.Clear(_array, 0, _count);
            _count = 0;
        }

        private void _CheckArray()
        {
            if (_count < _array.Length) return;

            MapItem[] tmp = new MapItem[_array.Length << 1];
            Array.Copy(_array, 0, tmp, 0, _count);

            _array = tmp;
        }

        public MapItem Dequeue()
        {
            if (_count <= 0) return null;

            MapItem res = _array[0];
            res.OpenTableIndex = -1;

            MapItem last = _array[--_count];
            _array[0] = last;
            last.OpenTableIndex = 0;
            _array[_count] = null;

            int swap = 0;
            do
            {
                swap = _Balance(swap);
            } while (swap >= 0);

            return res;
        }

        private int _Balance(int id)
        {
            int l = (id << 1) + 1;
            if (l >= _count) return -1;

            int r = l + 1;
            var lItem = _array[l];
            if (r >= _count) return _BalanceItem(id, lItem, l);

            var rItem = _array[r];
            return lItem.TotalWeight > rItem.TotalWeight ? _BalanceItem(id, rItem, r) : _BalanceItem(id, lItem, l);
        }

        private int _BalanceItem(int parent, MapItem childItem, int child)
        {
            MapItem parentItem = _array[parent];
            if (parentItem.TotalWeight > childItem.TotalWeight)
            {
                _array[parent] = childItem;
                childItem.OpenTableIndex = parent;
                _array[child] = parentItem;
                parentItem.OpenTableIndex = child;
                return child;
            }

            return -1;
        }

        private int _BalanceChild(int id)
        {
            if (id <= 0) return -1;

            int parent = (id - 1) >> 1;
            MapItem parentItem = _array[parent];
            MapItem childItem = _array[id];
            if (parentItem.TotalWeight > childItem.TotalWeight)
            {
                _array[parent] = childItem;
                childItem.OpenTableIndex = parent;
                _array[id] = parentItem;
                parentItem.OpenTableIndex = id;
                return parent;
            }

            return -1;
        }

        public void Add(MapItem newItem)
        {
            _CheckArray();
            _array[_count] = newItem;
            newItem.OpenTableIndex = _count;

            int parent = _count;
            do
            {
                parent = _BalanceChild(parent);
            } while (parent >= 0);

            _count++;
        }

        public void Refresh(int oldIndex)
        {
            int parent = oldIndex;
            do
            {
                parent = _BalanceChild(parent);
            } while (parent >= 0);
        }
    }
}