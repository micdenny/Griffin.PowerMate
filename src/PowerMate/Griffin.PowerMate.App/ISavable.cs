namespace Griffin.PowerMate.App;

public interface ISavable
{
	bool Save(string path);

	bool Save(string path, bool overwrite);
}
