using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Griffin.PowerMate.MouseActions.Properties;

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
				ResourceManager resourceManager = new ResourceManager("Griffin.PowerMate.MouseActions.Properties.Resources", typeof(Resources).Assembly);
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

	internal static Icon cursorDown
	{
		get
		{
			object obj = ResourceManager.GetObject("cursorDown", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon cursorLeft
	{
		get
		{
			object obj = ResourceManager.GetObject("cursorLeft", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon cursorRight
	{
		get
		{
			object obj = ResourceManager.GetObject("cursorRight", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon cursorUp
	{
		get
		{
			object obj = ResourceManager.GetObject("cursorUp", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon doubleClick
	{
		get
		{
			object obj = ResourceManager.GetObject("doubleClick", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon leftClick
	{
		get
		{
			object obj = ResourceManager.GetObject("leftClick", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon middleClick
	{
		get
		{
			object obj = ResourceManager.GetObject("middleClick", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon Mouse
	{
		get
		{
			object obj = ResourceManager.GetObject("Mouse", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon rightClick
	{
		get
		{
			object obj = ResourceManager.GetObject("rightClick", resourceCulture);
			return (Icon)obj;
		}
	}

	internal Resources()
	{
	}
}
