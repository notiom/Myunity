using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
	[SerializeField] private string sceneName = "MapScene";
	[SerializeField] private GameObject continueButton;
	private UI_FadeScreen fadeScreen;

	private void Start()
	{
		fadeScreen = gameObject.GetComponentInChildren<UI_FadeScreen>();
		if (!SaveManager.instance.HasSaveData())
		{
			// Òþ²Ø¼ÌÐø°´Å¥
			continueButton.SetActive(false);
		}
	}
	public void ContinueGame()
	{
		fadeScreen.gameObject.SetActive(true);
		StartCoroutine(LoadSceneWithFadeEffect(1.5f));
	}

	public void NewGame()
	{
		SaveManager.instance.DeleteSaveData();
		ContinueGame();
	}

	public void ExitGame()
	{
		SaveManager.instance.QuitGame();
	}

	IEnumerator LoadSceneWithFadeEffect(float _delay)
	{
		fadeScreen.FadeOut();

		yield return new WaitForSeconds(_delay);
		SceneManager.LoadScene(sceneName);
	}
}
