using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour, ISaveManager
{
	// Start is called before the first frame update
	[Header("End Screen")]
	[SerializeField] private UI_FadeScreen fadeScreen;
	[SerializeField] private GameObject endText;
	[SerializeField] private GameObject restartButton;
	[SerializeField] private GameObject NextButton;
	[SerializeField] private GameObject SelectionButton;
	[SerializeField] private GameObject returnMainMenuButton;
	[Space]

	[SerializeField] private GameObject characterUI;
	[SerializeField] private GameObject skillTreeUI;
	[SerializeField] private GameObject craftUI;
	[SerializeField] private GameObject optionsUI;
	[SerializeField] private GameObject inGameUI;



	[SerializeField] private GameObject uiButton;

	public UI_ItemToolTip itemToolTip;
	public UI_StatToolTip statToolTip;
	public UI_CraftWindow craftWindow;
	public UI_SkillToolTip skillToolTip;

	[SerializeField] private UI_VolumeSlider[] volumeSettings;

	private void Awake()
	{
		fadeScreen.gameObject.SetActive(true);
	}
	void Start()
	{
		SwitchTo(skillTreeUI);
		itemToolTip.gameObject.SetActive(false);
		statToolTip.gameObject.SetActive(false);
		StartCoroutine(SwitchInGame());
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			SwitchWithKeyTo(characterUI);
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			SwitchWithKeyTo(craftUI);
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			SwitchWithKeyTo(skillTreeUI);
		}

		if (Input.GetKeyDown(KeyCode.N))
		{
			SwitchWithKeyTo(optionsUI);
		}
	}

	public void SwitchTo(GameObject _menu)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			bool isFadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
			if (!isFadeScreen)
				transform.GetChild(i).gameObject.SetActive(false);
		}
		uiButton.gameObject.SetActive(true);
		if (_menu != null)
		{
			_menu.SetActive(true);
		}

		if (GameManager.instance != null)
		{
			if (_menu == inGameUI)
			{
				GameManager.instance.PauseGame(false);
			}
			else
			{
				GameManager.instance.PauseGame(true);
			}
		}
	}

	public void SwitchWithKeyTo(GameObject _menu)
	{
		if (_menu != null && _menu.activeSelf)
		{
			_menu.SetActive(false);
			CheckForInGameUI();
			return;
		}

		SwitchTo(_menu);

	}

	private void CheckForInGameUI()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).gameObject.tag != "Button" && !transform.GetChild(i).GetComponent<UI_FadeScreen>())
			{
				return;
			}
		}
		SwitchTo(inGameUI);
	}

	private IEnumerator SwitchInGame()
	{
		yield return null;
		SwitchTo(inGameUI);
	}

	public void SwitchOnEndScreen(bool _isVictory)
	{
		// SwitchTo(null);
		// fadeScreen.gameObject.SetActive(true);
		fadeScreen.FadeOut();
		StartCoroutine(EndScreenCorutione(_isVictory));
	}

	// 只需要传入是否胜利即可
	IEnumerator EndScreenCorutione(bool _isVictory)
	{
		yield return new WaitForSeconds(1);
		if (!_isVictory) endText.GetComponent<TextMeshProUGUI>().text = "You Died!";
		else endText.GetComponent<TextMeshProUGUI>().text = "Victory!";
		endText.SetActive(true);

		yield return new WaitForSeconds(1.5f);
		SelectionButton.SetActive(true);
		if (!_isVictory) restartButton.SetActive(true);
		else NextButton.SetActive(true);
	}

	public void SelectionMap()
	{
		SceneManager.LoadScene("MapScene");
		MapManager.instance.mapCanvas.gameObject.SetActive(true);
	}

	public void RestartGameButton() => GameManager.instance.RestartScene();

	public void NextGameButton() => MapManager.instance.ToGameScene("GameScene" + $"{MapManager.instance.currentPassMap}");

	public void LoadData(GameData _data)
	{
		foreach(KeyValuePair<string,float> pair in _data.volumeSettings)
		{
			foreach(UI_VolumeSlider item in volumeSettings)
			{
				if(item.parameter == pair.Key)
				{
					item.LoadSlider(pair.Value);
				}
			}
		}
	}

	public void SaveData(ref GameData _data)
	{
		_data.volumeSettings.Clear();
		foreach(UI_VolumeSlider item in volumeSettings)
		{
			_data.volumeSettings.Add(item.parameter, item.slider.value);
		}
	}
}
