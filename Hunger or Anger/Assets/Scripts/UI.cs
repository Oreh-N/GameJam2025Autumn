using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
	[SerializeField] Slider attentionSlider;
	[SerializeField] Slider angerSlider;
	[SerializeField] Slider hungerSlider;


	public void AddValueToSlider(Slider s, int val)
	{
		s.value += val;
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
