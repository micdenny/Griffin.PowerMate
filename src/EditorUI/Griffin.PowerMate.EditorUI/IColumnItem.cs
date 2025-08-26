using System.Drawing;

namespace Griffin.PowerMate.EditorUI;

internal interface IColumnItem
{
	Icon Icon { get; }

	string Text { get; set; }

	Color TextColor { get; }

	bool Selected { get; set; }

	event ColumnItemHandler TextChanged;

	event ColumnItemHandler IconChanged;

	event ColumnItemHandler SelectedChanged;
}
