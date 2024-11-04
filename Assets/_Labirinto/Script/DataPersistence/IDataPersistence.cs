using DataPersistence;

namespace DataPersistence
{
	public interface IDataPersistence
	{
		void LoadData(GameData data);
		void SaveData(GameData data);
	}
}