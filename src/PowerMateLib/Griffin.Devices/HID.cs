using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Griffin.Devices;

internal class HID : Device
{
	private enum HIDP_STATUS : uint
	{
		SUCCESS = 1114112u,
		INVALID_PREPARSED_DATA = 3222339585u
	}

	public enum HIDP_REPORT_TYPE : uint
	{
		Input,
		Output,
		Feature
	}

	public struct HIDD_ATTRIBUTES
	{
		public uint Size;

		public ushort VendorID;

		public ushort ProductID;

		public ushort VersionNumber;
	}

	public struct HIDP_CAPS
	{
		public ushort Usage;

		public ushort UsagePage;

		public ushort InputReportByteLength;

		public ushort OutputReportByteLength;

		public ushort FeatureReportByteLength;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
		public ushort[] Reserved;

		public ushort NumberLinkCollectionNodes;

		public ushort NumberInputButtonCaps;

		public ushort NumberInputValueCaps;

		public ushort NumberInputDataIndices;

		public ushort NumberOutputButtonCaps;

		public ushort NumberOutputValueCaps;

		public ushort NumberOutputDataIndices;

		public ushort NumberFeatureButtonCaps;

		public ushort NumberFeatureValueCaps;

		public ushort NumberFeatureDataIndices;
	}

	public struct Range
	{
		public ushort UsageMin;

		public ushort UsageMax;

		public ushort StringMin;

		public ushort StringMax;

		public ushort DesignatorMin;

		public ushort DesignatorMax;

		public ushort DataIndexMin;

		public ushort DataIndexMax;
	}

	public struct NotRange
	{
		public ushort Usage;

		public ushort Reserved1;

		public ushort StringIndex;

		public ushort Reserved2;

		public ushort DesignatorIndex;

		public ushort Reserved3;

		public ushort DataIndex;

		public ushort Reserved4;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct HIDP_VALUE_CAPS
	{
		[FieldOffset(0)]
		public ushort UsagePage;

		[FieldOffset(2)]
		public byte ReportID;

		[FieldOffset(3)]
		public bool IsAlias;

		[FieldOffset(4)]
		public ushort BitField;

		[FieldOffset(6)]
		public ushort LinkCollection;

		[FieldOffset(8)]
		public ushort LinkUsage;

		[FieldOffset(10)]
		public ushort LinkUsagePage;

		[FieldOffset(12)]
		public bool IsRange;

		[FieldOffset(13)]
		public bool IsStringRange;

		[FieldOffset(14)]
		public bool IsDesignatorRange;

		[FieldOffset(15)]
		public bool IsAbsolute;

		[FieldOffset(16)]
		public bool HasNull;

		[FieldOffset(17)]
		public byte Reserved;

		[FieldOffset(18)]
		public ushort BitSize;

		[FieldOffset(20)]
		public ushort ReportCount;

		[FieldOffset(22)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public ushort[] Reserved2;

		[FieldOffset(32)]
		public uint UnitsExp;

		[FieldOffset(36)]
		public uint Units;

		[FieldOffset(40)]
		public int LogicalMin;

		[FieldOffset(44)]
		public int LogicalMax;

		[FieldOffset(48)]
		public int PhysicalMin;

		[FieldOffset(52)]
		public int PhysicalMax;

		[FieldOffset(56)]
		public Range Range;

		[FieldOffset(56)]
		public NotRange NotRange;
	}

	private const uint USB_STRING_MAX_BYTES = 128u;

	private const uint FACILITY_HID_ERROR_CODE = 17u;

	private IntPtr _PreparsedData = IntPtr.Zero;

	private HIDP_CAPS _Capabilities;

	protected IntPtr PreparsedData => _PreparsedData;

	public string ManufacturerString
	{
		get
		{
			IntPtr intPtr = Marshal.AllocHGlobal(128);
			if (HidD_GetManufacturerString(base.Handle, intPtr, 128u))
			{
				string result = Marshal.PtrToStringUni(intPtr);
				Marshal.FreeHGlobal(intPtr);
				return result;
			}
			Marshal.FreeHGlobal(intPtr);
			return null;
		}
	}

	public string PhysicalDescriptor
	{
		get
		{
			IntPtr intPtr = Marshal.AllocHGlobal(128);
			if (HidD_GetPhysicalDescriptor(base.Handle, intPtr, 128u))
			{
				string result = Marshal.PtrToStringUni(intPtr);
				Marshal.FreeHGlobal(intPtr);
				return result;
			}
			Marshal.FreeHGlobal(intPtr);
			return null;
		}
	}

	public string ProductString
	{
		get
		{
			IntPtr intPtr = Marshal.AllocHGlobal(128);
			if (HidD_GetProductString(base.Handle, intPtr, 128u))
			{
				string result = Marshal.PtrToStringUni(intPtr);
				Marshal.FreeHGlobal(intPtr);
				return result;
			}
			Marshal.FreeHGlobal(intPtr);
			return null;
		}
	}

	public string SerialNumberString
	{
		get
		{
			IntPtr intPtr = Marshal.AllocHGlobal(128);
			if (HidD_GetSerialNumberString(base.Handle, intPtr, 128u))
			{
				string result = Marshal.PtrToStringUni(intPtr);
				Marshal.FreeHGlobal(intPtr);
				return result;
			}
			Marshal.FreeHGlobal(intPtr);
			return null;
		}
	}

	public HIDD_ATTRIBUTES Attributes
	{
		get
		{
			HIDD_ATTRIBUTES attributes = default(HIDD_ATTRIBUTES);
			attributes.Size = (uint)Marshal.SizeOf((object)attributes);
			if (HidD_GetAttributes(base.Handle, out attributes))
			{
				return attributes;
			}
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
	}

	public HIDP_CAPS Capabilities => _Capabilities;

	public byte[] Feature
	{
		get
		{
			byte[] array = new byte[Capabilities.FeatureReportByteLength];
			if (HidD_GetFeature(base.Handle, array, (uint)array.Length))
			{
				return array;
			}
			return null;
		}
		set
		{
			if (base.IsOpen && !HidD_SetFeature(base.Handle, value, (uint)value.Length))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}
	}

	public byte[] Report
	{
		get
		{
			byte[] array = new byte[Capabilities.InputReportByteLength];
			if (HidD_GetInputReport(base.Handle, array, (uint)array.Length))
			{
				return array;
			}
			return null;
		}
		set
		{
			if (base.IsOpen && !HidD_SetOutputReport(base.Handle, value, (uint)value.Length))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}
	}

	public static Guid Guid
	{
		get
		{
			Guid hidGuid = Guid.Empty;
			HidD_GetHidGuid(out hidGuid);
			return hidGuid;
		}
	}

	[DllImport("hid.dll", SetLastError = true)]
	private static extern void HidD_GetHidGuid(out Guid hidGuid);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetAttributes(IntPtr hidHandle, out HIDD_ATTRIBUTES attributes);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetPreparsedData(IntPtr hidHandle, out IntPtr preparsedData);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern HIDP_STATUS HidP_GetCaps(IntPtr preparsedData, out HIDP_CAPS capabilities);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern HIDP_STATUS HidP_GetValueCaps(HIDP_REPORT_TYPE reportType, out HIDP_VALUE_CAPS[] valueCaps, out uint valueCapsLength, IntPtr preparsedData);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_FreePreparsedData(IntPtr preparsedData);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_SetFeature(IntPtr hidHandle, byte[] reportBuffer, uint reportBufferLength);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetFeature(IntPtr hidHandle, [MarshalAs(UnmanagedType.LPArray)] byte[] reportBuffer, uint reportBufferLength);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_SetOutputReport(IntPtr hidHandle, byte[] reportBuffer, uint reportBufferLength);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetInputReport(IntPtr hidHandle, [MarshalAs(UnmanagedType.LPArray)] byte[] reportBuffer, uint reportBufferLength);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_FlushQueue(IntPtr hidHandle);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetIndexedString(IntPtr hidHandle, uint stringIndex, IntPtr buffer, uint bufferLength);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetManufacturerString(IntPtr hidHandle, IntPtr buffer, uint bufferLength);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetPhysicalDescriptor(IntPtr hidHandle, IntPtr buffer, uint bufferLength);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetProductString(IntPtr hidHandle, IntPtr buffer, uint bufferLength);

	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetSerialNumberString(IntPtr hidHandle, IntPtr buffer, uint bufferLength);

	public HID(string devicePath, FileAccess access)
		: base(devicePath, access)
	{
	}

	public HIDP_VALUE_CAPS[] GetValueCapabilites(HIDP_REPORT_TYPE reportType)
	{
		uint valueCapsLength = 0u;
		switch (reportType)
		{
		case HIDP_REPORT_TYPE.Feature:
			valueCapsLength = Capabilities.NumberFeatureValueCaps;
			break;
		case HIDP_REPORT_TYPE.Input:
			valueCapsLength = Capabilities.NumberInputValueCaps;
			break;
		case HIDP_REPORT_TYPE.Output:
			valueCapsLength = Capabilities.NumberOutputValueCaps;
			break;
		}
		HIDP_VALUE_CAPS[] valueCaps = new HIDP_VALUE_CAPS[valueCapsLength];
		if (HidP_GetValueCaps(reportType, out valueCaps, out valueCapsLength, PreparsedData) == HIDP_STATUS.SUCCESS)
		{
			return valueCaps;
		}
		throw new Win32Exception(Marshal.GetLastWin32Error());
	}

	public byte[] Read()
	{
		byte[] array = new byte[Capabilities.InputReportByteLength];
		Read(array, 0, array.Length);
		return array;
	}

	public bool AsyncRead()
	{
		byte[] array = new byte[Capabilities.InputReportByteLength];
		return AsyncRead(array, 0, array.Length);
	}

	public void Write(ref byte[] report)
	{
		byte[] array = null;
		if (report.Length < Capabilities.OutputReportByteLength)
		{
			array = new byte[Capabilities.OutputReportByteLength];
			report.CopyTo(array, 0);
		}
		else
		{
			array = report;
		}
		Write(ref array, 0, Capabilities.OutputReportByteLength);
	}

	public bool AsyncWrite(byte[] report)
	{
		byte[] array = null;
		if (report.Length < Capabilities.OutputReportByteLength)
		{
			array = new byte[Capabilities.OutputReportByteLength];
			Array.Copy(report, array, array.Length);
		}
		else
		{
			array = report;
		}
		return AsyncWrite(array, 0, Capabilities.OutputReportByteLength);
	}

	public new static string[] Find()
	{
		return Device.Find(Guid);
	}

	public new static string[] Find(string pnpId)
	{
		return Device.Find(Guid, pnpId);
	}

	public static HID[] GetAll(ushort vendorID, ushort productID, FileAccess access)
	{
		List<HID> list = new List<HID>();
		string[] array = Find();
		string[] array2 = array;
		foreach (string devicePath in array2)
		{
			try
			{
				HID hID = new HID(devicePath, access);
				HIDD_ATTRIBUTES attributes = hID.Attributes;
				if (attributes.VendorID == vendorID && attributes.ProductID == productID)
				{
					list.Add(hID);
				}
				else
				{
					hID.Dispose();
				}
			}
			catch
			{
			}
		}
		return list.ToArray();
	}

	public static HID[] GetAll(ushort vendorID, ushort productID, ushort versionNumber, FileAccess access)
	{
		List<HID> list = new List<HID>();
		string[] array = Find();
		string[] array2 = array;
		foreach (string devicePath in array2)
		{
			try
			{
				HID hID = new HID(devicePath, access);
				HIDD_ATTRIBUTES attributes = hID.Attributes;
				if (attributes.VendorID == vendorID && attributes.ProductID == productID && attributes.VersionNumber == versionNumber)
				{
					list.Add(hID);
				}
				else
				{
					hID.Dispose();
				}
			}
			catch
			{
			}
		}
		return list.ToArray();
	}

	protected override void BytesWritten(IAsyncResult writeResult)
	{
		try
		{
			base.DeviceStream.Flush();
		}
		finally
		{
			base.BytesWritten(writeResult);
		}
	}

	protected override void OnOpened(EventArgs e)
	{
		if (!HidD_GetPreparsedData(base.Handle, out _PreparsedData))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		_Capabilities = default(HIDP_CAPS);
		if (HidP_GetCaps(PreparsedData, out _Capabilities) != HIDP_STATUS.SUCCESS)
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		base.OnOpened(e);
	}

	protected override void OnClosed(EventArgs e)
	{
		HidD_FreePreparsedData(PreparsedData);
		_PreparsedData = IntPtr.Zero;
		base.OnClosed(e);
	}
}
