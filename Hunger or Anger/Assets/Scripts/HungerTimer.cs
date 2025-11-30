using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class HungerTimer : MonoBehaviour
{
    Slider hungerSlider;
    public Text timerText;
    public float roundLength;

	bool stopTimer = false;

	private void Start()
	{
		hungerSlider = GetComponent<Slider>();
		hungerSlider.maxValue = roundLength;
		hungerSlider.value = roundLength;
	}

	private void Update()
	{
		float time = roundLength - Time.time;

		int minutes = Mathf.FloorToInt(time / 60);
		int seconds = Mathf.FloorToInt(time - minutes * 60);

		string textTime = $"Time until death from starvation: {minutes}:{seconds}";

		if (time <= 0) stopTimer = true;
		if (stopTimer == false)
		{
			timerText.text = textTime;
			hungerSlider.value = time;
		}
	}
}
