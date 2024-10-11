using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataPersistenceManager : MonoBehaviour
{
	[Header("File Storage Config")]
	[SerializeField] private string player;

	private PlayerData playerData;

	private List<IPlayerDataPersistence> playerDataPersistenceObjects;

	private PlayerDataHandler playerDataHandler;
	public static PlayerDataPersistenceManager instance { get; private set; }

	private void Start()
	{
		this.playerDataHandler = new PlayerDataHandler(Application.persistentDataPath, player);
		this.playerDataPersistenceObjects = FindAllDataPersistenceObjects();
		LoadPlayer();
	}
	public void NewPlayer()
	{
		this.playerData = new PlayerData();
	}
	public void LoadPlayer()
	{
		this.playerData = playerDataHandler.Load();
		if (this.playerData == null)
		{
			NewPlayer();
			Debug.Log("New Player ");
		}
		foreach (IPlayerDataPersistence dataPersistenceObj in playerDataPersistenceObjects)
		{
			dataPersistenceObj.PlayerLoad(playerData);
		}
		Debug.Log("Loaded Player Data: " + playerData.name);
	}
	public void SaveGame()
	{
		foreach (IPlayerDataPersistence dataPersistenceObj in playerDataPersistenceObjects)
		{
			dataPersistenceObj.PlayerSave(ref playerData);
		}
		Debug.Log("Saved Player Data: " + playerData.name);
		playerDataHandler.Save(playerData);
	}
	public void SaveLocal()
	{
		SaveGame();
	}
	private List<IPlayerDataPersistence> FindAllDataPersistenceObjects()
	{
		IEnumerable<IPlayerDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerDataPersistence>();

		return new List<IPlayerDataPersistence>(dataPersistenceObjects);
	}
}
