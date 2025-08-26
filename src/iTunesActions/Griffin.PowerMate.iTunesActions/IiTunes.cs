using System.Runtime.InteropServices;

namespace Griffin.PowerMate.iTunesActions;

[ComImport]
[Guid("9DD6680B-3EDC-40DB-A771-E6FE4832E34A")]
[TypeLibType(4176)]
internal interface IiTunes
{
	void BackTrack();

	void FastForward();

	void NextTrack();

	void Pause();

	void Play();

	void PlayFile([In][MarshalAs(UnmanagedType.BStr)] string filePath);

	void PlayPause();

	void PreviousTrack();

	void Resume();

	void Rewind();

	void Stop();

	[return: MarshalAs(UnmanagedType.Interface)]
	object ConvertFile([In][MarshalAs(UnmanagedType.BStr)] string filePath);

	[return: MarshalAs(UnmanagedType.Interface)]
	object ConvertFiles([In][MarshalAs(UnmanagedType.Struct)] ref object filePaths);

	[return: MarshalAs(UnmanagedType.Interface)]
	object ConvertTrack([In][MarshalAs(UnmanagedType.Struct)] ref object iTrackToConvert);

	[return: MarshalAs(UnmanagedType.Interface)]
	object ConvertTracks([In][MarshalAs(UnmanagedType.Struct)] ref object iTracksToConvert);

	bool CheckVersion([In] int majorVersion, [In] int minorVersion);

	[return: MarshalAs(UnmanagedType.Interface)]
	object GetITObjectByID(int sourceID, int playlistID, int trackID, int databaseID);

	[return: MarshalAs(UnmanagedType.Interface)]
	object CreatePlaylist([In][MarshalAs(UnmanagedType.BStr)] string playlistName);

	void OpenURL([In][MarshalAs(UnmanagedType.BStr)] string url);

	void GotoMusicStoreHomePage();

	void UpdateIPod();

	void Authorize([In] int numElems, [In][MarshalAs(UnmanagedType.Struct)] ref object data, [In][MarshalAs(UnmanagedType.BStr)] ref string names);

	void Quit();

	object Sources
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object Encoders
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object EQPresets
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object Visuals
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object Windows
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	int SoundVolume { get; set; }

	bool Mute { get; set; }

	ITPlayerState PlayerState { get; }

	int PlayerPosition { get; set; }

	object CurrentEncoder
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	bool VisualsEnabled { get; set; }

	bool FullScreenVisuals { get; set; }

	ITVisualSize VisualSize { get; set; }

	object CurrentVisual
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	bool EQEnabled { get; set; }

	object CurrentEQPreset
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	string CurrentStreamTitle
	{
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	string CurrentStreamURL
	{
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	object BrowserWindow
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object EQWindow
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object LibrarySource
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object LibraryPlaylist
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object CurrentTrack
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object CurrentPlaylist
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	object SelectedTracks
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	string Version
	{
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	void SetOptions([In] int options);

	[return: MarshalAs(UnmanagedType.Interface)]
	object ConvertFile2([In][MarshalAs(UnmanagedType.BStr)] string filePath);

	[return: MarshalAs(UnmanagedType.Interface)]
	object ConvertFiles2([In][MarshalAs(UnmanagedType.Struct)] ref object filePaths);

	[return: MarshalAs(UnmanagedType.Interface)]
	object ConvertTrack2([In][MarshalAs(UnmanagedType.Struct)] ref object iTrackToConvert);

	[return: MarshalAs(UnmanagedType.Interface)]
	object ConvertTracks2([In][MarshalAs(UnmanagedType.Struct)] ref object iTracksToConvert);

	bool AppCommandMessageProcessingEnabled { get; set; }

	bool ForceToForegroundOnDialog { get; set; }

	[return: MarshalAs(UnmanagedType.Interface)]
	object CreateEQPreset([In][MarshalAs(UnmanagedType.BStr)] string eqPresetName);

	[return: MarshalAs(UnmanagedType.Interface)]
	object CreatelaylistInSource([In][MarshalAs(UnmanagedType.BStr)] string playlistName, [In][MarshalAs(UnmanagedType.Struct)] ref object iSource);

	void GetPlayerButtonsState(out bool previousEnabled, out ITPlayButtonState playPauseStopState, out bool nextEnabled);

	void PlayerButtonClicked([In] ITPlayerButton playerButton, [In] int playerButtonModifierKeys);

	bool CanGetShuffle([In][MarshalAs(UnmanagedType.Struct)] ref object iPlaylist);

	bool CanSetSongRepeat([In][MarshalAs(UnmanagedType.Struct)] ref object iPlaylist);

	object ConvertOperationStatus
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void SubscribeToPodcast([In][MarshalAs(UnmanagedType.BStr)] string url);

	void UpdatePodcastFeeds();

	[return: MarshalAs(UnmanagedType.Interface)]
	object CreateFolder([In][MarshalAs(UnmanagedType.BStr)] string folderName);

	[return: MarshalAs(UnmanagedType.Interface)]
	object CreateFolderInSource([In][MarshalAs(UnmanagedType.BStr)] string folderName, [In][MarshalAs(UnmanagedType.Struct)] ref object iSource);

	bool SoundVolumeControlEnabled { get; }

	string LibraryXMLPath
	{
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
