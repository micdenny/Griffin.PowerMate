using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Griffin.PowerMate.EditorUI;

internal class RunningAppsHeaderColumn : HeaderColumn
{
	public void UpdateProcesses(bool includeGlobal)
	{
		foreach (RunningAppsColumnItem item in base.Items)
		{
			item.Dispose();
		}
		base.Items.Clear();
		Process[] processes = Process.GetProcesses();
		List<string> list = new List<string>(new string[1] { "explorer" });
		if (includeGlobal)
		{
			base.Items.Add(new RunningAppsColumnItem(null));
		}
		Process[] array = processes;
		foreach (Process process in array)
		{
			if (process.MainWindowHandle != IntPtr.Zero && !list.Contains(process.ProcessName))
			{
				list.Add(process.ProcessName);
				base.Items.Add(new RunningAppsColumnItem(process));
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			foreach (RunningAppsColumnItem item in base.Items)
			{
				item.Dispose();
			}
		}
		base.Dispose(disposing);
	}
}
