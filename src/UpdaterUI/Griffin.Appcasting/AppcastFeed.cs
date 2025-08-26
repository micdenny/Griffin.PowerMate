using System;
using System.IO;
using System.Net;
using System.Xml;

namespace Griffin.Appcasting;

public class AppcastFeed
{
	private string _Location;

	private string _Title;

	private XmlNode FeedNode;

	public bool Found
	{
		get
		{
			if (FeedNode != null)
			{
				return true;
			}
			return false;
		}
	}

	public string Location
	{
		get
		{
			return _Location;
		}
		set
		{
			if (value != _Location)
			{
				_Location = value;
				OnLocationChanged(EventArgs.Empty);
			}
		}
	}

	public string Title
	{
		get
		{
			return _Title;
		}
		set
		{
			if (value != _Title)
			{
				_Title = value;
				OnTitleChanged(EventArgs.Empty);
			}
		}
	}

	public string Link
	{
		get
		{
			try
			{
				return FeedNode["link"].InnerText;
			}
			catch
			{
				return null;
			}
		}
	}

	public string Description
	{
		get
		{
			try
			{
				return FeedNode["description"].InnerText;
			}
			catch
			{
				return null;
			}
		}
	}

	public string Language
	{
		get
		{
			try
			{
				return FeedNode["language"].InnerText;
			}
			catch
			{
				return null;
			}
		}
	}

	public string PubDate
	{
		get
		{
			try
			{
				return FeedNode["pubDate"].InnerText;
			}
			catch
			{
				return null;
			}
		}
	}

	public AppcastItem[] Items
	{
		get
		{
			try
			{
				XmlNodeList elementsByTagName = ((XmlElement)FeedNode).GetElementsByTagName("item");
				AppcastItem[] array = new AppcastItem[elementsByTagName.Count];
				for (int i = 0; i < elementsByTagName.Count; i++)
				{
					array[i] = new AppcastItem(elementsByTagName[i]);
				}
				return array;
			}
			catch
			{
				return new AppcastItem[0];
			}
		}
	}

	public AppcastItem GreatestBuildItem => AppcastItem.FindGreatestBuild(Items);

	public AppcastItem GreatestVersionItem => AppcastItem.FindGreatestVersion(Items);

	public AppcastItem MostRecentlyPublishedItem
	{
		get
		{
			AppcastItem appcastItem = null;
			AppcastItem[] items = Items;
			foreach (AppcastItem appcastItem2 in items)
			{
				if (appcastItem == null || appcastItem2.PubDate > appcastItem.PubDate)
				{
					appcastItem = appcastItem2;
				}
			}
			return appcastItem;
		}
	}

	public AppcastItem TopItem
	{
		get
		{
			XmlNode xmlNode = FeedNode["item"];
			if (xmlNode != null)
			{
				return new AppcastItem(xmlNode);
			}
			return null;
		}
	}

	public event EventHandler Reloaded;

	public event EventHandler LocationChanged;

	public event EventHandler TitleChanged;

	public AppcastFeed(string location)
		: this(location, null)
	{
	}

	public AppcastFeed(string location, string title)
	{
		_Location = location;
		_Title = title;
	}

	public void Reload()
	{
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Location);
			httpWebRequest.UserAgent = AppcastSettings.UserAgent;
			WebResponse response = httpWebRequest.GetResponse();
			Stream responseStream = response.GetResponseStream();
			xmlDocument.Load(responseStream);
			responseStream.Close();
			response.Close();
			foreach (XmlElement item in xmlDocument.GetElementsByTagName("channel"))
			{
				XmlElement xmlElement2 = item["title"];
				if (Title == null || xmlElement2.InnerText == Title)
				{
					FeedNode = item;
					break;
				}
			}
		}
		catch
		{
			FeedNode = null;
		}
		OnReloaded(EventArgs.Empty);
	}

	protected virtual void OnReloaded(EventArgs e)
	{
		if (this.Reloaded != null)
		{
			this.Reloaded(this, e);
		}
	}

	protected virtual void OnLocationChanged(EventArgs e)
	{
		if (this.LocationChanged != null)
		{
			this.LocationChanged(this, e);
		}
	}

	protected virtual void OnTitleChanged(EventArgs e)
	{
		if (this.TitleChanged != null)
		{
			this.TitleChanged(this, e);
		}
	}
}
