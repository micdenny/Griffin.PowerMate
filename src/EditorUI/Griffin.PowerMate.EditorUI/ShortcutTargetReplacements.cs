using System.Collections.Generic;

namespace Griffin.PowerMate.EditorUI;

internal class ShortcutTargetReplacements : Dictionary<string, string>
{
	public ShortcutTargetReplacements()
	{
		Add("iTunesIco", "iTunes");
	}
}
