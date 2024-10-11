using UnityEngine;

[System.Serializable]
public class PlayerData
{
	public string name;
	public SerializableDictionary<string, bool> itemCollected;
	public string[] lastCollidedTag;
	public int[] buttonIndex;
	public Vector3 playerPosition;
	public int scoreData;
	public PlayerData()
	{
		itemCollected = new SerializableDictionary<string, bool>();
		playerPosition = Vector3.zero;
		this.scoreData = 0;
	}
}
