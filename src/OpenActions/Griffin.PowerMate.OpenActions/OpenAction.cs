using System.Diagnostics;
using System.Windows.Forms;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.OpenActions.Properties;

namespace Griffin.PowerMate.OpenActions;

internal class OpenAction : ComputerActionBase
{
	private OpenPanel ActionPanel = new OpenPanel();

	public override string Name => "Open File";

	public override string Description => "Opens an Application, File, or Web Address";

	public override Panel Panel => ActionPanel;

	public OpenAction(string pluginName)
		: base(pluginName, Resources.openFile)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		if (settings != null && settings.Length > 0 && settings[0] != null)
		{
			string text = settings[0].Trim();
			int num = -1;
			if (text.StartsWith("\""))
			{
				num = text.IndexOf("\" ", 1);
				if (num > 0)
				{
					num++;
				}
			}
			else
			{
				num = text.IndexOf(' ');
			}
			if (num > 0 && ShellExecuteFile(text.Substring(0, num), text.Substring(num + 1, text.Length - (num + 1))))
			{
				return true;
			}
			return ShellExecuteFile(text, null);
		}
		return false;
	}

	protected static bool ShellExecuteFile(string filename, string arguments)
	{
		bool result = false;
		Process process = new Process();
		try
		{
			process.StartInfo.FileName = filename;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.UseShellExecute = true;
			result = process.Start();
		}
		catch
		{
			result = false;
		}
		finally
		{
			process.Dispose();
		}
		return result;
	}
}
