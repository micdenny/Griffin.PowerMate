using System;
using System.Drawing;
using System.Reflection;

namespace Griffin.PowerMate.App;

public abstract class PMActionPluginBase : IPMActionPlugin, IDisposable
{
	private Icon _Icon;

	protected bool IsDisposed;

	public abstract string Name { get; }

	public abstract string Description { get; }

	public Icon Icon => _Icon;

	public abstract string Author { get; }

	public virtual string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

	public abstract IComputerAction[] AvailableActions { get; }

	public event EventHandler Disposed;

	public PMActionPluginBase(Icon icon)
	{
		_Icon = icon;
	}

	public void Dispose()
	{
		if (IsDisposed)
		{
			return;
		}
		IsDisposed = true;
		IComputerAction[] availableActions = AvailableActions;
		foreach (IComputerAction computerAction in availableActions)
		{
			if (computerAction is IDisposable)
			{
				((IDisposable)computerAction).Dispose();
			}
		}
		_Icon.Dispose();
		OnDisposed(EventArgs.Empty);
	}

	protected virtual void OnDisposed(EventArgs e)
	{
		if (this.Disposed != null)
		{
			this.Disposed(this, e);
		}
	}
}
