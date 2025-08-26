using System;
using System.Runtime.InteropServices;

namespace Griffin.Audio;

public class XPVolume : IOSVolume, IDisposable
{
	private enum MMSYSERR
	{
		NOERROR = 0,
		BADDEVICEID = 2,
		ALLOCATED = 4,
		INVALHANDLE = 5,
		NODRIVER = 6,
		NOMEM = 7,
		NOTSUPPORTED = 8,
		INVALFLAG = 10,
		INVALPARAM = 11
	}

	private enum MIXER_GETLINEINFOF : uint
	{
		DESTINATION,
		SOURCE,
		LINEID,
		COMPONENTTYPE,
		TARGETTYPE
	}

	private enum MIXERLINE_COMPONENTTYPE : uint
	{
		DST_UNDEFINED,
		DST_DIGITAL,
		DST_LINE,
		DST_MONITOR,
		DST_SPEAKERS,
		DST_HEADPHONES,
		DST_TELEPHONE,
		DST_WAVEIN,
		DST_VOICEIN
	}

	private enum MIXER_GETLINECONTROLSF : uint
	{
		ALL,
		ONEBYID,
		ONEBYTYPE
	}

	private enum MIXER_GETCONTROLDETAILSF : uint
	{
		VALUE,
		LISTTEXT
	}

	private enum MIXERCONTROL_CONTROLTYPE : uint
	{
		MUTE = 536936450u,
		VOLUME = 1342373889u
	}

	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	private struct MIXERCONTROL_BOUNDS
	{
		[FieldOffset(0)]
		public int lMinimum;

		[FieldOffset(4)]
		public int lMaximum;

		[FieldOffset(0)]
		public uint dwMinimum;

		[FieldOffset(4)]
		public uint dwMaximum;

		[FieldOffset(0)]
		public uint dwReserved1;

		[FieldOffset(4)]
		public uint dwReserved2;

		[FieldOffset(8)]
		public uint dwReserved3;

		[FieldOffset(12)]
		public uint dwReserved4;

		[FieldOffset(16)]
		public uint dwReserved5;

		[FieldOffset(20)]
		public uint dwReserved6;
	}

	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	private struct MIXERCONTROL_METRICS
	{
		[FieldOffset(0)]
		public uint cSteps;

		[FieldOffset(0)]
		public uint cbCustomData;

		[FieldOffset(0)]
		public uint dwReserved1;

		[FieldOffset(4)]
		public uint dwReserved2;

		[FieldOffset(8)]
		public uint dwReserved3;

		[FieldOffset(12)]
		public uint dwReserved4;

		[FieldOffset(16)]
		public uint dwReserved5;

		[FieldOffset(20)]
		public uint dwReserved6;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct MIXERCONTROL
	{
		public int cbStruct;

		public uint dwControlID;

		public uint dwControlType;

		public uint fdwControl;

		public uint cMultipleItems;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string szShortName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string szName;

		public MIXERCONTROL_BOUNDS Bounds;

		public MIXERCONTROL_METRICS Metrics;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	private struct MIXERCONTROLDETAILS
	{
		public int cbStruct;

		public uint dwControlID;

		public uint cChannels;

		public IntPtr hwndOwner;

		public int cbDetails;

		public IntPtr paDetails;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct MIXERCONTROLDETAILS_UNSIGNED
	{
		public uint dwValue;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct MIXERCONTROLDETAILS_BOOLEAN
	{
		public bool fValue;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	private struct MIXERLINETARGET
	{
		public uint dwType;

		public uint dwDeviceID;

		public ushort wMid;

		public ushort wPid;

		public uint vDriverVersion;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string szPname;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	private struct MIXERLINE
	{
		public int cbStruct;

		public uint dwDestination;

		public uint dwSource;

		public uint dwLineID;

		public uint fdwLine;

		public IntPtr dwUser;

		public MIXERLINE_COMPONENTTYPE dwComponentType;

		public uint cChannels;

		public uint cConnections;

		public uint cControls;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string szShortName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string szName;

		public MIXERLINETARGET Target;
	}

	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Pack = 4)]
	private struct MIXERLINECONTROLS
	{
		[FieldOffset(0)]
		public int cbStruct;

		[FieldOffset(4)]
		public uint dwLineID;

		[FieldOffset(8)]
		public uint dwControlID;

		[FieldOffset(8)]
		public MIXERCONTROL_CONTROLTYPE dwControlType;

		[FieldOffset(12)]
		public uint cControls;

		[FieldOffset(16)]
		public int cbmxctrl;

		[FieldOffset(20)]
		public IntPtr pamxctrl;
	}

	private const int MAXPNAMELEN = 32;

	private const int MIXER_LONG_NAME_CHARS = 64;

	private const int MIXER_SHORT_NAME_CHARS = 16;

	private const int VOLUME_MAX = 65535;

	private bool IsDisposed;

	private IntPtr MixerHandle = IntPtr.Zero;

	private MIXERCONTROL VolumeControl;

	private MIXERCONTROL MuteControl;

	public float MasterVolume
	{
		get
		{
			uint num = 0u;
			if (!IsDisposed)
			{
				MIXERCONTROLDETAILS mixerControlDetails = default(MIXERCONTROLDETAILS);
				int num2 = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS_UNSIGNED));
				mixerControlDetails.cbStruct = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS));
				mixerControlDetails.dwControlID = VolumeControl.dwControlID;
				mixerControlDetails.paDetails = Marshal.AllocHGlobal(num2);
				mixerControlDetails.cChannels = 1u;
				mixerControlDetails.cbDetails = num2;
				if (mixerGetControlDetails(MixerHandle, ref mixerControlDetails, MIXER_GETCONTROLDETAILSF.VALUE) == MMSYSERR.NOERROR)
				{
					num = ((MIXERCONTROLDETAILS_UNSIGNED)Marshal.PtrToStructure(mixerControlDetails.paDetails, typeof(MIXERCONTROLDETAILS_UNSIGNED))).dwValue;
				}
				Marshal.FreeHGlobal(mixerControlDetails.paDetails);
			}
			return (float)num / 65535f;
		}
	}

	public bool MasterMute
	{
		get
		{
			bool result = false;
			if (!IsDisposed)
			{
				MIXERCONTROLDETAILS mixerControlDetails = default(MIXERCONTROLDETAILS);
				int num = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS_BOOLEAN));
				mixerControlDetails.cbStruct = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS));
				mixerControlDetails.dwControlID = MuteControl.dwControlID;
				mixerControlDetails.paDetails = Marshal.AllocHGlobal(num);
				mixerControlDetails.cChannels = 1u;
				mixerControlDetails.cbDetails = num;
				if (mixerGetControlDetails(MixerHandle, ref mixerControlDetails, MIXER_GETCONTROLDETAILSF.VALUE) == MMSYSERR.NOERROR)
				{
					result = ((MIXERCONTROLDETAILS_BOOLEAN)Marshal.PtrToStructure(mixerControlDetails.paDetails, typeof(MIXERCONTROLDETAILS_BOOLEAN))).fValue;
				}
				Marshal.FreeHGlobal(mixerControlDetails.paDetails);
			}
			return result;
		}
	}

	[DllImport("winmm.dll", CharSet = CharSet.Unicode)]
	private static extern MMSYSERR mixerClose(IntPtr mixerHandle);

	[DllImport("winmm.dll", CharSet = CharSet.Unicode)]
	private static extern MMSYSERR mixerGetControlDetails(IntPtr mixerHandle, ref MIXERCONTROLDETAILS mixerControlDetails, MIXER_GETCONTROLDETAILSF flags);

	[DllImport("winmm.dll", CharSet = CharSet.Unicode)]
	private static extern MMSYSERR mixerGetLineControls(IntPtr mixerHandle, ref MIXERLINECONTROLS mixerLineControls, MIXER_GETLINECONTROLSF flags);

	[DllImport("winmm.dll", CharSet = CharSet.Unicode)]
	private static extern MMSYSERR mixerGetLineInfo(IntPtr mixerHandle, ref MIXERLINE mixerLine, MIXER_GETLINEINFOF flags);

	[DllImport("winmm.dll", CharSet = CharSet.Unicode)]
	private static extern MMSYSERR mixerOpen(out IntPtr mixerHandle, uint mixerDeviceID, IntPtr dwCallback, IntPtr dwInstance, uint fdwOpen);

	public XPVolume()
	{
		if (mixerOpen(out MixerHandle, 0u, IntPtr.Zero, IntPtr.Zero, 0u) != MMSYSERR.NOERROR)
		{
			return;
		}
		MIXERLINE mixerLine = default(MIXERLINE);
		mixerLine.cbStruct = Marshal.SizeOf((object)mixerLine);
		mixerLine.dwComponentType = MIXERLINE_COMPONENTTYPE.DST_SPEAKERS;
		if (mixerGetLineInfo(MixerHandle, ref mixerLine, MIXER_GETLINEINFOF.COMPONENTTYPE) == MMSYSERR.NOERROR)
		{
			int num = Marshal.SizeOf(typeof(MIXERCONTROL));
			MIXERLINECONTROLS mixerLineControls = default(MIXERLINECONTROLS);
			mixerLineControls.pamxctrl = Marshal.AllocHGlobal(num);
			mixerLineControls.cbStruct = Marshal.SizeOf((object)mixerLineControls);
			mixerLineControls.dwLineID = mixerLine.dwLineID;
			mixerLineControls.dwControlType = MIXERCONTROL_CONTROLTYPE.VOLUME;
			mixerLineControls.cControls = 1u;
			mixerLineControls.cbmxctrl = num;
			if (mixerGetLineControls(MixerHandle, ref mixerLineControls, MIXER_GETLINECONTROLSF.ONEBYTYPE) == MMSYSERR.NOERROR)
			{
				VolumeControl = (MIXERCONTROL)Marshal.PtrToStructure(mixerLineControls.pamxctrl, typeof(MIXERCONTROL));
			}
			mixerLineControls.dwControlType = MIXERCONTROL_CONTROLTYPE.MUTE;
			if (mixerGetLineControls(MixerHandle, ref mixerLineControls, MIXER_GETLINECONTROLSF.ONEBYTYPE) == MMSYSERR.NOERROR)
			{
				MuteControl = (MIXERCONTROL)Marshal.PtrToStructure(mixerLineControls.pamxctrl, typeof(MIXERCONTROL));
			}
			Marshal.FreeHGlobal(mixerLineControls.pamxctrl);
		}
	}

	public void Dispose()
	{
		if (!IsDisposed)
		{
			mixerClose(MixerHandle);
		}
	}
}
