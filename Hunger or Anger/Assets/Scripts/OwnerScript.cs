using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OwnerScript : MonoBehaviour
{
	public static OwnerScript Instance;

	Queue<Vector3> itemsToCheck = new Queue<Vector3>();
	int radiusToItem = 6;
	bool isCheckingItem = false;


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
	}


	void Start()
    {
        
    }

    void Update()
    {
		if (isCheckingItem && itemsToCheck.Count > 0)
		{
			StartCoroutine(GoCheckItem(itemsToCheck.Dequeue()));
		}
		else
		{
			GetComponent<Wandering>().isMoving = true;

		}
	}

	private IEnumerator GoCheckItem(Vector3 vector3)
	{
		isCheckingItem = true;
		GetComponent<Wandering>().isMoving = false;
		var ownerMapPos = Map.Instance.WorldToMap(transform.position);
		var targetMapPos = Map.Instance.WorldToMap(vector3);
		var pathOnMap = Chasing.Instance.BFS(ownerMapPos, targetMapPos);
		var worldPath = TranslateMapPathToWorld(pathOnMap);
	
		foreach (Vector3 step in worldPath)
		{ yield return StartCoroutine(MoveTo(step)); }

		isCheckingItem = false;
	}

	private IEnumerator MoveTo(Vector3 target)
	{
		float speed = 6f; // your movement speed
		while (Vector3.Distance(transform.position, target) > radiusToItem)
		{
			transform.position = Vector3.MoveTowards(
				transform.position,
				target,
				speed * Time.deltaTime);

			yield return null;
		}
	}

	private List<Vector3> TranslateMapPathToWorld(List<Vector2Int> mapPath)
	{
		List<Vector3> worldPath = new List<Vector3>();
		foreach (var item in mapPath) 
		{
			worldPath.Add(Map.Instance.MapToWorld(item.x, item.y));
		}

		return worldPath;
	}

	public void CheckBrokenItem(Vector3 itemPosition)
    {
		itemsToCheck.Enqueue(itemPosition);
    }
}
