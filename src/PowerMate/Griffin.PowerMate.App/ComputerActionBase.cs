using System;
using System.Drawing;
using System.Windows.Forms;
using Griffin.PowerMate.Device;

namespace Griffin.PowerMate.App;

public abstract class ComputerActionBase : IComputerAction, IDisposable
{
	private string _PluginName;

	private Icon _Icon;

	protected bool IsDisposed;

	public abstract string Name { get; }

	public Icon Icon => _Icon;

	public abstract string Description { get; }

	public abstract Panel Panel { get; }

	public string PluginName => _PluginName;

	public event EventHandler Disposed;

	public ComputerActionBase(string pluginName, Icon icon)
	{
		_PluginName = pluginName;
		_Icon = icon;
	}

	public ComputerActionBase(IPMActionPlugin plugin, Icon icon)
		: this(plugin.Name, icon)
	{
	}

	public abstract bool SupportsPMAction(PMAction action);

	public abstract bool Perform(IPowerMateDevice sender, params string[] settings);

	public void Dispose()
	{
		if (!IsDisposed)
		{
			IsDisposed = true;
			_Icon.Dispose();
			OnDisposed(EventArgs.Empty);
		}
	}

	protected virtual void OnDisposed(EventArgs e)
	{
		if (this.Disposed != null)
		{
			this.Disposed(this, e);
		}
	}
}
