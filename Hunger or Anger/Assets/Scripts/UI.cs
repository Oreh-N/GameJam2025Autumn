using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	public static UI Instance;
	[SerializeField] Slider attentionSlider;
	[SerializeField] Slider angerSlider;
	[SerializeField] Slider hungerSlider;
	[SerializeField] GameObject interactionButton;



	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		interactionButton.SetActive(false);
	}

	public void AddValueToSlider(Slider s, int val)
	{
		s.value += val;
	}

	public void ShowInteractionButton(bool show)
	{
		interactionButton.SetActive(show);
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Game");
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void BackToMenu()
	{
		SceneManager.LoadScene("Menu");
	}
}
