namespace Griffin.Input;

public static class Mouse
{
	public static bool SendMouseEvent(MouseEventType type, int x, int y, int data)
	{
		if (InputBase.SendInput(new MouseEvent(type, x, y, data).MouseInput) != 0)
		{
			return true;
		}
		return false;
	}

	public static uint SendMouseEvent(params MouseEvent[] mouseEvents)
	{
		MOUSEINPUT[] array = new MOUSEINPUT[mouseEvents.Length];
		for (int i = 0; i < mouseEvents.Length; i++)
		{
			ref MOUSEINPUT reference = ref array[i];
			reference = mouseEvents[i].MouseInput;
		}
		return InputBase.SendInput(array);
	}

	public static bool LeftClick()
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.LeftDown);
		MouseEvent mouseEvent2 = new MouseEvent(MouseEventType.LeftUp);
		if (InputBase.SendInput(mouseEvent.MouseInput, mouseEvent2.MouseInput) >= 2)
		{
			return true;
		}
		return false;
	}

	public static bool MiddleClick()
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.MiddleDown);
		MouseEvent mouseEvent2 = new MouseEvent(MouseEventType.MiddleUp);
		if (InputBase.SendInput(mouseEvent.MouseInput, mouseEvent2.MouseInput) >= 2)
		{
			return true;
		}
		return false;
	}

	public static bool RightClick()
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.RightDown);
		MouseEvent mouseEvent2 = new MouseEvent(MouseEventType.RightUp);
		if (InputBase.SendInput(mouseEvent.MouseInput, mouseEvent2.MouseInput) >= 2)
		{
			return true;
		}
		return false;
	}

	public static bool LeftDoubleClick()
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.LeftDown);
		MouseEvent mouseEvent2 = new MouseEvent(MouseEventType.LeftUp);
		if (InputBase.SendInput(mouseEvent.MouseInput, mouseEvent2.MouseInput, mouseEvent.MouseInput, mouseEvent2.MouseInput) >= 4)
		{
			return true;
		}
		return false;
	}

	public static bool MoveCursor(int x, int y)
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.Move, x, y, 0);
		if (InputBase.SendInput(mouseEvent.MouseInput) != 0)
		{
			return true;
		}
		return false;
	}

	public static bool MoveCursorTo(ushort x, ushort y)
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.MoveAbsolute, x, y, 0);
		if (InputBase.SendInput(mouseEvent.MouseInput) != 0)
		{
			return true;
		}
		return false;
	}

	public static bool RotateWheel(int amount)
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.Wheel, 0, 0, amount);
		if (InputBase.SendInput(mouseEvent.MouseInput) != 0)
		{
			return true;
		}
		return false;
	}

	public static bool RotateWheelHorizontal(int amount)
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.HorzWheel, 0, 0, amount);
		if (InputBase.SendInput(mouseEvent.MouseInput) != 0)
		{
			return true;
		}
		return false;
	}

	public static bool XButton1Click()
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.XButtonDown, 0, 0, 1);
		MouseEvent mouseEvent2 = new MouseEvent(MouseEventType.XButtonUp, 0, 0, 1);
		if (InputBase.SendInput(mouseEvent.MouseInput, mouseEvent2.MouseInput) >= 2)
		{
			return true;
		}
		return false;
	}

	public static bool XButton2Click()
	{
		MouseEvent mouseEvent = new MouseEvent(MouseEventType.XButtonDown, 0, 0, 2);
		MouseEvent mouseEvent2 = new MouseEvent(MouseEventType.XButtonUp, 0, 0, 2);
		if (InputBase.SendInput(mouseEvent.MouseInput, mouseEvent2.MouseInput) >= 2)
		{
			return true;
		}
		return false;
	}
}
