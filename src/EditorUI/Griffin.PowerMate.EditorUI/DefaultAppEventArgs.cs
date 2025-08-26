using System;
using Griffin.PowerMate.App;

namespace Griffin.PowerMate.EditorUI;

internal class DefaultAppEventArgs : EventArgs
{
	private AppNode[] _AppNodes;

	public AppNode[] AppNodes => _AppNodes;

	public DefaultAppEventArgs(AppNode[] appNodes)
	{
		_AppNodes = appNodes;
	}
}
