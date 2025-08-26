using System.Windows.Forms;
using Griffin.Input;
using Griffin.PowerMate.App;
using Griffin.PowerMate.Device;
using Griffin.PowerMate.SendKeysActions.Properties;

namespace Griffin.PowerMate.SendKeysActions;

internal class SendKeysComputerActions : ComputerActionBase
{
	private SendKeysPanel ActionPanel = new SendKeysPanel();

	public override string Name => "Send Keys";

	public override string Description => "Simulates Keystrokes";

	public override Panel Panel => ActionPanel;

	public SendKeysComputerActions(string pluginName)
		: base(pluginName, Resources.keyPress)
	{
	}

	public override bool SupportsPMAction(PMAction action)
	{
		return true;
	}

	public override bool Perform(IPowerMateDevice sender, params string[] settings)
	{
		try
		{
			int num = 0;
			while (num < settings[0].Length)
			{
				bool flag = false;
				int num2 = settings[0].IndexOf(", ", num);
				if (num2 < 0)
				{
					num2 = settings[0].Length;
				}
				string text = settings[0].Substring(num, num2 - num);
				if (text.Contains("Win+"))
				{
					text = text.Replace("Win+", "");
					flag = true;
					Keyboard.KeyDown(Keys.LWin);
				}
				Keyboard.KeyDown(ActionPanel.StringToKey(text));
				Keyboard.KeyUp(ActionPanel.StringToKey(text));
				if (flag)
				{
					Keyboard.KeyUp(Keys.LWin);
				}
				num = num2 + 2;
			}
			return true;
		}
		catch
		{
			return false;
		}
	}
}
