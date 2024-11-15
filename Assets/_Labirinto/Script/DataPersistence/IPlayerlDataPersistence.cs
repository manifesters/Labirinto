using DataPersistence;

namespace DataPersistence
{
	public interface IPlayerDataPersistence
	{
		void LoadPlayerData(PlayerData playerData);
		void SavePlayerData(PlayerData playerData);
	}
}
