using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Griffin.PowerMate.EditorUI;

internal class IWshShortcut : IDisposable
{
	private object iWshShortcut;

	private bool Disposed;

	public string TargetPath
	{
		get
		{
			if (!Disposed)
			{
				return iWshShortcut.GetType().InvokeMember("TargetPath", BindingFlags.GetProperty, null, iWshShortcut, null).ToString();
			}
			return null;
		}
		set
		{
			iWshShortcut.GetType().InvokeMember("TargetPath", BindingFlags.SetProperty, null, iWshShortcut, new object[1] { value });
		}
	}

	public string Description
	{
		get
		{
			if (!Disposed)
			{
				return iWshShortcut.GetType().InvokeMember("Description", BindingFlags.GetProperty, null, iWshShortcut, null).ToString();
			}
			return null;
		}
		set
		{
			if (!Disposed)
			{
				iWshShortcut.GetType().InvokeMember("Description", BindingFlags.SetProperty, null, iWshShortcut, new object[1] { value });
			}
		}
	}

	public string IconLocation
	{
		get
		{
			if (!Disposed)
			{
				return iWshShortcut.GetType().InvokeMember("IconLocation", BindingFlags.GetProperty, null, iWshShortcut, null).ToString();
			}
			return null;
		}
		set
		{
			if (!Disposed)
			{
				iWshShortcut.GetType().InvokeMember("IconLocation", BindingFlags.SetProperty, null, iWshShortcut, new object[1] { value });
			}
		}
	}

	public void Save()
	{
		if (!Disposed)
		{
			iWshShortcut.GetType().InvokeMember("Save", BindingFlags.InvokeMethod, null, iWshShortcut, null);
		}
	}

	public IWshShortcut(object _iWshShortcut)
	{
		iWshShortcut = _iWshShortcut;
	}

	public void Dispose()
	{
		if (!Disposed)
		{
			Marshal.ReleaseComObject(iWshShortcut);
			iWshShortcut = null;
		}
	}
}
