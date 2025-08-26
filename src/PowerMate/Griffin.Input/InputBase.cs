using System;
using System.Runtime.InteropServices;

namespace Griffin.Input;

internal static class InputBase
{
	private static readonly int SizeofINPUT = (Is64Bit ? Marshal.SizeOf(typeof(INPUT_64)) : Marshal.SizeOf(typeof(INPUT_32)));

	private static bool Is64Bit => IntPtr.Size == 8;

	[DllImport("user32.dll", SetLastError = true)]
	public static extern uint SendInput(uint nInputs, INPUT_32[] pInputs, int cbSize);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern uint SendInput(uint nInputs, INPUT_64[] pInputs, int cbSize);

	public static uint SendInput(INPUT_32[] inputs)
	{
		uint num = 0u;
		return SendInput((uint)inputs.Length, inputs, SizeofINPUT);
	}

	public static uint SendInput(INPUT_64[] inputs)
	{
		uint num = 0u;
		return SendInput((uint)inputs.Length, inputs, SizeofINPUT);
	}

	public static uint SendInput(params MOUSEINPUT[] mouseInput)
	{
		uint num = 0u;
		if (Is64Bit)
		{
			INPUT_64[] array = new INPUT_64[mouseInput.Length];
			for (int i = 0; i < mouseInput.Length; i++)
			{
				INPUT_64 iNPUT_ = new INPUT_64
				{
					type = InputType.Mouse,
					mi = mouseInput[i]
				};
				array[i] = iNPUT_;
			}
			return SendInput(array);
		}
		INPUT_32[] array2 = new INPUT_32[mouseInput.Length];
		for (int j = 0; j < mouseInput.Length; j++)
		{
			INPUT_32 iNPUT_2 = new INPUT_32
			{
				type = InputType.Mouse,
				mi = mouseInput[j]
			};
			array2[j] = iNPUT_2;
		}
		return SendInput(array2);
	}

	public static uint SendInput(params KEYBDINPUT[] keybdInput)
	{
		uint num = 0u;
		if (Is64Bit)
		{
			INPUT_64[] array = new INPUT_64[keybdInput.Length];
			for (int i = 0; i < keybdInput.Length; i++)
			{
				INPUT_64 iNPUT_ = new INPUT_64
				{
					type = InputType.Keyboard,
					ki = keybdInput[i]
				};
				array[i] = iNPUT_;
			}
			return SendInput(array);
		}
		INPUT_32[] array2 = new INPUT_32[keybdInput.Length];
		for (int j = 0; j < keybdInput.Length; j++)
		{
			INPUT_32 iNPUT_2 = new INPUT_32
			{
				type = InputType.Keyboard,
				ki = keybdInput[j]
			};
			array2[j] = iNPUT_2;
		}
		return SendInput(array2);
	}

	public static uint SendInput(params HARDWAREINPUT[] hardwareInput)
	{
		uint num = 0u;
		if (Is64Bit)
		{
			INPUT_64[] array = new INPUT_64[hardwareInput.Length];
			for (int i = 0; i < hardwareInput.Length; i++)
			{
				INPUT_64 iNPUT_ = new INPUT_64
				{
					type = InputType.Hardware,
					hi = hardwareInput[i]
				};
				array[i] = iNPUT_;
			}
			return SendInput(array);
		}
		INPUT_32[] array2 = new INPUT_32[hardwareInput.Length];
		for (int j = 0; j < hardwareInput.Length; j++)
		{
			INPUT_32 iNPUT_2 = new INPUT_32
			{
				type = InputType.Hardware,
				hi = hardwareInput[j]
			};
			array2[j] = iNPUT_2;
		}
		return SendInput(array2);
	}
}
