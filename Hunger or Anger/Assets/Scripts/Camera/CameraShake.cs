using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public static CameraShake Instance;
	Camera _cam;

	Vector3 _startPos;
	float _intensity;
	float _time;
	bool _isShaking;

	private void Awake()
	{
		// Ensure only one instance exists
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject); // Optional: persist between scenes
	}

	private void Update()
	{
		if (_time > 0 && _isShaking)
		{
			Shake();
		}
		else
		{
			if (_cam)
				_cam.transform.position = _startPos;
			_isShaking = false;
		}
	}

	private void Shake()
	{
		_cam.transform.localPosition = _startPos + Random.insideUnitSphere * _intensity;
		_time -= Time.deltaTime + EaseShake(Time.deltaTime);
	}

	private float EaseShake(float t)
	{
		return Mathf.Pow(1 - (1 - t), 3);
	}

	public void Shake(float time, float intensity, Camera cam)
	{
		if (_isShaking) _cam.transform.position = _startPos;
		_isShaking = true;
		_intensity = intensity;
		_startPos = cam.transform.position;
		_time = time;
		_cam = cam;
	}
}
