using System;
using System.Xml;

namespace Griffin.PowerMate.App;

public abstract class PowerMateNode
{
	private static XmlDocument Owner = new XmlDocument();

	protected XmlElement NodeXml;

	protected static XmlDocument Source => Owner;

	internal virtual XmlElement NodeXmlElement => (XmlElement)NodeXml.Clone();

	public abstract string TagName { get; }

	public event EventHandler<PowerMateNodeEventArgs> NodeSettingsChanged;

	public PowerMateNode()
	{
		NodeXml = Source.CreateElement(TagName);
	}

	public PowerMateNode(XmlElement elem)
	{
		if (elem.Name == TagName)
		{
			if (elem.OwnerDocument != Owner)
			{
				elem = (XmlElement)Owner.ImportNode(elem, deep: true);
			}
			NodeXml = elem;
			return;
		}
		throw new Exception("XmlElement is not a PowerMateNode of type " + GetType().ToString());
	}

	public string GetSetting(string type)
	{
		return NodeXml.SelectSingleNode("//settings/" + type)?.InnerText;
	}

	public void SetSetting(string type, string value)
	{
		XmlNode xmlNode = NodeXml.SelectSingleNode("//settings/" + type);
		if (xmlNode != null && value == null)
		{
			xmlNode.ParentNode.RemoveChild(xmlNode);
		}
		else
		{
			if (value == null)
			{
				return;
			}
			if (xmlNode == null)
			{
				xmlNode = NodeXml.SelectSingleNode("//settings");
				if (xmlNode == null)
				{
					xmlNode = NodeXml.AppendChild(Source.CreateElement("settings"));
				}
				xmlNode = xmlNode.AppendChild(Source.CreateElement(type));
			}
			xmlNode.InnerText = value;
			OnNodeSettingsChanged(new PowerMateNodeEventArgs(type));
		}
	}

	public abstract PowerMateNode Clone();

	protected virtual void OnNodeSettingsChanged(PowerMateNodeEventArgs e)
	{
		if (this.NodeSettingsChanged != null)
		{
			this.NodeSettingsChanged(this, e);
		}
	}
}
