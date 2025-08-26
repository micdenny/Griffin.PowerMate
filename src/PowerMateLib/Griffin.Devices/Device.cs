using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Griffin.Devices;

internal class Device : IDisposable
{
	private enum DIGCF : uint
	{
		ALLCLASSES = 4u,
		DEVICEINTERFACE = 16u,
		PRESENT = 2u,
		PROFILE = 8u
	}

	[Flags]
	private enum FileFlags : uint
	{
		Readonly = 1u,
		Hidden = 2u,
		System = 4u,
		Directory = 0x10u,
		Archive = 0x20u,
		Device = 0x40u,
		Normal = 0x80u,
		Temporary = 0x100u,
		SparseFile = 0x200u,
		ReparsePoint = 0x400u,
		Compressed = 0x800u,
		Offline = 0x1000u,
		NotContentIndexed = 0x2000u,
		Encrypted = 0x4000u,
		Write_Through = 0x80000000u,
		Overlapped = 0x40000000u,
		NoBuffering = 0x20000000u,
		RandomAccess = 0x10000000u,
		SequentialScan = 0x8000000u,
		DeleteOnClose = 0x4000000u,
		BackupSemantics = 0x2000000u,
		PosixSemantics = 0x1000000u,
		OpenReparsePoint = 0x200000u,
		OpenNoRecall = 0x100000u,
		FirstPipeInstance = 0x80000u
	}

	[StructLayout(LayoutKind.Explicit)]
	private struct OVERLAPPED
	{
		[FieldOffset(0)]
		public IntPtr Internal;

		[FieldOffset(4)]
		public IntPtr InternalHigh;

		[FieldOffset(8)]
		private uint Offset;

		[FieldOffset(12)]
		private uint OffsetHigh;

		[FieldOffset(8)]
		public IntPtr Pointer;

		[FieldOffset(16)]
		public IntPtr hEvent;
	}

	[StructLayout(LayoutKind.Explicit)]
	private struct OVERLAPPED_64
	{
		[FieldOffset(0)]
		public IntPtr Internal;

		[FieldOffset(8)]
		public IntPtr InternalHigh;

		[FieldOffset(16)]
		private uint Offset;

		[FieldOffset(20)]
		private uint OffsetHigh;

		[FieldOffset(16)]
		public IntPtr Pointer;

		[FieldOffset(24)]
		public IntPtr hEvent;
	}

	private struct SP_DEVINFO_DATA
	{
		public uint cbSize;

		public Guid ClassGuid;

		public uint DevInst;

		public IntPtr Reserved;
	}

	private struct SP_DEVICE_INTERFACE_DATA
	{
		public uint cbSize;

		public Guid InterfaceClassGuid;

		public uint Flags;

		public IntPtr Reserved;
	}

	private struct SP_DEVICE_INTERFACE_DETAIL_DATA
	{
		public uint cbSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string DevicePath;
	}

	private const string DisconnectString = "The device is not connected.\r\n";

	private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

	private IntPtr _Handle;

	private string _DevicePath;

	private SafeFileHandle SafeHandle;

	private FileStream _DeviceStream;

	private AsyncCallback ReadCallback;

	private AsyncCallback WriteCallback;

	protected FileStream DeviceStream => _DeviceStream;

	public IntPtr Handle => _Handle;

	public string DevicePath => _DevicePath;

	public bool IsOpen
	{
		get
		{
			if (SafeHandle != null && !SafeHandle.IsClosed)
			{
				return !SafeHandle.IsInvalid;
			}
			return false;
		}
	}

	public bool CanRead
	{
		get
		{
			if (DeviceStream != null)
			{
				return DeviceStream.CanRead;
			}
			return false;
		}
	}

	public bool CanWrite
	{
		get
		{
			if (DeviceStream != null)
			{
				return DeviceStream.CanWrite;
			}
			return false;
		}
	}

	public event EventHandler<DeviceAsyncEventArgs> AsyncReadCompleted;

	public event EventHandler<DeviceAsyncEventArgs> AsyncWriteCompleted;

	public event EventHandler Detached;

	public event EventHandler Opened;

	public event EventHandler Closed;

	public event EventHandler Disposed;

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
	private static extern bool DeviceIoControl(IntPtr deviceHandle, uint ioControlCode, [MarshalAs(UnmanagedType.LPArray)] byte[] inBuffer, uint inBufferSize, [MarshalAs(UnmanagedType.LPArray)] byte[] outBuffer, uint outBufferSize, out uint bytesReturned, IntPtr overlapped);

	[DllImport("setupapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
	private static extern IntPtr SetupDiGetClassDevs([MarshalAs(UnmanagedType.LPStruct)] Guid classGuid, string enumerator, IntPtr hwndParent, DIGCF flags);

	[DllImport("setupapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
	private static extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData, [MarshalAs(UnmanagedType.LPStruct)] Guid interfaceClassGuid, uint memberIndex, out SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

	[DllImport("setupapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
	private static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

	[DllImport("setupapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
	private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, IntPtr deviceInfoData);

	[DllImport("setupapi.dll", CharSet = CharSet.Ansi, SetLastError = true)]
	private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, IntPtr deviceInfoData);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
	private static extern IntPtr CreateFile(string fileName, FileAccess fileAccess, FileShare fileShare, IntPtr securityAttributes, FileMode creationDisposition, FileFlags flags, IntPtr template);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(int hObject);

	public Device(string devicePath, FileAccess access)
	{
		ReadCallback = BytesRead;
		WriteCallback = BytesWritten;
		Open(devicePath, access);
	}

	public virtual void Open(string devicePath, FileAccess access)
	{
		if (IsOpen)
		{
			Close();
		}
		_DevicePath = devicePath;
		_Handle = CreateFile(devicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, FileFlags.Normal | FileFlags.Overlapped, IntPtr.Zero);
		if (Handle == INVALID_HANDLE_VALUE)
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		SafeHandle = new SafeFileHandle(Handle, ownsHandle: true);
		_DeviceStream = new FileStream(SafeHandle, access, 2, isAsync: true);
		OnOpened(EventArgs.Empty);
	}

	public void Close()
	{
		if (IsOpen)
		{
			DeviceStream.Close();
			DeviceStream.Dispose();
			SafeHandle.Close();
			SafeHandle.Dispose();
			_Handle = IntPtr.Zero;
			OnClosed(EventArgs.Empty);
		}
	}

	public int Read(byte[] array, int offset, int count)
	{
		if (IsOpen && DeviceStream.CanRead)
		{
			try
			{
				return DeviceStream.Read(array, offset, count);
			}
			catch (IOException ex)
			{
				if (!CheckForDisconnection(ex))
				{
					throw ex;
				}
				array = null;
			}
		}
		return 0;
	}

	public bool AsyncRead(byte[] array, int offset, int count)
	{
		if (IsOpen && DeviceStream.CanRead)
		{
			DeviceStream.BeginRead(array, offset, count, ReadCallback, array);
			return true;
		}
		return false;
	}

	public void Write(ref byte[] array, int offset, int count)
	{
		if (!IsOpen || !DeviceStream.CanWrite)
		{
			return;
		}
		try
		{
			DeviceStream.Write(array, offset, count);
		}
		catch (Exception ex)
		{
			if (CheckForDisconnection(ex))
			{
				array = null;
				return;
			}
			throw ex;
		}
	}

	public bool AsyncWrite(byte[] array, int offset, int count)
	{
		if (IsOpen && DeviceStream.CanWrite)
		{
			DeviceStream.BeginWrite(array, offset, count, WriteCallback, array);
			return true;
		}
		return false;
	}

	public void Dispose()
	{
		Close();
		OnDisposed(EventArgs.Empty);
	}

	public uint SendIoControlCode(uint controlCode, byte[] inBuffer, byte[] outBuffer)
	{
		uint bytesReturned = 0u;
		if (IsOpen && !DeviceIoControl(Handle, controlCode, inBuffer, (uint)inBuffer.Length, outBuffer, (uint)outBuffer.Length, out bytesReturned, IntPtr.Zero))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		return bytesReturned;
	}

	public static uint GetIOCTL(uint deviceType, uint function, uint method, uint access)
	{
		return (deviceType << 16) | (access << 14) | (function << 2) | method;
	}

	public static string[] Find()
	{
		return Find(Guid.Empty, null);
	}

	public static string[] Find(Guid classGuid)
	{
		return Find(classGuid, null);
	}

	public static string[] Find(string pnpEnumerator)
	{
		return Find(Guid.Empty, pnpEnumerator);
	}

	public static string[] Find(Guid classGuid, string pnpId)
	{
		return Find(classGuid, null, pnpId);
	}

	public static string[] Find(string pnpEnumerator, string pnpId)
	{
		return Find(Guid.Empty, pnpEnumerator, pnpId);
	}

	public static string[] Find(Guid classGuid, string pnpEnumerator, string pnpId)
	{
		List<string> list = new List<string>();
		DIGCF dIGCF = DIGCF.PRESENT;
		dIGCF = ((!(classGuid == Guid.Empty)) ? (dIGCF | DIGCF.DEVICEINTERFACE) : (dIGCF | DIGCF.ALLCLASSES));
		if (pnpId != null)
		{
			pnpId = pnpId.Replace('\\', '#').ToLower();
		}
		IntPtr intPtr = SetupDiGetClassDevs(classGuid, pnpEnumerator, IntPtr.Zero, dIGCF);
		if (intPtr == INVALID_HANDLE_VALUE)
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		bool flag = true;
		uint num = 0u;
		while (flag)
		{
			SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA
			{
				cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA))
			};
			flag = SetupDiEnumDeviceInterfaces(intPtr, IntPtr.Zero, classGuid, num, out deviceInterfaceData);
			if (flag)
			{
				uint requiredSize = 0u;
				SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData = default(SP_DEVICE_INTERFACE_DETAIL_DATA);
				SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, IntPtr.Zero, 0u, out requiredSize, IntPtr.Zero);
				if (requiredSize != 0)
				{
					deviceInterfaceDetailData.cbSize = ((IntPtr.Size == 8) ? 8u : 5u);
					if (SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, ref deviceInterfaceDetailData, 256u, out requiredSize, IntPtr.Zero) && (pnpId == null || deviceInterfaceDetailData.DevicePath.Contains(pnpId)))
					{
						list.Add(deviceInterfaceDetailData.DevicePath);
					}
				}
			}
			num++;
		}
		SetupDiDestroyDeviceInfoList(intPtr);
		return list.ToArray();
	}

	protected virtual void OnAsyncReadCompleted(DeviceAsyncEventArgs e)
	{
		if (this.AsyncReadCompleted != null)
		{
			this.AsyncReadCompleted(this, e);
		}
	}

	protected virtual void OnAsyncWriteCompleted(DeviceAsyncEventArgs e)
	{
		if (this.AsyncWriteCompleted != null)
		{
			this.AsyncWriteCompleted(this, e);
		}
	}

	protected virtual void OnDetached(EventArgs e)
	{
		if (this.Detached != null)
		{
			this.Detached(this, e);
		}
	}

	protected virtual void OnOpened(EventArgs e)
	{
		if (this.Opened != null)
		{
			this.Opened(this, e);
		}
	}

	protected virtual void OnClosed(EventArgs e)
	{
		if (this.Closed != null)
		{
			this.Closed(this, e);
		}
	}

	protected virtual void OnDisposed(EventArgs e)
	{
		if (this.Disposed != null)
		{
			this.Disposed(this, e);
		}
	}

	protected virtual void BytesRead(IAsyncResult readResult)
	{
		byte[] array = (byte[])readResult.AsyncState;
		try
		{
			DeviceStream.EndRead(readResult);
		}
		catch (Exception ex)
		{
			if (!CheckForDisconnection(ex))
			{
				throw ex;
			}
			array = null;
		}
		OnAsyncReadCompleted(new DeviceAsyncEventArgs(array));
	}

	protected virtual void BytesWritten(IAsyncResult writeResult)
	{
		byte[] array = (byte[])writeResult.AsyncState;
		try
		{
			DeviceStream.EndWrite(writeResult);
		}
		catch (Exception ex)
		{
			if (!CheckForDisconnection(ex))
			{
				throw ex;
			}
		}
		OnAsyncWriteCompleted(new DeviceAsyncEventArgs(array));
	}

	protected bool CheckForDisconnection(Exception exception)
	{
		if (exception is OperationCanceledException || (exception is IOException && exception.Message == "The device is not connected.\r\n"))
		{
			Close();
			OnDetached(EventArgs.Empty);
			return true;
		}
		return false;
	}
}
