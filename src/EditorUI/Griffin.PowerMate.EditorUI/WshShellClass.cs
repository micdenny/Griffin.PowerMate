using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Griffin.PowerMate.EditorUI;

internal class WshShellClass : IDisposable
{
	private object wshShellClass;

	private bool Disposed;

	public IWshShortcut CreateShortcut(string path)
	{
		if (!Disposed)
		{
			return new IWshShortcut(wshShellClass.GetType().InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, wshShellClass, new object[1] { path }));
		}
		return null;
	}

	public WshShellClass()
	{
		Guid clsid = new Guid("72c24dd5-d70a-438b-8a42-98424b88afb8");
		Type typeFromCLSID = Type.GetTypeFromCLSID(clsid);
		wshShellClass = Activator.CreateInstance(typeFromCLSID);
	}

	public void Dispose()
	{
		if (!Disposed)
		{
			Marshal.ReleaseComObject(wshShellClass);
			wshShellClass = null;
		}
	}
}
