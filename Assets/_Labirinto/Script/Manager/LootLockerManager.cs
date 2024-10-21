using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;

public class LootLockerManager : MonoBehaviour
{
	public Button checkInternet;
	public bool checker = true;
	public GameObject noInternetPanel;

	private string zipfileName = "save_slot.zip"; //name of zipped folder 
	private string zippedFolderFileName = "save_slot"; //name of unzipped folder
	// Start is called before the first frame update
	void Start()
    {
		
	}

	public void CheckNameFromLootLocker(string playerName)
	{
		if (checker == false)
		{
			LootLockerSDKManager.GetPlayerName((infoResponse) =>
			{
				if (infoResponse.success)
				{
					// Compare the existing player name with the provided name
					if (infoResponse.name == playerName)
					{
						Debug.LogWarning("Player name already exists: " + playerName);
					}
					else
					{
						// Save the new player name if it doesn't exist
						SaveDataToLootLocker(playerName);
					}
				}
				else
				{
					Debug.LogError("Failed to get player info.");
				}
			});
		}
		else
		{
			Debug.Log("No internet connection");
		}
	}
	public void SaveDataToLootLocker(string playerName) //pang set name sa player
	{
		if (checker == false)
		{
			// Define custom data in key-value pairs
			LootLockerSDKManager.SetPlayerName(playerName, (response) =>
			{
				if (response.success)
				{
					Debug.Log("Player name saved successfully in lootlocker");
				}
				else
				{
					Debug.LogError("Failed to save player data online: ");
				}
			});
		}
		else
		{
			Debug.Log("No internet connection");
		}
	}
	public void DownloadPlayerFiles() //ichecheck niya gamit si name kang player sa lootlocker pag download nin file
	{
		LootLockerSDKManager.GetPlayerName((playerResponse) =>
		{
			if (playerResponse.success)
			{
				// Extract player name
				string playerName = playerResponse.name;
				Debug.Log($"Player Name: {playerName}");

				// Proceed with downloading files for the player
				Download(); //si pang download 
			}
			else
			{
				Debug.LogError("Failed to get player info.");
			}
		});
	}
	public void Download() //si pang download kang zipped file hali sa lootlocker
	{
		string zippedFolderPath = Path.Combine(Application.persistentDataPath, zippedFolderFileName);
		if (Directory.Exists(zippedFolderPath))
		{
			// Delete the existing folder and its contents
			Directory.Delete(zippedFolderPath, true);
			Debug.Log("Existing folder deleted in download method.");
		}
		if (checker == false)
		{
			LootLockerSDKManager.GetAllPlayerFiles((response) =>
			{
				if (response.success)
				{
					foreach (var file in response.items)
					{
						string url = ExtractUrl(file.url);
						Debug.Log("Player Data extracted from url");

						StartCoroutine(DownloadFile(url));
						Debug.Log("Player Data saved from url");
					}
					Debug.Log("Player Data saved locally");
				}
			});
		}
		else
		{
			Debug.Log("No internet connection");
		}
	}
	private IEnumerator DownloadFile(string url) //gamit si url kang na upload sa lootlocker na file ieextract kani
	{
		UnityWebRequest request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();

		string fullPath = Path.Combine(Application.persistentDataPath, zipfileName);

		if (request.result == UnityWebRequest.Result.Success)
		{
			File.WriteAllBytes(fullPath, request.downloadHandler.data);
			Debug.Log("File downloaded successfully.");
			UnzipFile(fullPath);//iuunzipped na si folder pag download
			Debug.Log("File unzipped successfully.");
			if (File.Exists(fullPath))
			{
				File.Delete(fullPath);
				Debug.Log("folder1 deleted");
			}
		}
		else
		{
			Debug.LogError($"Failed to download file: {request.error}");
		}
	}
	void UnzipFile(string fullPath)//si pang unzipped ning folder
	{
		string extractPath = Application.persistentDataPath;
		// Unzip the file
		try
		{
			ZipFile.ExtractToDirectory(fullPath, extractPath);
			Debug.Log("File unzipped successfully to: " + extractPath);
		}
		catch (IOException ex)
		{
			Debug.LogError("Error during unzip: " + ex.Message);
		}
	}
	private string ExtractUrl(string fullUrl)
	{
		int index = fullUrl.IndexOf('?');
		if (index > 0)
		{
			return fullUrl.Substring(0, index);
		}
		return fullUrl;
	}
	public void UploadToLootLocker() //pang upload sa lootlocker kang folder na naka zipped
	{
		string fullPath = Path.Combine(Application.persistentDataPath, zipfileName);
		// Step 1: Get all the player files
		LootLockerSDKManager.GetAllPlayerFiles(response =>
		{
			if (response.success)
			{
				// Step 2: Check if a file with the same name exists
				LootLockerPlayerFile oldFile = response.items.FirstOrDefault(f => f.name == zipfileName);

				if (oldFile != null)
				{
					// Step 3: Delete the old file
					LootLockerSDKManager.DeletePlayerFile(oldFile.id, deleteResponse =>
					{
						if (deleteResponse.success)
						{
							Debug.Log("Old file deleted successfully");

							// Proceed with uploading the new file
							UploadNewFile(fullPath);
						}
						else
						{
							Debug.Log("Failed to delete old file");
						}
					});
				}
				else
				{
					// No old file, proceed with upload
					UploadNewFile(fullPath);
				}
			}
			else
			{
				Debug.Log("Failed to get player files");
			}
		});
	}
	private void UploadNewFile(string fullPath)
	{
		string fileContents = File.ReadAllText(fullPath);
		Debug.Log("File contents to upload: " + fileContents);
		LootLockerSDKManager.UploadPlayerFile(fullPath, zipfileName, true, response =>
		{
			if (response.success)
			{
				Debug.Log("Successfully uploaded player file" + response);
			}
			else
			{
				Debug.Log("Error uploading player file");
			}
		});
	}

	public void fileCompressor() //method to call for compressing the folders
	{
		// Array of folder names that need to be compressed into a zip file
		string[] folderToZip = { "save_slot_1, save_slot_2, save_slot_3" };

		// Name of the zip file to be created
		string folderZipName = "save_slot.zip";

		//specific file 
		string[] specificFile = { "testdata.File" };

		// method that performs the compression
		compressedFile(folderToZip, folderZipName, specificFile);
	}
	public void compressedFile(string[] folderPath, string path, string[] specificFilePath)
	{
		//path of data persistence
		string zipFilePath = Path.Combine(Application.persistentDataPath, path);
		//method for archiving
		using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
		{
			foreach (string folderName in folderPath)
			{
				string pathFolder = Path.Combine(Application.persistentDataPath, folderName);

				if (Directory.Exists(pathFolder))
				{
					AddFolderToArchive(archive, pathFolder, folderName);
				}
			}
		}
	}
	private void AddFolderToArchive(ZipArchive archive, string folderPath, string entryName)
	{
		// Loops through all the files in the folder
		foreach (string filePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
		{
			// Creates a path
			string relativePath = Path.Combine(entryName, Path.GetRelativePath(folderPath, filePath));
			// Adds the file to the zip archive
			archive.CreateEntryFromFile(filePath, relativePath);
		}
	}
}
