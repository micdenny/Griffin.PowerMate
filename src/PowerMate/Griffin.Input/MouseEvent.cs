namespace Griffin.Input;

public class MouseEvent
{
	public const int WHEEL_DELTA = 120;

	public const int XBUTTON1 = 1;

	public const int XBUTTON2 = 2;

	private MOUSEINPUT _MouseInput;

	public MouseEventType Type
	{
		get
		{
			return _MouseInput.dwFlags;
		}
		set
		{
			_MouseInput.dwFlags = value;
		}
	}

	public int X
	{
		get
		{
			return _MouseInput.dx;
		}
		set
		{
			_MouseInput.dx = value;
		}
	}

	public int Y
	{
		get
		{
			return _MouseInput.dy;
		}
		set
		{
			_MouseInput.dy = value;
		}
	}

	public int Data
	{
		get
		{
			return _MouseInput.mouseData;
		}
		set
		{
			_MouseInput.mouseData = value;
		}
	}

	internal MOUSEINPUT MouseInput => _MouseInput;

	public MouseEvent(MouseEventType type)
	{
		_MouseInput = default(MOUSEINPUT);
		_MouseInput.dwFlags = type;
	}

	public MouseEvent(MouseEventType type, int x, int y, int data)
		: this(type)
	{
		_MouseInput.dx = x;
		_MouseInput.dy = y;
		_MouseInput.mouseData = data;
	}
}
