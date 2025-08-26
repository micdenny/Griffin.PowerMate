using System;
using System.Windows.Forms;
using System.Xml;
using Griffin.PowerMate.Device;

namespace Griffin.PowerMate.App;

public class ActionNode : PowerMateNode
{
	public const string XmlNodeName = "action";

	private IComputerAction CAction;

	private uint SensitivityCount;

	public string Description
	{
		get
		{
			string text = GetSetting("description");
			if (text == null && CAction != null)
			{
				text = CAction.Description;
			}
			return text;
		}
		set
		{
			SetSetting("description", value);
		}
	}

	public uint Sensitivity
	{
		get
		{
			try
			{
				return XmlConvert.ToUInt32(GetSetting("sensitivity"));
			}
			catch
			{
				return 0u;
			}
		}
		set
		{
			SetSetting("sensitivity", value.ToString());
		}
	}

	public PMAction Action
	{
		get
		{
			string attribute = NodeXml.GetAttribute("pmaction");
			try
			{
				return (PMAction)Enum.Parse(typeof(PMAction), attribute);
			}
			catch
			{
				return PMAction.ClockwiseRotate;
			}
		}
	}

	public ModifierKey Modifier
	{
		get
		{
			ModifierKey modifierKey = ModifierKey.None;
			string attribute = NodeXml.GetAttribute("modifier");
			string[] names = Enum.GetNames(typeof(ModifierKey));
			foreach (string value in names)
			{
				if (attribute.Contains(value))
				{
					modifierKey |= (ModifierKey)Enum.Parse(typeof(ModifierKey), value);
				}
			}
			return modifierKey;
		}
	}

	public IComputerAction ComputerAction
	{
		get
		{
			return CAction;
		}
		set
		{
			CAction = value;
			if (value != null)
			{
				NodeXml.SetAttribute("caction", value.Name);
				PluginName = value.PluginName;
			}
			else
			{
				NodeXml.RemoveAttribute("caction");
				PluginName = null;
			}
			PanelSettings = null;
			Description = null;
			if (this.ComputerActionChanged != null)
			{
				this.ComputerActionChanged(this, new EventArgs());
			}
		}
	}

	public Panel Panel
	{
		get
		{
			Panel panel = null;
			if (CAction != null)
			{
				panel = CAction.Panel;
			}
			if (panel is IPMActionPanel)
			{
				((IPMActionPanel)panel).Settings = PanelSettings;
			}
			return panel;
		}
	}

	public string[] PanelSettings
	{
		get
		{
			XmlElement xmlElement = (XmlElement)NodeXml.SelectSingleNode("//panel");
			if (xmlElement != null)
			{
				XmlNodeList elementsByTagName = xmlElement.GetElementsByTagName("string");
				string[] array = new string[elementsByTagName.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = elementsByTagName[i].InnerText;
				}
				return array;
			}
			return null;
		}
		set
		{
			XmlElement xmlElement = (XmlElement)NodeXml.SelectSingleNode("//panel");
			if (xmlElement == null && value != null)
			{
				xmlElement = PowerMateNode.Source.CreateElement("panel");
				NodeXml.AppendChild(xmlElement);
			}
			if (xmlElement != null && value != null)
			{
				XmlNodeList elementsByTagName = xmlElement.GetElementsByTagName("string");
				for (int i = 0; i < value.Length; i++)
				{
					if (i < elementsByTagName.Count)
					{
						elementsByTagName[i].InnerText = value[i];
					}
					else
					{
						xmlElement.AppendChild(PowerMateNode.Source.CreateElement("string")).InnerText = value[i];
					}
				}
				for (int j = 0; j < elementsByTagName.Count - value.Length; j++)
				{
					xmlElement.RemoveChild(elementsByTagName[value.Length + j]);
				}
			}
			else if (xmlElement != null && value == null)
			{
				NodeXml.RemoveChild(xmlElement);
			}
		}
	}

	public string PluginName
	{
		get
		{
			return NodeXml.GetAttribute("plugin");
		}
		protected set
		{
			if (value != null)
			{
				NodeXml.SetAttribute("plugin", value);
			}
			else
			{
				NodeXml.RemoveAttribute("plugin");
			}
		}
	}

	public override string TagName => "action";

	public event EventHandler ComputerActionChanged;

	public ActionNode(PMAction pmAction, IComputerAction compAction)
	{
		NodeXml.SetAttribute("pmaction", pmAction.ToString());
		if (compAction != null)
		{
			NodeXml.SetAttribute("caction", compAction.Name);
			PluginName = compAction.PluginName;
		}
		CAction = compAction;
	}

	public ActionNode(PMAction pmAction, ModifierKey modifier, IComputerAction compAction)
		: this(pmAction, compAction)
	{
		string text = "";
		foreach (byte value in Enum.GetValues(typeof(ModifierKey)))
		{
			if (((byte)modifier & value) == value)
			{
				if (text != "")
				{
					text += " | ";
				}
				text += value;
			}
		}
		NodeXml.SetAttribute("modifier", text);
	}

	public ActionNode(XmlElement elem, IPMActionPlugin[] plugins)
		: base(elem)
	{
		CAction = FindCompAction(plugins);
	}

	public bool Perform(IPowerMateDevice sender)
	{
		SensitivityCount++;
		if (CAction != null && SensitivityCount >= Sensitivity)
		{
			SensitivityCount = 0u;
			return CAction.Perform(sender, PanelSettings);
		}
		return false;
	}

	private IComputerAction FindCompAction(IPMActionPlugin[] plugins)
	{
		string attribute = NodeXml.GetAttribute("caction");
		string pluginName = PluginName;
		PMAction action = Action;
		if (attribute != null)
		{
			foreach (IPMActionPlugin iPMActionPlugin in plugins)
			{
				if (!string.IsNullOrEmpty(pluginName) && !(pluginName == iPMActionPlugin.Name))
				{
					continue;
				}
				IComputerAction[] availableActions = iPMActionPlugin.AvailableActions;
				foreach (IComputerAction computerAction in availableActions)
				{
					if (attribute == computerAction.Name && computerAction.SupportsPMAction(action))
					{
						return computerAction;
					}
				}
			}
		}
		return null;
	}

	public override PowerMateNode Clone()
	{
		ActionNode actionNode = new ActionNode(NodeXmlElement, new IPMActionPlugin[0]);
		actionNode.CAction = CAction;
		return actionNode;
	}
}
