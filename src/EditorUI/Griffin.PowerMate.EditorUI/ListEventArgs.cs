using System;

namespace Griffin.PowerMate.EditorUI;

public class ListEventArgs<T> : EventArgs
{
	private T _Item;

	private int _Index;

	public T Item => _Item;

	public int Index => _Index;

	public ListEventArgs(T item, int index)
	{
		_Item = item;
		_Index = index;
	}
}
