using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Griffin.PowerMate.App;

public static class IconHelper
{
	public static Icon GetIconFromFile(string pathToFile, int index, IconSize size)
	{
		Icon result = null;
		if (pathToFile != null)
		{
			int num = ExtractIconEx(pathToFile, -1, null, null, 0);
			if (index < num)
			{
				IntPtr[] array = new IntPtr[1];
				if (size == IconSize.Large)
				{
					ExtractIconEx(pathToFile, index, array, null, 1);
				}
				else
				{
					ExtractIconEx(pathToFile, index, null, array, 1);
				}
				result = (Icon)Icon.FromHandle(array[0]).Clone();
				DestroyIcon(array[0]);
			}
		}
		return result;
	}

	public static Icon GetIconFromFile(string pathAndIndex, IconSize size)
	{
		Icon result = null;
		if (!string.IsNullOrEmpty(pathAndIndex))
		{
			int num = pathAndIndex.LastIndexOf(',');
			try
			{
				int index = int.Parse(pathAndIndex.Substring(num + 1));
				string pathToFile = pathAndIndex.Substring(0, num);
				result = GetIconFromFile(pathToFile, index, size);
			}
			catch
			{
				result = GetIconFromFile(pathAndIndex, 0, size);
			}
		}
		return result;
	}

	public static Icon[] GetIconsFromFile(string pathToFile, IconSize size)
	{
		Icon[] array = null;
		if (pathToFile != null)
		{
			int num = ExtractIconEx(pathToFile, -1, null, null, 0);
			IntPtr[] array2 = new IntPtr[num];
			if (size == IconSize.Large)
			{
				ExtractIconEx(pathToFile, 0, array2, null, num);
			}
			else
			{
				ExtractIconEx(pathToFile, 0, null, array2, num);
			}
			array = new Icon[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (Icon)Icon.FromHandle(array2[i]).Clone();
				DestroyIcon(array2[i]);
			}
		}
		else
		{
			array = new Icon[0];
		}
		return array;
	}

	[DllImport("shell32.dll", CharSet = CharSet.Auto)]
	private static extern int ExtractIconEx([MarshalAs(UnmanagedType.LPTStr)] string lpszFile, int nIconIndex, IntPtr[] phIconLarge, IntPtr[] phIconSmall, int nIcons);

	[DllImport("user32.dll")]
	private static extern bool DestroyIcon(IntPtr handle);
}
