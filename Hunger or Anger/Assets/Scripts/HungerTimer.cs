using UnityEngine;
using UnityEngine.UI;


public class HungerTimer : MonoBehaviour
{
    public Slider hungerSlider;
    public Text timerText;
    public float gameTime;

	bool stopTimer = false;

	private void Update()
	{
		float time = gameTime - Time.time;

		int minutes = Mathf.FloorToInt(time / 60);
		int seconds = Mathf.FloorToInt(time - minutes * 60);

		string textTime = $"{minutes}:{seconds}";

		if (time >= hungerSlider.maxValue) stopTimer = true;
		if (stopTimer == false)
		{
			timerText.text = textTime;
			hungerSlider.value = time;
		}
	}
}
