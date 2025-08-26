using System.Collections.Generic;
using System.IO;
using System.Xml;
using Griffin.PowerMate.Device;

namespace Griffin.PowerMate.App;

public class PowerMateDoc : NodeContainer<DeviceNode>, ISavable
{
	public const string XmlNodeName = "PowerMateDoc";

	private const DeviceAssignment DefaultAssigment = DeviceAssignment.Mixed;

	private string FilePath;

	public string Path
	{
		get
		{
			return FilePath;
		}
		set
		{
			FilePath = value;
		}
	}

	public bool UpdateCheck
	{
		get
		{
			try
			{
				return XmlConvert.ToBoolean(GetSetting("updatecheck").ToLower());
			}
			catch
			{
				return true;
			}
		}
		set
		{
			SetSetting("updatecheck", value.ToString());
		}
	}

	public override string TagName => "PowerMateDoc";

	public PowerMateDoc()
	{
	}

	public PowerMateDoc(XmlDocument doc, IPMActionPlugin[] plugins)
		: base(doc.DocumentElement)
	{
		while (NodeXml["device"] != null)
		{
			Add(new DeviceNode((XmlElement)NodeXml.RemoveChild(NodeXml["device"]), plugins));
		}
	}

	public PowerMateDoc(XmlDocument doc, IPowerMateDevice[] powermates, IPMActionPlugin[] plugins)
		: this(doc, plugins)
	{
		AssignPowerMates(powermates, DeviceAssignment.Mixed);
	}

	public PowerMateDoc(string path, IPMActionPlugin[] plugins)
		: this(LoadXmlDoc(path), plugins)
	{
		FilePath = path;
	}

	public PowerMateDoc(string path, IPowerMateDevice[] powermates, IPMActionPlugin[] plugins)
		: this(path, plugins)
	{
		AssignPowerMates(powermates, DeviceAssignment.Mixed);
	}

	private static XmlDocument LoadXmlDoc(string path)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(path);
		return xmlDocument;
	}

	public bool Save()
	{
		if (FilePath != null)
		{
			return Save(FilePath, overwrite: true);
		}
		return false;
	}

	public bool Save(string path)
	{
		try
		{
			PowerMateNode.Source.InnerXml = NodeXmlElement.OuterXml;
			PowerMateNode.Source.Save(path);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public bool Save(string path, bool overwrite)
	{
		if (!overwrite && File.Exists(path))
		{
			return false;
		}
		return Save(path);
	}

	public void AssignPowerMates(IPowerMateDevice[] powermates, DeviceAssignment assignment)
	{
		switch (assignment)
		{
		case DeviceAssignment.ByEnumeration:
			AssignPMByEnumeration(powermates);
			break;
		case DeviceAssignment.ByPath:
			AssignPMByPath(powermates);
			break;
		case DeviceAssignment.Mixed:
			AssignPMMixed(powermates);
			break;
		case DeviceAssignment.Default:
			AssignPowerMates(powermates, DeviceAssignment.Mixed);
			break;
		}
	}

	private void AssignPMByEnumeration(IPowerMateDevice[] powermates)
	{
		int i;
		for (i = 0; i < base.Count; i++)
		{
			if (i < powermates.Length)
			{
				base[i].Device = powermates[i];
			}
			else
			{
				base[i].Device = null;
			}
		}
		for (; i < powermates.Length; i++)
		{
			Add(new DeviceNode(powermates[i]));
		}
	}

	private void AssignPMByPath(IPowerMateDevice[] powermates)
	{
		foreach (IPowerMateDevice powerMateDevice in powermates)
		{
			bool flag = false;
			using (IEnumerator<DeviceNode> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DeviceNode current = enumerator.Current;
					if (powerMateDevice.Name == current.LastPath)
					{
						current.Device = powerMateDevice;
						flag = true;
					}
				}
			}
			if (!flag)
			{
				Add(new DeviceNode(powerMateDevice));
			}
		}
	}

	private void AssignPMMixed(IPowerMateDevice[] powermates)
	{
		List<IPowerMateDevice> list = new List<IPowerMateDevice>();
		using (IEnumerator<DeviceNode> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DeviceNode current = enumerator.Current;
				current.Device = null;
			}
		}
		foreach (IPowerMateDevice powerMateDevice in powermates)
		{
			bool flag = false;
			using (IEnumerator<DeviceNode> enumerator2 = GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					DeviceNode current2 = enumerator2.Current;
					if (powerMateDevice.Name == current2.LastPath)
					{
						current2.Device = powerMateDevice;
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				list.Add(powerMateDevice);
			}
		}
		int j = 0;
		for (int k = 0; k < base.Count; k++)
		{
			if (j >= list.Count)
			{
				break;
			}
			if (!base[k].HasDevice)
			{
				base[k].Device = list[j];
				j++;
			}
		}
		for (; j < list.Count; j++)
		{
			Add(new DeviceNode(list[j]));
		}
	}

	public void ActivateAll()
	{
		using IEnumerator<DeviceNode> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			DeviceNode current = enumerator.Current;
			current.Active = true;
		}
	}

	public void Close()
	{
		using IEnumerator<DeviceNode> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			DeviceNode current = enumerator.Current;
			current.Active = false;
		}
	}

	public PowerMateDoc MergeDocs(params PowerMateDoc[] docs)
	{
		PowerMateDoc powerMateDoc = (PowerMateDoc)Clone();
		foreach (PowerMateDoc powerMateDoc2 in docs)
		{
			foreach (XmlElement item in powerMateDoc2.NodeXmlElement)
			{
				if (item.Name != "settings")
				{
					powerMateDoc.NodeXmlElement.AppendChild(item);
				}
			}
		}
		return powerMateDoc;
	}

	public override PowerMateNode Clone()
	{
		PowerMateDoc powerMateDoc = new PowerMateDoc();
		powerMateDoc.FilePath = FilePath;
		powerMateDoc.NodeXml = (XmlElement)NodeXml.Clone();
		using IEnumerator<DeviceNode> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			DeviceNode current = enumerator.Current;
			powerMateDoc.Add((DeviceNode)current.Clone());
		}
		return powerMateDoc;
	}
}
