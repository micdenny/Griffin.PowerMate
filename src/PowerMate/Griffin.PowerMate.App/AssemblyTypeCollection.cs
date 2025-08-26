using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Griffin.PowerMate.App;

internal class AssemblyTypeCollection<T> : List<T>
{
	public AssemblyTypeCollection()
	{
	}

	public AssemblyTypeCollection(IEnumerable<T> collection)
		: base(collection)
	{
	}

	public int Load()
	{
		return Load(Application.StartupPath);
	}

	public int Load(string dirPath)
	{
		return Load(dirPath, "*");
	}

	public int Load(string dirPath, string fileExt)
	{
		return Load(dirPath, fileExt, SearchOption.TopDirectoryOnly);
	}

	public int Load(string dirPath, string fileExt, SearchOption searchOption)
	{
		int num = 0;
		if (Directory.Exists(dirPath))
		{
			string[] files = Directory.GetFiles(dirPath, "*." + fileExt, searchOption);
			foreach (string path in files)
			{
				try
				{
					num += AddFoundTypes(Assembly.LoadFile(path));
				}
				catch (Exception ex)
				{
					if (ex is ReflectionTypeLoadException)
					{
						_ = (ReflectionTypeLoadException)ex;
					}
				}
			}
		}
		return num;
	}

	public int Load(Assembly assembly)
	{
		return AddFoundTypes(assembly);
	}

	private int AddFoundTypes(Assembly assembly)
	{
		int num = 0;
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			if (type.ToString().Contains("PMEditorUI"))
			{
				num = num;
			}
			if (typeof(T).IsAssignableFrom(type))
			{
				Add((T)Activator.CreateInstance(type));
				num++;
			}
		}
		return num;
	}
}
