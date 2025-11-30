using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerScript : MonoBehaviour
{
	public static OwnerScript Instance;

	Queue<Vector3> itemsToCheck = new Queue<Vector3>();
	int radiusToItem = 6;
	bool isCheckingItem = false;

	[SerializeField] float checkWaitTime = 1.2f;
	[SerializeField] float moveSpeed = 6f;

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

	void Update()
	{
		if (!isCheckingItem && itemsToCheck.Count > 0)
		{
			Vector3 next = itemsToCheck.Peek();
			StartCoroutine(GoCheckItem(next));
		}
		if (itemsToCheck.Count == 0 && !isCheckingItem)
			GetComponent<Wandering>().isMoving = true;
	}

	private IEnumerator GoCheckItem(Vector3 itemWorldPos)
	{
		isCheckingItem = true;
		GetComponent<Wandering>().isMoving = false;

		if (Vector3.Distance(transform.position, itemWorldPos) <= radiusToItem)
		{
			DequeueMatching(itemWorldPos);
			yield return new WaitForSeconds(checkWaitTime);

			isCheckingItem = false;
			yield break;
		}

		var ownerMapPos = Map.Instance.WorldToMap(transform.position);
		var targetMapPos = Map.Instance.WorldToMap(itemWorldPos);
		var pathOnMap = Chasing.Instance.BFS(ownerMapPos, targetMapPos);

		if (pathOnMap == null || pathOnMap.Count == 0)
		{
			DequeueMatching(itemWorldPos);
			isCheckingItem = false;
			yield break;
		}

		var worldPath = TranslateMapPathToWorld(pathOnMap);

		foreach (Vector3 step in worldPath)
		{
			if (Vector3.Distance(transform.position, itemWorldPos) <= radiusToItem)
				break;

			yield return StartCoroutine(MoveTo(step, 0.1f));
		}

		if (Vector3.Distance(transform.position, itemWorldPos) > radiusToItem)
		{
			yield return StartCoroutine(MoveTo(itemWorldPos, radiusToItem));
		}

		DequeueMatching(itemWorldPos);

		yield return new WaitForSeconds(checkWaitTime);

		isCheckingItem = false;
	}

	private IEnumerator MoveTo(Vector3 target, float stoppingDistance)
	{
		while (Vector3.Distance(transform.position, target) > Mathf.Max(0.05f, stoppingDistance))
		{
			transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
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
		foreach (var p in itemsToCheck)
		{
			if (Vector3.Distance(p, itemPosition) <= radiusToItem)
				return;
		}
		if (isCheckingItem && itemsToCheck.Count > 0)
		{
			var top = itemsToCheck.Peek();
			if (Vector3.Distance(top, itemPosition) <= radiusToItem)
				return;
		}

		itemsToCheck.Enqueue(itemPosition);
	}

	private void DequeueMatching(Vector3 itemPosition)
	{
		if (itemsToCheck.Count == 0) return;

		Vector3 first = itemsToCheck.Peek();
		if (Vector3.Distance(first, itemPosition) <= radiusToItem || first == itemPosition)
		{
			itemsToCheck.Dequeue();
			return;
		}

		Queue<Vector3> newQ = new Queue<Vector3>();
		bool removed = false;
		while (itemsToCheck.Count > 0)
		{
			var p = itemsToCheck.Dequeue();
			if (!removed && (Vector3.Distance(p, itemPosition) <= radiusToItem || p == itemPosition))
			{
				removed = true; // skip this one
				continue;
			}
			newQ.Enqueue(p);
		}
		itemsToCheck = newQ;
	}
}
