using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Griffin.PowerMate.App;

internal class CustomNotifyIcon : Component
{
	private enum NotifyIconFlags : uint
	{
		None = 0u,
		Message = 1u,
		Icon = 2u,
		Tip = 4u,
		State = 8u,
		Info = 0x10u,
		Guid = 0x20u,
		RealTime = 0x40u,
		ShowTip = 0x80u
	}

	private enum NotifyIconState : uint
	{
		Hidden = 1u,
		SharedIcon
	}

	private enum NotifyIconInfoFlags : uint
	{
		None = 0u,
		Info = 1u,
		Warning = 2u,
		Error = 3u,
		User = 4u,
		IconMask = 15u,
		NoSound = 16u,
		LargeIcon = 32u
	}

	private enum NotifyIconMessage : uint
	{
		Add,
		Modify,
		Delete,
		SetFocus,
		SetVersion
	}

	private struct NotifyIconData
	{
		public int cbSize;

		public IntPtr hWnd;

		public uint uID;

		public NotifyIconFlags uFlags;

		public uint uCallbackMessage;

		public IntPtr hIcon;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string szTip;

		public NotifyIconState dwState;

		public NotifyIconState dwStateMask;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string szInfo;

		public uint uTimeout_uVersion;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string szInfoTitle;

		public NotifyIconInfoFlags dwInfoFlags;

		public Guid guidItem;

		public IntPtr hBalloonIcon;
	}

	private class NotifyIconMessageWindow : NativeWindow
	{
		private List<IMessageFilter> MessageFilterList = new List<IMessageFilter>();

		public void AddMessageFilter(IMessageFilter filter)
		{
			if (!MessageFilterList.Contains(filter))
			{
				MessageFilterList.Add(filter);
			}
		}

		public void RemoveMessageFilter(IMessageFilter filter)
		{
			MessageFilterList.Remove(filter);
		}

		protected override void WndProc(ref Message m)
		{
			foreach (IMessageFilter messageFilter in MessageFilterList)
			{
				if (messageFilter.PreFilterMessage(ref m))
				{
					return;
				}
			}
			base.WndProc(ref m);
		}
	}

	private class EventMessageFilter : IMessageFilter
	{
		private enum MouseButtonState
		{
			Up,
			Down,
			DoubleDown
		}

		private const uint WM_MOUSEMOVE = 512u;

		private const uint WM_LBUTTONDOWN = 513u;

		private const uint WM_LBUTTONUP = 514u;

		private const uint WM_LBUTTONDBLCLK = 515u;

		private const uint WM_RBUTTONDOWN = 516u;

		private const uint WM_RBUTTONUP = 517u;

		private const uint WM_RBUTTONDBLCLK = 518u;

		private const uint WM_MBUTTONDOWN = 519u;

		private const uint WM_MBUTTONUP = 520u;

		private const uint WM_MBUTTONDBLCLK = 521u;

		private MouseButtonState LeftButtonState;

		private MouseButtonState MiddleButtonState;

		private MouseButtonState RightButtonState;

		public event EventHandler Click;

		public event EventHandler DoubleClick;

		public event MouseEventHandler MouseClick;

		public event MouseEventHandler MouseDoubleClick;

		public event MouseEventHandler MouseDown;

		public event MouseEventHandler MouseMove;

		public event MouseEventHandler MouseUp;

		public bool PreFilterMessage(ref Message msg)
		{
			if ((long)msg.Msg == 32769)
			{
				switch ((uint)(int)msg.LParam)
				{
				case 512u:
				{
					MouseEventArgs e = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);
					OnMouseMove(e);
					break;
				}
				case 513u:
					ButtonDown(MouseButtons.Left);
					break;
				case 516u:
					ButtonDown(MouseButtons.Right);
					break;
				case 519u:
					ButtonDown(MouseButtons.Middle);
					break;
				case 514u:
					ButtonUp(MouseButtons.Left);
					break;
				case 517u:
					ButtonUp(MouseButtons.Right);
					break;
				case 520u:
					ButtonUp(MouseButtons.Middle);
					break;
				case 515u:
					ButtonDoubleClick(MouseButtons.Left);
					break;
				case 518u:
					ButtonDoubleClick(MouseButtons.Right);
					break;
				case 521u:
					ButtonDoubleClick(MouseButtons.Middle);
					break;
				}
			}
			return false;
		}

		protected void ButtonDown(MouseButtons button)
		{
			MouseEventArgs e = new MouseEventArgs(button, 0, 0, 0, 0);
			OnMouseDown(e);
			SetMouseButtonState(button, MouseButtonState.Down);
		}

		protected void ButtonUp(MouseButtons button)
		{
			MouseEventArgs e = new MouseEventArgs(button, 0, 0, 0, 0);
			OnMouseUp(e);
			if (GetMouseButtonState(button) != MouseButtonState.DoubleDown)
			{
				OnClick(e);
				OnMouseClick(e);
			}
			SetMouseButtonState(button, MouseButtonState.Up);
		}

		protected void ButtonDoubleClick(MouseButtons button)
		{
			MouseEventArgs e = new MouseEventArgs(button, 0, 0, 0, 0);
			OnDoubleClick(e);
			OnMouseDoubleClick(e);
			OnMouseDown(e);
			SetMouseButtonState(button, MouseButtonState.DoubleDown);
		}

		private MouseButtonState GetMouseButtonState(MouseButtons button)
		{
			MouseButtonState result = MouseButtonState.Up;
			switch (button)
			{
			case MouseButtons.Left:
				result = LeftButtonState;
				break;
			case MouseButtons.Middle:
				result = MiddleButtonState;
				break;
			case MouseButtons.Right:
				result = RightButtonState;
				break;
			}
			return result;
		}

		private void SetMouseButtonState(MouseButtons button, MouseButtonState state)
		{
			switch (button)
			{
			case MouseButtons.Left:
				LeftButtonState = state;
				break;
			case MouseButtons.Middle:
				MiddleButtonState = state;
				break;
			case MouseButtons.Right:
				RightButtonState = state;
				break;
			}
		}

		protected virtual void OnClick(EventArgs e)
		{
			if (this.Click != null)
			{
				this.Click(this, e);
			}
		}

		protected virtual void OnDoubleClick(EventArgs e)
		{
			if (this.DoubleClick != null)
			{
				this.DoubleClick(this, e);
			}
		}

		protected virtual void OnMouseClick(MouseEventArgs e)
		{
			if (this.MouseClick != null)
			{
				this.MouseClick(this, e);
			}
		}

		protected virtual void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (this.MouseDoubleClick != null)
			{
				this.MouseDoubleClick(this, e);
			}
		}

		protected virtual void OnMouseDown(MouseEventArgs e)
		{
			if (this.MouseDown != null)
			{
				this.MouseDown(this, e);
			}
		}

		protected virtual void OnMouseMove(MouseEventArgs e)
		{
			if (this.MouseMove != null)
			{
				this.MouseMove(this, e);
			}
		}

		protected virtual void OnMouseUp(MouseEventArgs e)
		{
			if (this.MouseUp != null)
			{
				this.MouseUp(this, e);
			}
		}
	}

	private const uint WM_NULL = 0u;

	private const uint WM_APP = 32768u;

	private const uint UWM_CALLBACK = 32769u;

	private NotifyIconData IconData = default(NotifyIconData);

	private NotifyIconMessageWindow MessageWindow = new NotifyIconMessageWindow();

	private EventMessageFilter EventFilter = new EventMessageFilter();

	private ContextMenuStrip _ContextMenuStrip;

	private Icon _Icon;

	private bool _Visible;

	private bool IsDisposed;

	public Icon Icon
	{
		get
		{
			return _Icon;
		}
		set
		{
			if (value != null)
			{
				IconData.hIcon = value.Handle;
				IconData.uFlags |= NotifyIconFlags.Icon;
			}
			else
			{
				IconData.hIcon = IntPtr.Zero;
				IconData.uFlags &= (NotifyIconFlags)4294967293u;
			}
			_Icon = value;
			if (!IsDisposed)
			{
				Shell_NotifyIcon(NotifyIconMessage.Modify, ref IconData);
			}
		}
	}

	public string Text
	{
		get
		{
			return IconData.szTip;
		}
		set
		{
			if (value != null)
			{
				if (value.Length > 127)
				{
					IconData.szTip = value.Substring(0, 127);
				}
				else
				{
					IconData.szTip = value;
				}
				IconData.uFlags |= NotifyIconFlags.ShowTip;
			}
			else
			{
				IconData.szTip = null;
				IconData.uFlags &= (NotifyIconFlags)4294967167u;
			}
			if (!IsDisposed)
			{
				Shell_NotifyIcon(NotifyIconMessage.Modify, ref IconData);
			}
		}
	}

	public bool Visible
	{
		get
		{
			return _Visible;
		}
		set
		{
			if (_Visible != value && !IsDisposed)
			{
				if (value)
				{
					IconData.dwState &= (NotifyIconState)4294967294u;
				}
				else
				{
					IconData.dwState |= NotifyIconState.Hidden;
				}
				if (Shell_NotifyIcon(NotifyIconMessage.Modify, ref IconData))
				{
					_Visible = value;
				}
			}
		}
	}

	public ContextMenuStrip ContextMenuStrip
	{
		get
		{
			return _ContextMenuStrip;
		}
		set
		{
			_ContextMenuStrip = value;
		}
	}

	public event EventHandler Click;

	public event EventHandler DoubleClick;

	public event MouseEventHandler MouseClick;

	public event MouseEventHandler MouseDoubleClick;

	public event MouseEventHandler MouseDown;

	public event MouseEventHandler MouseMove;

	public event MouseEventHandler MouseUp;

	[DllImport("shell32.dll")]
	private static extern bool Shell_NotifyIcon(NotifyIconMessage cmd, ref NotifyIconData data);

	[DllImport("user32.dll")]
	private static extern bool SetForegroundWindow(IntPtr hWnd);

	[DllImport("user32.dll")]
	private static extern bool PostMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

	public CustomNotifyIcon()
	{
		InitializeMessageWindow();
		IconData.cbSize = Marshal.SizeOf((object)IconData);
		IconData.hWnd = MessageWindow.Handle;
		IconData.uFlags = (NotifyIconFlags)9u;
		IconData.uCallbackMessage = 32769u;
		IconData.dwState = NotifyIconState.Hidden;
		IconData.dwStateMask = NotifyIconState.Hidden;
		Shell_NotifyIcon(NotifyIconMessage.Add, ref IconData);
	}

	public CustomNotifyIcon(IContainer container)
		: this()
	{
		container.Add(this);
	}

	public void InitializeMessageWindow()
	{
		EventFilter.Click += delegate(object sender, EventArgs e)
		{
			OnClick(e);
		};
		EventFilter.DoubleClick += delegate(object sender, EventArgs e)
		{
			OnDoubleClick(e);
		};
		EventFilter.MouseClick += delegate(object sender, MouseEventArgs e)
		{
			OnMouseClick(e);
		};
		EventFilter.MouseDoubleClick += delegate(object sender, MouseEventArgs e)
		{
			OnMouseDoubleClick(e);
		};
		EventFilter.MouseDown += delegate(object sender, MouseEventArgs e)
		{
			OnMouseDown(e);
		};
		EventFilter.MouseMove += delegate(object sender, MouseEventArgs e)
		{
			OnMouseMove(e);
		};
		EventFilter.MouseUp += delegate(object sender, MouseEventArgs e)
		{
			OnMouseUp(e);
		};
		MessageWindow.AddMessageFilter(EventFilter);
		MessageWindow.CreateHandle(new CreateParams());
	}

	public void AddMessageFilter(IMessageFilter filter)
	{
		MessageWindow.AddMessageFilter(filter);
	}

	public void RemoveMessageFilter(IMessageFilter filter)
	{
		MessageWindow.RemoveMessageFilter(filter);
	}

	public new void Dispose()
	{
		Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		if (!IsDisposed)
		{
			IsDisposed = true;
			Shell_NotifyIcon(NotifyIconMessage.Delete, ref IconData);
			MessageWindow.DestroyHandle();
			base.Dispose(disposing);
		}
	}

	protected virtual void OnClick(EventArgs e)
	{
		if (this.Click != null)
		{
			this.Click(this, e);
		}
	}

	protected virtual void OnDoubleClick(EventArgs e)
	{
		if (this.DoubleClick != null)
		{
			this.DoubleClick(this, e);
		}
	}

	protected virtual void OnMouseClick(MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			ShowContextMenuStrip();
		}
		if (this.MouseClick != null)
		{
			this.MouseClick(this, e);
		}
	}

	protected virtual void OnMouseDoubleClick(MouseEventArgs e)
	{
		if (this.MouseDoubleClick != null)
		{
			this.MouseDoubleClick(this, e);
		}
	}

	protected virtual void OnMouseDown(MouseEventArgs e)
	{
		if (this.MouseDown != null)
		{
			this.MouseDown(this, e);
		}
	}

	protected virtual void OnMouseMove(MouseEventArgs e)
	{
		if (this.MouseMove != null)
		{
			this.MouseMove(this, e);
		}
	}

	protected virtual void OnMouseUp(MouseEventArgs e)
	{
		if (this.MouseUp != null)
		{
			this.MouseUp(this, e);
		}
	}

	protected void ShowContextMenuStrip()
	{
		if (ContextMenuStrip != null && !IsDisposed)
		{
			Point position = Cursor.Position;
			SetForegroundWindow(MessageWindow.Handle);
			typeof(ContextMenuStrip).GetMethod("ShowInTaskbar", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(ContextMenuStrip, new object[2] { position.X, position.Y });
			PostMessage(MessageWindow.Handle, 0u, 0u, 0u);
		}
	}
}
