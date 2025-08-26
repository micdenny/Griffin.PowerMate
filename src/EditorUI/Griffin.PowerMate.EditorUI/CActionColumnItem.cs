using System;
using System.Drawing;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal class CActionColumnItem : NodeColumnItem<ActionNode>
{
	public delegate void NodeChangeHandler(CActionColumnItem sender, ActionNode node);

	private bool EmptyComputerAction = true;

	private Color _SelectedTextColor = Color.Transparent;

	private EventHandler ComputerActionChanged;

	public new ActionNode Node
	{
		get
		{
			return base.Node;
		}
		set
		{
			if (myNode != null)
			{
				myNode.ComputerActionChanged -= ComputerActionChanged;
			}
			myNode = value;
			if (value != null)
			{
				value.ComputerActionChanged += ComputerActionChanged;
				EmptyComputerAction = value.ComputerAction == null;
			}
			else
			{
				EmptyComputerAction = true;
			}
			OnTextChanged();
			OnIconChanged();
		}
	}

	public override Icon Icon
	{
		get
		{
			if (myNode != null && myNode.ComputerAction != null)
			{
				return myNode.ComputerAction.Icon;
			}
			return null;
		}
	}

	public override string Text
	{
		get
		{
			if (myNode != null)
			{
				return myNode.Description;
			}
			return null;
		}
		set
		{
			if (myNode.Description != value)
			{
				myNode.Description = value;
				OnTextChanged();
			}
		}
	}

	public override Color TextColor
	{
		get
		{
			if (base.Selected)
			{
				return _SelectedTextColor;
			}
			return Color.Transparent;
		}
	}

	public Color SelectedTextColor
	{
		get
		{
			return _SelectedTextColor;
		}
		set
		{
			_SelectedTextColor = value;
			if (base.Selected)
			{
				OnTextChanged();
			}
		}
	}

	public event NodeChangeHandler AddNodeToCollection;

	public event NodeChangeHandler RemoveNodeFromCollection;

	public CActionColumnItem(ActionNode node)
		: base(node)
	{
		ComputerActionChanged = Node_ComputerActionChanged;
		Node = node;
	}

	private void Node_ComputerActionChanged(object sender, EventArgs e)
	{
		OnTextChanged();
		OnIconChanged();
		if (Node.ComputerAction == null != EmptyComputerAction)
		{
			EmptyComputerAction = Node.ComputerAction == null;
			if (EmptyComputerAction)
			{
				OnRemoveNodeFromCollection(Node);
			}
			else
			{
				OnAddNodeToCollection(Node);
			}
		}
	}

	protected virtual void OnAddNodeToCollection(ActionNode node)
	{
		if (this.AddNodeToCollection != null)
		{
			this.AddNodeToCollection(this, node);
		}
	}

	protected virtual void OnRemoveNodeFromCollection(ActionNode node)
	{
		if (this.RemoveNodeFromCollection != null)
		{
			this.RemoveNodeFromCollection(this, node);
		}
	}
}
