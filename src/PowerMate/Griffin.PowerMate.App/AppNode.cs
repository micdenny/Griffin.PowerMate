using System.Collections.Generic;
using System.Xml;
using Griffin.PowerMate.Device;

namespace Griffin.PowerMate.App;

public class AppNode : NodeContainer<ActionNode>
{
	public const string XmlNodeName = "app";

	protected bool IsActive = true;

	public virtual bool Active
	{
		get
		{
			return IsActive;
		}
		set
		{
			IsActive = value;
		}
	}

	public string Name
	{
		get
		{
			return NodeXml.GetAttribute("name");
		}
		set
		{
			NodeXml.SetAttribute("name", value);
			OnNodeSettingsChanged(new PowerMateNodeEventArgs("name"));
		}
	}

	public string Image => NodeXml.GetAttribute("image");

	public static string GlobalName => "Global Setting";

	public override string TagName => "app";

	public AppNode(string imageName)
	{
		if (!string.IsNullOrEmpty(imageName))
		{
			NodeXml.SetAttribute("image", imageName);
			NodeXml.SetAttribute("name", imageName);
		}
		else
		{
			NodeXml.SetAttribute("name", GlobalName);
		}
	}

	public AppNode(XmlElement elem, IPMActionPlugin[] plugins)
		: base(elem)
	{
		while (elem["action"] != null)
		{
			Add(new ActionNode((XmlElement)elem.RemoveChild(elem["action"]), plugins));
		}
	}

	public void PerformAll(IPowerMateDevice sender, PMAction action, ModifierKey modifier)
	{
		if (IsActive)
		{
			ActionNode[] array = FindAll(action, modifier);
			ActionNode[] array2 = array;
			foreach (ActionNode actionNode in array2)
			{
				actionNode.Perform(sender);
			}
		}
	}

	public void Perform(IPowerMateDevice sender, PMAction action, ModifierKey modifier)
	{
		if (IsActive)
		{
			Find(action, modifier)?.Perform(sender);
		}
	}

	public ActionNode[] FindAll(PMAction action, ModifierKey modifier)
	{
		List<ActionNode> list = new List<ActionNode>();
		foreach (ActionNode node in NodeList)
		{
			if (node.Action == action && node.Modifier == modifier)
			{
				list.Insert(0, node);
			}
		}
		return list.ToArray();
	}

	public ActionNode Find(PMAction action, ModifierKey modifier)
	{
		foreach (ActionNode node in NodeList)
		{
			if (node.Action == action && node.Modifier == modifier)
			{
				return node;
			}
		}
		return null;
	}

	public override PowerMateNode Clone()
	{
		AppNode appNode = new AppNode(null);
		appNode.NodeXml = (XmlElement)NodeXml.Clone();
		using IEnumerator<ActionNode> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			ActionNode current = enumerator.Current;
			appNode.Add((ActionNode)current.Clone());
		}
		return appNode;
	}
}
