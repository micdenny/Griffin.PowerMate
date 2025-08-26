using System.Runtime.InteropServices;

namespace Griffin.PowerMate.iTunesActions;

[Guid("6B1BD814-CA6E-4063-9EDA-4128D31068C1")]
internal enum ITPlayButtonState
{
	ITPlayButtonStatePlayDisabled,
	ITPlayButtonStatePlayEnabled,
	ITPlayButtonStatePauseEnabled,
	ITPlayButtonStatePauseDisabled,
	ITPlayButtonStateStopEnabled,
	ITPlayButtonStateStopDisabled
}
