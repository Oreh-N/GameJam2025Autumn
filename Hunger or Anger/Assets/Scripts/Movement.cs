using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{   // It is important to set correct possition (so that map were detecting the character correctly)
	// Correct if gizmo on character's position
	[SerializeField] int speed = 1;


	private void Update()
	{
		Vector2 dir = GetAnyWayDirection();
		TryMove(dir);
	}

	private void TryMove(Vector2 dir)
	{
		Vector2Int cellSize = new Vector2Int(1, 1);
		dir *= speed;
		var nxt_pos = transform.position + new Vector3(dir.x * cellSize.x, dir.y * cellSize.y, 0);
		int val = Map.Instance.GetMapValue(nxt_pos);
		if (val != -1 && (val == (int)Items.Empty || val == (int)Items.Carpets))
			transform.position = nxt_pos;
	}


	private Vector2 GetAnyWayDirection()
	{
		Vector2 dir = Vector2.zero;
		if (Input.GetKeyDown(KeyCode.A)) dir.x = -1;
		if (Input.GetKeyDown(KeyCode.D)) dir.x = 1;
		if (Input.GetKeyDown(KeyCode.S)) dir.y = -1;
		if (Input.GetKeyDown(KeyCode.W)) dir.y = 1;

		if (dir != Vector2.zero)
			dir = dir.normalized;

		return dir;
	}
}
