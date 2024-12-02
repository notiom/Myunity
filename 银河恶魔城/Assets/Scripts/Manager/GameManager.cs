using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour,ISaveManager
{
	public static GameManager instance;
	[SerializeField] private Checkpoint[] checkpoints;
	public string closetCheckpointLoaded;
	private void Awake()
	{
		if (instance != null)
		{
			Destroy(instance.gameObject);
		}
		else
			instance = this;
	}

	private void Start()
	{
		checkpoints = FindObjectsOfType<Checkpoint>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			RestartScene();
		}
	}

	public void RestartScene()
    {
		SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

	public void LoadData(GameData _data)
	{
		LoadCheckpoints(_data);
		LoadClosetCheckpoint(_data);
	}

	private void LoadClosetCheckpoint(GameData _data)
	{
		if (_data.closetCheckpointId == null) return;
		foreach (Checkpoint checkpoint in checkpoints)
		{
			if (_data.closetCheckpointId == checkpoint.id)
			{
				PlayerManager.instance.player.transform.position = checkpoint.transform.position;
				closetCheckpointLoaded = _data.closetCheckpointId;
			}
		}
	}

	private void LoadCheckpoints(GameData _data)
	{
		foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
		{
			foreach (Checkpoint checkpoint in checkpoints)
			{
				if (checkpoint.id == pair.Key && pair.Value)
				{
					checkpoint.ActivateCheckpoint();
				}
			}
		}
	}

	public void SaveData(ref GameData _data)
	{
		_data.closetCheckpointId = FindCloseCheckPoint()?.id;
		_data.checkpoints.Clear();
		foreach (Checkpoint checkpoint in checkpoints)
		{
			_data.checkpoints.Add(checkpoint.id, checkpoint.activated);
		}
	}

	private Checkpoint FindCloseCheckPoint()
	{
		float closeDistance = Mathf.Infinity;
		Checkpoint closetCheckpoint = null;

		foreach(Checkpoint checkpoint in checkpoints) 
		{
			float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);
			if(distanceToCheckpoint < closeDistance && checkpoint.activated) 
			{
				closeDistance = distanceToCheckpoint;
				closetCheckpoint = checkpoint;
			}
		}
		return closetCheckpoint;
	}

	public void PauseGame(bool _pause)
	{
		if(_pause) 
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

}
