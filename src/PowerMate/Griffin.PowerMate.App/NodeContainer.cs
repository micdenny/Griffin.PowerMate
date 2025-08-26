using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Griffin.PowerMate.App;

public abstract class NodeContainer<T> : PowerMateNode, IEnumerable<T>, IEnumerable where T : PowerMateNode
{
	protected List<T> NodeList = new List<T>();

	internal override XmlElement NodeXmlElement
	{
		get
		{
			XmlElement xmlElement = (XmlElement)NodeXml.Clone();
			using IEnumerator<T> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				xmlElement.AppendChild(current.NodeXmlElement);
			}
			return xmlElement;
		}
	}

	public int Count => NodeList.Count;

	public bool IsEmpty
	{
		get
		{
			if (NodeList.Count > 0)
			{
				return false;
			}
			return true;
		}
	}

	public T this[int index]
	{
		get
		{
			return NodeList[index];
		}
		set
		{
			T val = NodeList[index];
			NodeList[index] = value;
			if (val != value)
			{
				OnChildNodeReplaced(new NodeContainerEventArgs<T>(val, index));
			}
		}
	}

	public event EventHandler<NodeContainerEventArgs<T>> ChildNodeInserted;

	public event EventHandler<NodeContainerEventArgs<T>> ChildNodeRemoved;

	public event EventHandler<NodeContainerEventArgs<T>> ChildNodeReplaced;

	public event EventHandler NodeCleared;

	public NodeContainer()
	{
	}

	public NodeContainer(XmlElement elem)
		: base(elem)
	{
	}

	public void Add(T node)
	{
		NodeList.Add(node);
		OnChildNodeInserted(new NodeContainerEventArgs<T>(node, NodeList.Count - 1));
	}

	public void Clear()
	{
		NodeList.Clear();
		OnNodeCleared(EventArgs.Empty);
	}

	public bool Contains(T node)
	{
		return NodeList.Contains(node);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		NodeList.CopyTo(array, arrayIndex);
	}

	public void CopyTo(int index, T[] array, int arrayIndex, int count)
	{
		NodeList.CopyTo(index, array, arrayIndex, count);
	}

	public void CopyTo(T[] array)
	{
		NodeList.CopyTo(array);
	}

	public bool Exists(Predicate<T> match)
	{
		return NodeList.Exists(match);
	}

	public T Find(Predicate<T> match)
	{
		return NodeList.Find(match);
	}

	public virtual T[] FindAll(Predicate<T> match)
	{
		try
		{
			return NodeList.FindAll(match).ToArray();
		}
		catch
		{
			return new T[0];
		}
	}

	public int FindIndex(int startIndex, int count, Predicate<T> match)
	{
		return NodeList.FindIndex(startIndex, count, match);
	}

	public int FindIndex(int startIndex, Predicate<T> match)
	{
		return NodeList.FindIndex(startIndex, match);
	}

	public int FindIndex(Predicate<T> match)
	{
		return NodeList.FindIndex(match);
	}

	public T FindLast(Predicate<T> match)
	{
		return NodeList.FindLast(match);
	}

	public int FindLastIndex(int startIndex, int count, Predicate<T> match)
	{
		return NodeList.FindLastIndex(startIndex, count, match);
	}

	public int FindLastIndex(int startIndex, Predicate<T> match)
	{
		return NodeList.FindLastIndex(startIndex, match);
	}

	public int FindLastIndex(Predicate<T> match)
	{
		return NodeList.FindLastIndex(match);
	}

	public void ForEach(Action<T> action)
	{
		NodeList.ForEach(action);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return NodeList.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public virtual T[] GetRange(int index, int count)
	{
		return NodeList.GetRange(index, count).ToArray();
	}

	public int IndexOf(T item, int index, int count)
	{
		return NodeList.IndexOf(item, index, count);
	}

	public int IndexOf(T item, int index)
	{
		return NodeList.IndexOf(item, index);
	}

	public int IndexOf(T item)
	{
		return NodeList.IndexOf(item);
	}

	public void Insert(int index, T item)
	{
		NodeList.Insert(index, item);
		OnChildNodeInserted(new NodeContainerEventArgs<T>(item, index));
	}

	public int LastIndexOf(T item, int index, int count)
	{
		return NodeList.LastIndexOf(item, index, count);
	}

	public int LastIndexOf(T item, int index)
	{
		return NodeList.LastIndexOf(item, index);
	}

	public int LastIndexOf(T item)
	{
		return NodeList.LastIndexOf(item);
	}

	public bool Remove(T item)
	{
		int index = NodeList.IndexOf(item);
		try
		{
			NodeList.RemoveAt(index);
			OnChildNodeRemoved(new NodeContainerEventArgs<T>(item, index));
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void RemoveAt(int index)
	{
		T node = NodeList[index];
		NodeList.RemoveAt(index);
		OnChildNodeRemoved(new NodeContainerEventArgs<T>(node, index));
	}

	public T[] ToArray()
	{
		return NodeList.ToArray();
	}

	public bool TrueForAll(Predicate<T> match)
	{
		return NodeList.TrueForAll(match);
	}

	protected virtual void OnChildNodeInserted(NodeContainerEventArgs<T> e)
	{
		if (this.ChildNodeInserted != null)
		{
			this.ChildNodeInserted(this, e);
		}
	}

	protected virtual void OnChildNodeRemoved(NodeContainerEventArgs<T> e)
	{
		if (this.ChildNodeRemoved != null)
		{
			this.ChildNodeRemoved(this, e);
		}
	}

	protected virtual void OnChildNodeReplaced(NodeContainerEventArgs<T> e)
	{
		if (this.ChildNodeReplaced != null)
		{
			this.ChildNodeReplaced(this, e);
		}
	}

	protected virtual void OnNodeCleared(EventArgs e)
	{
		if (this.NodeCleared != null)
		{
			this.NodeCleared(this, e);
		}
	}
}
