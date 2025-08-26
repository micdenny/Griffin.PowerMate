using System;
using System.Collections;
using System.Collections.Generic;

namespace Griffin.PowerMate.EditorUI;

public class EventList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	private List<T> ItemList = new List<T>();

	public int Count => ItemList.Count;

	public bool IsReadOnly => false;

	public T this[int index]
	{
		get
		{
			return ItemList[index];
		}
		set
		{
			T item = ItemList[index];
			ItemList[index] = value;
			OnItemReplaced(new ListEventArgs<T>(item, index));
		}
	}

	public event ListEventHandler<T> ItemInserted;

	public event ListEventHandler<T> ItemRemoved;

	public event ListEventHandler<T> ItemReplaced;

	public event EventHandler Cleared;

	public void Add(T item)
	{
		ItemList.Add(item);
		OnItemInserted(new ListEventArgs<T>(item, ItemList.Count - 1));
	}

	public void Clear()
	{
		ItemList.Clear();
		OnCleared(EventArgs.Empty);
	}

	public bool Contains(T item)
	{
		return ItemList.Contains(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		ItemList.CopyTo(array, arrayIndex);
	}

	public bool Remove(T item)
	{
		int num = ItemList.IndexOf(item);
		RemoveAt(num);
		if (num >= 0)
		{
			return true;
		}
		return false;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return ItemList.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public int IndexOf(T item)
	{
		return ItemList.IndexOf(item);
	}

	public void Insert(int index, T item)
	{
		ItemList.Insert(index, item);
		OnItemInserted(new ListEventArgs<T>(item, index));
	}

	public void RemoveAt(int index)
	{
		T item = ItemList[index];
		ItemList.RemoveAt(index);
		OnItemRemoved(new ListEventArgs<T>(item, index));
	}

	protected virtual void OnItemInserted(ListEventArgs<T> e)
	{
		if (this.ItemInserted != null)
		{
			this.ItemInserted(this, e);
		}
	}

	protected virtual void OnItemRemoved(ListEventArgs<T> e)
	{
		if (this.ItemRemoved != null)
		{
			this.ItemRemoved(this, e);
		}
	}

	protected virtual void OnItemReplaced(ListEventArgs<T> e)
	{
		if (this.ItemReplaced != null)
		{
			this.ItemReplaced(this, e);
		}
	}

	protected virtual void OnCleared(EventArgs e)
	{
		if (this.Cleared != null)
		{
			this.Cleared(this, e);
		}
	}
}
