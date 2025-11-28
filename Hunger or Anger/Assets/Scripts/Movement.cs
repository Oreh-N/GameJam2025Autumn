using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

	[SerializeField] float speed = 0.03f;

	private void Update()
	{
		TryMove();
	}

	private void TryMove()
	{
		Vector2 dir = GetAnyWayDirection();
		dir *= speed;
		transform.position += new Vector3(dir.x, dir.y, 0);
	}

	private Vector2 GetAnyWayDirection()
	{
		Vector2 dir = Vector2.zero;

		if (Keyboard.current.aKey.isPressed) dir.x = -1;
		if (Keyboard.current.dKey.isPressed) dir.x = 1;
		if (Keyboard.current.sKey.isPressed) dir.y = -1;
		if (Keyboard.current.wKey.isPressed) dir.y = 1;

		if (dir != Vector2.zero)
			dir = dir.normalized;

		return dir;
	}
}
