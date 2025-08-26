using System;
using System.Collections.Generic;

namespace Griffin.PowerMate.EditorUI;

internal class ColumnItemCollection : List<IColumnItem>
{
	public delegate void ColumnItemCollectionHandler(ColumnItemCollection sender, IColumnItem item, int index);

	private ColumnItemHandler TextChangeHandler;

	private ColumnItemHandler IconChangeHandler;

	private ColumnItemHandler SelectedChangeHandler;

	public new IColumnItem this[int index]
	{
		get
		{
			return base[index];
		}
		set
		{
			RemoveAt(index);
			Insert(index, value);
			if (this.ItemChanged != null)
			{
				this.ItemChanged(this, value, index);
			}
		}
	}

	public ColumnItemCollection SelectedItems => new ColumnItemCollection(FindAll(IsSelected));

	public event ColumnItemCollectionHandler ItemTextChanged;

	public event ColumnItemCollectionHandler ItemIconChanged;

	public event ColumnItemCollectionHandler ItemSelectedChanged;

	public event ColumnItemCollectionHandler ItemAdded;

	public event ColumnItemCollectionHandler ItemRemoved;

	public event ColumnItemCollectionHandler ItemChanged;

	public event EventHandler Sorted;

	public ColumnItemCollection()
	{
		TextChangeHandler = OnItemTextChanged;
		IconChangeHandler = OnItemIconChanged;
		SelectedChangeHandler = OnItemSelectedChanged;
	}

	public ColumnItemCollection(IEnumerable<IColumnItem> collection)
		: base(collection)
	{
		TextChangeHandler = OnItemTextChanged;
		IconChangeHandler = OnItemIconChanged;
		SelectedChangeHandler = OnItemSelectedChanged;
		foreach (IColumnItem item in collection)
		{
			item.TextChanged += TextChangeHandler;
			item.IconChanged += IconChangeHandler;
			item.SelectedChanged += SelectedChangeHandler;
		}
	}

	public new void Add(IColumnItem item)
	{
		base.Add(item);
		item.TextChanged += TextChangeHandler;
		item.IconChanged += IconChangeHandler;
		item.SelectedChanged += SelectedChangeHandler;
		if (this.ItemAdded != null)
		{
			this.ItemAdded(this, item, base.Count - 1);
		}
	}

	public new void AddRange(IEnumerable<IColumnItem> collection)
	{
		foreach (IColumnItem item in collection)
		{
			Add(item);
		}
	}

	public new void Clear()
	{
		while (base.Count > 0)
		{
			RemoveAt(0);
		}
	}

	public new void Insert(int index, IColumnItem item)
	{
		base.Insert(index, item);
		item.TextChanged += TextChangeHandler;
		item.IconChanged += IconChangeHandler;
		item.SelectedChanged += SelectedChangeHandler;
		if (this.ItemAdded != null)
		{
			this.ItemAdded(this, item, index);
		}
	}

	public new void InsertRange(int index, IEnumerable<IColumnItem> collection)
	{
		foreach (IColumnItem item in collection)
		{
			Insert(index, item);
			index++;
		}
	}

	public new bool Remove(IColumnItem item)
	{
		int num = IndexOf(item);
		if (num >= 0)
		{
			RemoveAt(num);
			return true;
		}
		return false;
	}

	public new int RemoveAll(Predicate<IColumnItem> match)
	{
		int num = 0;
		using Enumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			IColumnItem current = enumerator.Current;
			if (match(current) && Remove(current))
			{
				num++;
			}
		}
		return num;
	}

	public new void RemoveAt(int index)
	{
		IColumnItem columnItem = this[index];
		columnItem.TextChanged -= TextChangeHandler;
		columnItem.IconChanged -= IconChangeHandler;
		columnItem.SelectedChanged -= SelectedChangeHandler;
		base.RemoveAt(index);
		if (this.ItemRemoved != null)
		{
			this.ItemRemoved(this, columnItem, index);
		}
	}

	public new void RemoveRange(int index, int count)
	{
		for (int i = 0; i < count; i++)
		{
			RemoveAt(index);
		}
	}

	public new void Reverse(int index, int count)
	{
		base.Reverse(index, count);
		if (this.Sorted != null)
		{
			this.Sorted(this, new EventArgs());
		}
	}

	public new void Reverse()
	{
		base.Reverse();
		if (this.Sorted != null)
		{
			this.Sorted(this, new EventArgs());
		}
	}

	public new void Sort(Comparison<IColumnItem> comparison)
	{
		base.Sort(comparison);
		if (this.Sorted != null)
		{
			this.Sorted(this, new EventArgs());
		}
	}

	public new void Sort(int index, int count, IComparer<IColumnItem> comparer)
	{
		base.Sort(index, count, comparer);
		if (this.Sorted != null)
		{
			this.Sorted(this, new EventArgs());
		}
	}

	public new void Sort(IComparer<IColumnItem> comparer)
	{
		base.Sort(comparer);
		if (this.Sorted != null)
		{
			this.Sorted(this, new EventArgs());
		}
	}

	public new void Sort()
	{
		base.Sort();
		if (this.Sorted != null)
		{
			this.Sorted(this, new EventArgs());
		}
	}

	private bool IsSelected(IColumnItem item)
	{
		if (item.Selected)
		{
			return true;
		}
		return false;
	}

	private void OnItemTextChanged(IColumnItem sender)
	{
		if (this.ItemTextChanged != null)
		{
			int num = IndexOf(sender);
			if (num >= 0)
			{
				this.ItemTextChanged(this, sender, num);
			}
		}
	}

	private void OnItemIconChanged(IColumnItem sender)
	{
		if (this.ItemIconChanged != null)
		{
			int num = IndexOf(sender);
			if (num >= 0)
			{
				this.ItemIconChanged(this, sender, num);
			}
		}
	}

	private void OnItemSelectedChanged(IColumnItem sender)
	{
		if (this.ItemSelectedChanged != null)
		{
			int num = IndexOf(sender);
			if (num >= 0)
			{
				this.ItemSelectedChanged(this, sender, num);
			}
		}
	}
}
