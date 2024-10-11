public interface IPlayerDataPersistence
{
	void PlayerLoad(PlayerData playerData);
	void PlayerSave(ref PlayerData playerData);
}
