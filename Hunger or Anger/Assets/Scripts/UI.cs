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
	[SerializeField] GameObject win_lose_panel;
	[SerializeField] GameObject infoPanel;


	private void Update()
	{
		if (!angerSlider || !attentionSlider || !hungerSlider || !win_lose_panel || !infoPanel) return;
		if (angerSlider.value == angerSlider.maxValue)
		{
			Time.timeScale = 0;
			win_lose_panel.SetActive(true);
			win_lose_panel.GetComponentInChildren<Text>().text = "You were too bold, so the owner threw you out of the house.";
		}
		else if (attentionSlider.value == attentionSlider.maxValue)
		{
			Time.timeScale = 0;
			win_lose_panel.GetComponentInChildren<Text>().text 
				= "Excellent work! You caught the hostess's attention in a rather bold way. She remembered that she hadn't fed you in a long time!";
			win_lose_panel.SetActive(true);


		}
		else if (hungerSlider.value == 0)
		{
			Time.timeScale = 0;
			win_lose_panel.GetComponentInChildren<Text>().text = "You were too modest and inconspicuous. The mistress forgot to feed you, so you died of hunger.";

			win_lose_panel.SetActive(true);

		}

		
	}

	public void AddValueToAttantionSlider(int value)
	{
		var new_value = attentionSlider.value + value;
		if (new_value <= attentionSlider.maxValue)
		{
			attentionSlider.value += value;
		}
	}
	public void AddValueToAngerSlider(int value)
	{
		var new_value = angerSlider.value + value;
		if (new_value <= angerSlider.maxValue)
		{
			angerSlider.value += value;
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		if (win_lose_panel)
		win_lose_panel.SetActive(false);
		if (interactionButton)
		interactionButton.SetActive(false);

		if (angerSlider)
		angerSlider.maxValue = 100;
		if (attentionSlider)
		attentionSlider.maxValue = 100;
		Time.timeScale = 0;
	}

	public void LoadGame()
	{
		Time.timeScale = 1f;
		infoPanel.SetActive(false);
	}

	public void AddValueToSlider(Slider s, int val)
	{
		s.value += val;
	}

	public void ShowInteractionButton(bool show)
	{
		if (interactionButton)
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
