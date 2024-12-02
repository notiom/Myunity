using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour, ISaveManager
{
	public static MapManager instance; // ����ģʽ
	public int currentPassMap;
	[SerializeField] private UI_FadeScreen fadeScreen;
	public Canvas mapCanvas;

	[SerializeField] private Transform[] mapSlotParent;

	private void Awake()
	{
		// ����Ѿ�����ʵ���������ٵ�ǰ��ʵ��
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		// ��������Ϊ��ǰʵ������ȷ��������
		instance = this;
		DontDestroyOnLoad(gameObject);

	}
	private void Start()
	{
		UnlockMap();
	}

	public void LoadData(GameData _data)
	{
		this.currentPassMap = _data.currentPassMap;
	}

	public void SaveData(ref GameData _data)
	{
		_data.currentPassMap = this.currentPassMap;
	}

	public void ToGameScene(string _sceneName) => StartCoroutine(ToGameSceneWithDelay(_sceneName));

	public IEnumerator ToGameSceneWithDelay(string _sceneName)
	{
		fadeScreen.FadeOut();

		yield return new WaitForSeconds(1.5f);

		SceneManager.LoadScene(_sceneName);
		mapCanvas.gameObject.SetActive(false);
	}

	
	public void UnlockMap()
	{
		for(int i = 0;i < currentPassMap;i++)
		{
			mapSlotParent[i].gameObject.SetActive(false);
		}

		for(int i = currentPassMap; i < mapSlotParent.Length;i++) 
		{
			mapSlotParent[i].gameObject.SetActive(true);
		}
	}

}
