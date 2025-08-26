using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Griffin.PowerMate.Device;
using Microsoft.Win32;

namespace Griffin.PowerMate.App;

public static class PowerMateApp
{
	private const string PMAppVersion = "2.0.1";

	private const string PMAppBuild = "61";

	private static string UIPluginPath = Application.StartupPath + "\\Plugins";

	private static string ActionPluginPath = Application.StartupPath + "\\Plugins";

	private static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GriffinTechnology\\PowerMate\\";

	private static string PMDocPath = AppDataPath + "user.pmsettings";

	private static PowerModeChangedEventHandler PowerModeChanged;

	private static PowerMateMessageWindow DeviceChangeNotifier;

	private static IPowerMateDevice[] PowerMateDevices;

	private static PowerMateDoc PMDoc;

	private static AssemblyTypeCollection<IPMActionPlugin> PMActionPlugins;

	private static AssemblyTypeCollection<IPowerMateUIPlugin> PMUIPlugins;

	public static string Version => "2.0.1";

	public static string Build => "61";

	public static PowerMateDoc PowerMateDoc
	{
		get
		{
			return PMDoc;
		}
		set
		{
			PMDoc = value;
			foreach (IPowerMateUIPlugin pMUIPlugin in PMUIPlugins)
			{
				pMUIPlugin.PowerMateDoc = value;
			}
		}
	}

	public static IPowerMateUIPlugin[] UIPlugins => PMUIPlugins.ToArray();

	public static IPMActionPlugin[] ActionPlugins => PMActionPlugins.ToArray();

	public static IPowerMateDevice[] PowerMates => (IPowerMateDevice[])PowerMateDevices.Clone();

	private static bool AlreadyRunning
	{
		get
		{
			string processName = Process.GetCurrentProcess().ProcessName;
			Process[] processesByName = Process.GetProcessesByName(processName);
			if (processesByName.Length > 1)
			{
				Process[] array = processesByName;
				foreach (Process process in array)
				{
					if (process == Process.GetCurrentProcess())
					{
						continue;
					}
					ProcessThread processThread = null;
					foreach (ProcessThread thread in process.Threads)
					{
						if (processThread == null || thread.StartTime < processThread.StartTime)
						{
							processThread = thread;
						}
					}
					if (processThread != null)
					{
						PostThreadMessage(processThread.Id, 6, new IntPtr(1), IntPtr.Zero);
					}
				}
				return true;
			}
			return false;
		}
	}

	public static event EventHandler UIPluginsLoaded;

	[STAThread]
	internal static void Main(string[] args)
	{
		try
		{
			if (AlreadyRunning)
			{
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			DeviceChangeNotifier = new PowerMateMessageWindow();
			DeviceChangeNotifier.DeviceAdded += DeviceChangeNotifier_DeviceAdded;
			PMActionPlugins = new AssemblyTypeCollection<IPMActionPlugin>();
			PMActionPlugins.Load(ActionPluginPath);
			PowerMateDevices = FindPowerMates();
			try
			{
				PMDoc = new PowerMateDoc(args[0], ActionPlugins);
			}
			catch
			{
				if (!Directory.Exists(AppDataPath))
				{
					Directory.CreateDirectory(AppDataPath);
				}
				try
				{
					PMDoc = new PowerMateDoc(PMDocPath, ActionPlugins);
				}
				catch
				{
					PMDoc = new PowerMateDoc();
					PMDoc.Path = PMDocPath;
				}
			}
			Application.ApplicationExit += Application_ApplicationExit;
			SystemEvents.SessionEnding += Application_ApplicationExit;
			SystemEvents.PowerModeChanged += (PowerModeChanged = SystemEvents_PowerModeChanged);
			PMUIPlugins = new AssemblyTypeCollection<IPowerMateUIPlugin>(new IPowerMateUIPlugin[1]
			{
				new PMNotifyIconUI()
			});
			PMUIPlugins.Load(UIPluginPath);
			OnUIPluginsLoaded(EventArgs.Empty);
			PMDoc.AssignPowerMates(PowerMateDevices, DeviceAssignment.Default);
			Application.Run();
		}
		catch (Exception e)
		{
			ShowThreadExceptionDialog(e);
		}
	}

	private static void OnUIPluginsLoaded(EventArgs e)
	{
		if (PowerMateApp.UIPluginsLoaded != null)
		{
			PowerMateApp.UIPluginsLoaded(null, e);
		}
	}

	[DllImport("user32.dll")]
	private static extern bool PostThreadMessage(int idThread, int Msg, IntPtr wParam, IntPtr lParam);

	private static IPowerMateDevice[] FindPowerMates()
	{
		HIDPowerMate[] all = HIDPowerMate.GetAll();
		USBPowerMate[] all2 = USBPowerMate.GetAll();
		IPowerMateDevice[] array = new IPowerMateDevice[all.Length + all2.Length];
		all.CopyTo(array, 0);
		all2.CopyTo(array, all.Length);
		return array;
	}

	private static void ShowThreadExceptionDialog(Exception e)
	{
		ThreadExceptionDialog threadExceptionDialog = new ThreadExceptionDialog(e);
		threadExceptionDialog.Text = Application.ProductName + " 2.0.1 (build 61)";
		DialogResult dialogResult = threadExceptionDialog.ShowDialog();
		threadExceptionDialog.Dispose();
		if (dialogResult != DialogResult.Cancel)
		{
			Application.Exit();
		}
	}

	private static void Application_ApplicationExit(object sender, EventArgs e)
	{
		DeviceChangeNotifier.DestroyHandle();
		SystemEvents.PowerModeChanged -= PowerModeChanged;
		PMDoc.Close();
		foreach (IPowerMateUIPlugin pMUIPlugin in PMUIPlugins)
		{
			if (pMUIPlugin.Status == UIStatus.Open)
			{
				pMUIPlugin.Close();
			}
			if (pMUIPlugin is IDisposable)
			{
				((IDisposable)pMUIPlugin).Dispose();
			}
		}
		foreach (IPMActionPlugin pMActionPlugin in PMActionPlugins)
		{
			if (pMActionPlugin is IDisposable)
			{
				(pMActionPlugin as IDisposable).Dispose();
			}
		}
		IPowerMateDevice[] powerMates = PowerMates;
		foreach (IPowerMateDevice powerMateDevice in powerMates)
		{
			if (powerMateDevice is IDisposable)
			{
				((IDisposable)powerMateDevice).Dispose();
			}
		}
		PMDoc.Save();
	}

	private static void DeviceChangeNotifier_DeviceAdded(object sender, EventArgs e)
	{
		IPowerMateDevice[] powerMateDevices = PowerMateDevices;
		foreach (IPowerMateDevice powerMateDevice in powerMateDevices)
		{
			if (powerMateDevice is IDisposable)
			{
				((IDisposable)powerMateDevice).Dispose();
			}
		}
		PowerMateDevices = FindPowerMates();
		PMDoc.AssignPowerMates(PowerMateDevices, DeviceAssignment.Default);
	}

	private static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
	{
		if (e.Mode != PowerModes.Resume)
		{
			return;
		}
		Thread.Sleep(1000);
		foreach (DeviceNode item in PMDoc)
		{
			item.SetDeviceLED();
		}
	}
}
