using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Griffin.PowerMate.iTunesActions.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
[CompilerGenerated]
[DebuggerNonUserCode]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("Griffin.PowerMate.iTunesActions.Properties.Resources", typeof(Resources).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Icon fastforward
	{
		get
		{
			object obj = ResourceManager.GetObject("fastforward", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon next
	{
		get
		{
			object obj = ResourceManager.GetObject("next", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon pause
	{
		get
		{
			object obj = ResourceManager.GetObject("pause", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon play
	{
		get
		{
			object obj = ResourceManager.GetObject("play", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon previous
	{
		get
		{
			object obj = ResourceManager.GetObject("previous", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon rewind
	{
		get
		{
			object obj = ResourceManager.GetObject("rewind", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon stop
	{
		get
		{
			object obj = ResourceManager.GetObject("stop", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon volumedown
	{
		get
		{
			object obj = ResourceManager.GetObject("volumedown", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon volumemute
	{
		get
		{
			object obj = ResourceManager.GetObject("volumemute", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon volumeup
	{
		get
		{
			object obj = ResourceManager.GetObject("volumeup", resourceCulture);
			return (Icon)obj;
		}
	}

	internal Resources()
	{
	}
}
