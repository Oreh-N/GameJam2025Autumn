using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerScript : MonoBehaviour
{
	public static OwnerScript Instance;

	Queue<Item> itemsToCheck = new Queue<Item>();
	int radiusToItem = 6;
	bool isCheckingItem = false;

	[SerializeField] float checkWaitTime = 1.2f;
	[SerializeField] float moveSpeed = 6f;

	public bool catIsNearby = false;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		var cat = collision.gameObject.GetComponent<CatScript>();

		if (cat)
		{ catIsNearby = true; }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		var cat = collision.gameObject.GetComponent<CatScript>();

		if (cat)
		{ catIsNearby = false; }
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
	}

	void Update()
	{
		if (!isCheckingItem && itemsToCheck.Count > 0)
		{
			Item next = itemsToCheck.Peek();
			StartCoroutine(GoCheckItem(next));
		}
		if (itemsToCheck.Count == 0 && !isCheckingItem)
			GetComponent<Wandering>().isMoving = true;
	}

	private IEnumerator GoCheckItem(Item item)
	{
		isCheckingItem = true;
		GetComponent<Wandering>().isMoving = false;

		if (Vector3.Distance(transform.position, item.transform.position) <= radiusToItem)
		{
			DequeueMatching(item.transform.position);
			React(item);
			yield return new WaitForSeconds(checkWaitTime);

			isCheckingItem = false;
			yield break;
		}

		var ownerMapPos = Map.Instance.WorldToMap(transform.position);
		var targetMapPos = Map.Instance.WorldToMap(item.transform.position);
		var pathOnMap = Chasing.Instance.BFS(ownerMapPos, targetMapPos);

		if (pathOnMap == null || pathOnMap.Count == 0)
		{
			DequeueMatching(item.transform.position);
			isCheckingItem = false;
			yield break;
		}

		var worldPath = TranslateMapPathToWorld(pathOnMap);

		foreach (Vector3 step in worldPath)
		{
			if (Vector3.Distance(transform.position, item.transform.position) <= radiusToItem)
				break;

			yield return StartCoroutine(MoveTo(step, 0.1f));
		}

		if (Vector3.Distance(transform.position, item.transform.position) > radiusToItem)
		{
			yield return StartCoroutine(MoveTo(item.transform.position, radiusToItem));
		}

		DequeueMatching(item.transform.position);
		React(item);
		yield return new WaitForSeconds(checkWaitTime);

		isCheckingItem = false;
	}

	private void React(Item item)
	{
		var angerVal = item.angerVal;
		if (catIsNearby) angerVal*=2;

		UI.Instance.AddValueToAngerSlider(angerVal);
		UI.Instance.AddValueToAttantionSlider(item.attantionVal);
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

	public void CheckBrokenItem(Item item)
	{
		foreach (var p in itemsToCheck)
		{
			if (Vector3.Distance(p.transform.position, item.transform.position) <= radiusToItem)
				return;
		}
		if (isCheckingItem && itemsToCheck.Count > 0)
		{
			var top = itemsToCheck.Peek();
			if (Vector3.Distance(top.transform.position, item.transform.position) <= radiusToItem)
				return;
		}

		itemsToCheck.Enqueue(item);
	}

	private void DequeueMatching(Vector3 itemPosition)
	{
		if (itemsToCheck.Count == 0) return;

		Item first = itemsToCheck.Peek();
		if (Vector3.Distance(first.transform.position, itemPosition) <= radiusToItem || first.transform.position == itemPosition)
		{
			itemsToCheck.Dequeue();
			return;
		}

		Queue<Item> newQ = new Queue<Item>();
		bool removed = false;
		while (itemsToCheck.Count > 0)
		{
			var p = itemsToCheck.Dequeue();
			if (!removed && (Vector3.Distance(p.transform.position, itemPosition) <= radiusToItem || p.transform.position == itemPosition))
			{
				removed = true; // skip this one
				continue;
			}
			newQ.Enqueue(p);
		}
		itemsToCheck = newQ;
	}
}
