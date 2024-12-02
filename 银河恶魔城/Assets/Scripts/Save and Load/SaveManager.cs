using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{

	public static SaveManager instance;

	[SerializeField] private bool encryptData;
	[SerializeField] private string fileName;
	[SerializeField] private string filePath = "idbfs/xmu-yxl-qwer";

	private GameData gameData;
	private List<ISaveManager> saveManagers;
	private FileDataHandler dataHandler;

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
			instance = this;
	}

	[ContextMenu("Delete save file")]
	public void DeleteSaveData()
	{
		dataHandler = new FileDataHandler(filePath, fileName, encryptData);
		dataHandler.Delete();
	}

	private void Start()
	{
		dataHandler = new FileDataHandler(filePath, fileName, encryptData);
		saveManagers = FindAllSaveManagers();
		LoadGame();
	}

	public void NewGame()
	{
		gameData = new GameData();
	}

	public void LoadGame()
	{
		// game data = data from data handler

		gameData = dataHandler.Load();
		if (this.gameData == null)
		{
			Debug.Log("No saved data found");
			NewGame();
		}

		foreach (ISaveManager saveManager in saveManagers)
		{
			saveManager.LoadData(gameData);
		}
	}

	public void SaveGame()
	{
		foreach (ISaveManager saveManager in saveManagers)
		{
			saveManager.SaveData(ref gameData);
		}
		dataHandler.Save(gameData);
		Debug.Log("Game was saved!");
	}

	private void OnApplicationQuit()
	{
		SaveGame();
	}

	private List<ISaveManager> FindAllSaveManagers()
	{
		IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

		return new List<ISaveManager>(saveManagers);
	}

	public bool HasSaveData()
	{
		if (dataHandler.Load() != null)
		{
			return true;
		}
		return false;
	}

	public void QuitGame()
	{
		// 如果在编辑器模式下，会打印日志，实际构建时会退出
#if UNITY_EDITOR
		SaveManager.instance.SaveGame();
		UnityEditor.EditorApplication.isPlaying = false;
#else
		SaveManager.instance.SaveGame();
		Application.Quit();
#endif
	}
}

