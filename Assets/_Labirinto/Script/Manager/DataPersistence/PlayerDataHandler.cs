using System;
using System.IO;
using UnityEngine;

public class PlayerDataHandler
{
	private string dataDirPath = "";
	private string dataFileName = "";

	public PlayerDataHandler(string dataDirPath, string dataFileName)
	{
		this.dataDirPath = dataDirPath;
		this.dataFileName = dataFileName;
	}
	public PlayerData Load()
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);
		PlayerData loadData = null;
		if (File.Exists(fullPath))
		{
			try
			{
				string dataToLoad = "";
				using (FileStream stream = new FileStream(fullPath, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						dataToLoad = reader.ReadToEnd();
					}
				}
				loadData = JsonUtility.FromJson<PlayerData>(dataToLoad);
			}
			catch (Exception e)
			{
				Debug.LogError("Error to load data file: " + fullPath + e);
			}
		}
		return loadData;
	}
	public void Save(PlayerData data)
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			string dataToStore = JsonUtility.ToJson(data, true);

			using (FileStream stream = new FileStream(fullPath, FileMode.Create))
			{
				using (StreamWriter writer = new StreamWriter(stream))
				{
					writer.Write(dataToStore);
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Error to save data file: " + fullPath + e);
		}
	}
}
