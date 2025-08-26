using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Griffin.PowerMate.VolumeActions.Properties;

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
				ResourceManager resourceManager = new ResourceManager("Griffin.PowerMate.VolumeActions.Properties.Resources", typeof(Resources).Assembly);
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

	internal static Icon volumeDown
	{
		get
		{
			object obj = ResourceManager.GetObject("volumeDown", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon volumeMute
	{
		get
		{
			object obj = ResourceManager.GetObject("volumeMute", resourceCulture);
			return (Icon)obj;
		}
	}

	internal static Icon volumeUp
	{
		get
		{
			object obj = ResourceManager.GetObject("volumeUp", resourceCulture);
			return (Icon)obj;
		}
	}

	internal Resources()
	{
	}
}
