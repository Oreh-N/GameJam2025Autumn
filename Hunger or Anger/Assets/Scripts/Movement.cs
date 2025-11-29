using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{	// It is important to set correct possition (so that map were detecting the character correctly)
	// Correct if gizmo on character's position

	[SerializeField] int speed = 1;
	Map map;

	private void Awake()
	{
		map = FindAnyObjectByType<Map>();
	}

	private void Update()
	{
		TryMove();
	}

	private void TryMove()
	{
		Vector2 dir = GetAnyWayDirection();

		dir *= speed;
		var new_pos = transform.position;
		new_pos += new Vector3(dir.x * Map.cellSize.x, dir.y * Map.cellSize.y, 0);
		if (map.WhatOnPos(new_pos) == (int)Map.Items.Empty)
			transform.position = new_pos;
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
