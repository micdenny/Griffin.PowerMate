using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Griffin.PowerMate.App;

public class AppCollection : NodeContainer<AppNode>, ISavable
{
	private const string XmlNodeName = "PowerMateApps";

	public override string TagName => "PowerMateApps";

	public AppCollection()
	{
	}

	public AppCollection(XmlDocument doc, IPMActionPlugin[] plugins)
		: base(doc.DocumentElement)
	{
		while (NodeXml["app"] != null)
		{
			Add(new AppNode((XmlElement)NodeXml.RemoveChild(NodeXml["app"]), plugins));
		}
	}

	public AppCollection(string path, IPMActionPlugin[] plugins)
		: this(LoadXmlDoc(path), plugins)
	{
	}

	private static XmlDocument LoadXmlDoc(string path)
	{
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.Load(path);
		}
		catch
		{
			xmlDocument.AppendChild(xmlDocument.CreateElement("PowerMateApps"));
		}
		return xmlDocument;
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

	public AppNode Find(string image, bool caseSensitive)
	{
		if (caseSensitive)
		{
			using IEnumerator<AppNode> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				AppNode current = enumerator.Current;
				if (current.Image == image.ToLower())
				{
					return current;
				}
			}
		}
		else
		{
			image = image.ToLower();
			using IEnumerator<AppNode> enumerator2 = GetEnumerator();
			while (enumerator2.MoveNext())
			{
				AppNode current2 = enumerator2.Current;
				if (current2.Image.ToLower() == image.ToLower())
				{
					return current2;
				}
			}
		}
		return null;
	}

	public override PowerMateNode Clone()
	{
		AppCollection appCollection = new AppCollection();
		using IEnumerator<AppNode> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			AppNode current = enumerator.Current;
			appCollection.Add((AppNode)current.Clone());
		}
		return appCollection;
	}
}
