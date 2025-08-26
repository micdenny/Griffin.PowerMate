using System;

namespace Griffin.PowerMate.App;

public class NodeContainerEventArgs<T> : EventArgs
{
	private T ArgNode;

	private int ArgIndex;

	public T Node => ArgNode;

	public int Index => ArgIndex;

	public NodeContainerEventArgs(T node, int index)
	{
		ArgNode = node;
		ArgIndex = index;
	}
}
