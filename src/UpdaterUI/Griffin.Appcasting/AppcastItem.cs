using System;
using System.Collections.Generic;
using System.Xml;

namespace Griffin.Appcasting;

public class AppcastItem
{
	private XmlNode FeedNode;

	public string Title
	{
		get
		{
			try
			{
				return FeedNode["title"].InnerText;
			}
			catch
			{
				return null;
			}
		}
	}

	public string Location
	{
		get
		{
			try
			{
				return FeedNode["enclosure"].GetAttribute("url");
			}
			catch
			{
				return null;
			}
		}
	}

	public string Version
	{
		get
		{
			try
			{
				return FeedNode["enclosure"].GetAttribute("version");
			}
			catch
			{
				return null;
			}
		}
	}

	public string Build
	{
		get
		{
			try
			{
				return FeedNode["enclosure"].GetAttribute("build");
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

	public DateTime PubDate
	{
		get
		{
			try
			{
				return XmlConvert.ToDateTime(FeedNode["pubdate"].InnerText, XmlDateTimeSerializationMode.Local);
			}
			catch
			{
				return DateTime.MinValue;
			}
		}
	}

	public uint Size
	{
		get
		{
			try
			{
				return XmlConvert.ToUInt32(FeedNode["enclosure"].GetAttribute("length"));
			}
			catch
			{
				return 0u;
			}
		}
	}

	public AppcastItem(XmlNode xmlNode)
	{
		FeedNode = xmlNode;
	}

	public override string ToString()
	{
		return Title + " " + Version + " " + Build;
	}

	public int CompareVersion(string version)
	{
		return CompareVersion(Version, version);
	}

	public int CompareVersion(AppcastItem appcastItem)
	{
		return CompareVersion(appcastItem.Version);
	}

	public int CompareBuild(string build)
	{
		return CompareBuild(Build, build);
	}

	public int CompareBuild(AppcastItem appcastItem)
	{
		return CompareBuild(appcastItem.Build);
	}

	public static int CompareBuild(string build1, string build2)
	{
		if (build1 != null && build2 != null)
		{
			int totalWidth = Math.Max(build1.Length, build2.Length);
			build1 = build1.PadLeft(totalWidth, '0');
			build2 = build2.PadLeft(totalWidth, '0');
		}
		return string.Compare(build1, build2);
	}

	public static int CompareVersion(string version1, string version2)
	{
		int num = MainCompareVersion(version1, version2);
		if (num == 0)
		{
			num = AlphaBetaCompare(version1, version2, 'a');
		}
		if (num == 0)
		{
			num = AlphaBetaCompare(version1, version2, 'b');
		}
		return num;
	}

	private static int AlphaBetaCompare(string version1, string version2, char AorB)
	{
		int num = version1.ToLower().IndexOf(char.ToLower(AorB));
		int num2 = version2.ToLower().IndexOf(char.ToLower(AorB));
		if (num < 0 && num2 >= 0)
		{
			return 1;
		}
		if (num >= 0 && num2 < 0)
		{
			return -1;
		}
		if (num >= 0 && num2 >= 0)
		{
			return MainCompareVersion(version1.Substring(num + 1, version1.Length - (num + 1)), version2.Substring(num2 + 1, version2.Length - (num2 + 1)));
		}
		return 0;
	}

	private static int MainCompareVersion(string version1, string version2)
	{
		try
		{
			string[] array = version1.Split('.');
			string[] array2 = version2.Split('.');
			for (int i = 0; i < Math.Min(array2.Length, array.Length); i++)
			{
				int num = XmlConvert.ToInt32(array[i]);
				int num2 = XmlConvert.ToInt32(array2[i]);
				if (num > num2)
				{
					return 1;
				}
				if (num < num2)
				{
					return -1;
				}
			}
			if (array2.Length > array.Length)
			{
				for (int j = array.Length; j < array2.Length; j++)
				{
					if (XmlConvert.ToInt32(array2[j]) > 0)
					{
						return -1;
					}
				}
			}
			else if (array.Length > array2.Length)
			{
				for (int k = array2.Length; k < array.Length; k++)
				{
					if (XmlConvert.ToInt32(array[k]) > 0)
					{
						return 1;
					}
				}
			}
		}
		catch
		{
		}
		return 0;
	}

	public static AppcastItem FindGreatestVersion(IEnumerable<AppcastItem> items)
	{
		AppcastItem appcastItem = null;
		foreach (AppcastItem item in items)
		{
			if (appcastItem == null || CompareVersion(item.Build, appcastItem.Build) > 0)
			{
				appcastItem = item;
			}
		}
		return appcastItem;
	}

	public static AppcastItem FindGreatestBuild(IEnumerable<AppcastItem> items)
	{
		AppcastItem appcastItem = null;
		foreach (AppcastItem item in items)
		{
			if (appcastItem == null || CompareBuild(item.Build, appcastItem.Build) > 0)
			{
				appcastItem = item;
			}
		}
		return appcastItem;
	}
}
