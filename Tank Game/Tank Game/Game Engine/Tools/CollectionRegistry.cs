namespace Tank_Game
{
	internal class CollectionRegistry<T>
	{
		readonly List<T> _activeList = new();
		readonly List<T> _toAdd = new();
		readonly List<T> _toRemove = new();

		public int Count => _activeList.Count + _toAdd.Count;

		public IReadOnlyList<T> Items => _activeList;
		public IEnumerable<T> GetItems() => _activeList;
		public T this[int index] => _activeList[index];

		public void Register(T item)
		{
			if (item == null) return;
			if (!_activeList.Contains(item) && !_toAdd.Contains(item))
				_toAdd.Add(item);
		}

		public void Unregister(T item)
		{
			if (item == null) return;
			if (_activeList.Contains(item) && !_toRemove.Contains(item))
				_toRemove.Add(item);

			_toAdd.Remove(item);
		}

		public void ProcessPendingChanges()
		{
			if (_toRemove.Count != 0)
			{
				foreach (var item in _toRemove)
					_activeList.Remove(item);
				_toRemove.Clear();
			}

			if (_toAdd.Count != 0)
			{
				_activeList.AddRange(_toAdd);
				_toAdd.Clear();
			}
		}


		public void Clear()
		{
			_activeList.Clear();
			_toAdd.Clear();
			_toRemove.Clear();
		}
	}
}
