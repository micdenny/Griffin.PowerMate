using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Griffin.PowerMate.EditorUI.Properties;

[CompilerGenerated]
[DebuggerNonUserCode]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
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
				ResourceManager resourceManager = new ResourceManager("Griffin.PowerMate.EditorUI.Properties.Resources", typeof(Resources).Assembly);
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

	internal static Bitmap add
	{
		get
		{
			object obj = ResourceManager.GetObject("add", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Icon click
	{
		get
		{
			object obj = ResourceManager.GetObject("click", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon clickLeft
	{
		get
		{
			object obj = ResourceManager.GetObject("clickLeft", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon clickRight
	{
		get
		{
			object obj = ResourceManager.GetObject("clickRight", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static string credits => ResourceManager.GetString("credits", resourceCulture);

	internal static Icon dimPowerMate
	{
		get
		{
			object obj = ResourceManager.GetObject("dimPowerMate", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon global
	{
		get
		{
			object obj = ResourceManager.GetObject("global", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Bitmap hidePowerMates
	{
		get
		{
			object obj = ResourceManager.GetObject("hidePowerMates", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Icon left
	{
		get
		{
			object obj = ResourceManager.GetObject("left", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon longClick
	{
		get
		{
			object obj = ResourceManager.GetObject("longClick", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon PowerMate
	{
		get
		{
			object obj = ResourceManager.GetObject("PowerMate", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Bitmap powerMateHelp
	{
		get
		{
			object obj = ResourceManager.GetObject("powerMateHelp", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Icon right
	{
		get
		{
			object obj = ResourceManager.GetObject("right", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Bitmap showPowerMates
	{
		get
		{
			object obj = ResourceManager.GetObject("showPowerMates", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal Resources()
	{
	}
}
