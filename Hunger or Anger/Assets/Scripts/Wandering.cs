using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Wandering : MonoBehaviour
{
	public List<Transform> points = new List<Transform>();
	public bool isMoving;
	public int pointIndex;
	public float speed;


	private void Start()
	{
		StartMoving();
	}

	private void StartMoving()
	{
		pointIndex = 0;
		isMoving = true;
	}

	private void Update()
	{
		if (!isMoving) return;
		transform.position = Vector3.MoveTowards(transform.position, points[pointIndex].position, Time.deltaTime*speed);
		var dist = Vector3.Distance(transform.position, points[pointIndex].position);
		if (dist < 0.05f) 
		{ 
			pointIndex = (pointIndex+1) % points.Count;
		}
	}


	//	[SerializeField] bool allowMovement = false;
	//	[SerializeField] Transform[] points;
	//	public List<Vector2Int> path = new List<Vector2Int>();
	//	int curPoint = 0;
	//	int speed = 1;
	//	float pointRadius = 0.05f;
	//	Vector2Int size = Vector2Int.zero;


	//	private void Start()
	//	{
	//		StartPathToNxtPoint();
	//		size = Map.size;
	//	}

	//	public void StartPathToNxtPoint()
	//	{
	//		if (!allowMovement) return;
	//		path = BFS(Map.Instance.WorldToMap(transform.position), Map.Instance.WorldToMap(points[curPoint].transform.position));

	//		if (path != null && path.Count > 0)
	//		{ StartCoroutine(FollowPath()); }
	//	}

	//	IEnumerator FollowPath()
	//	{
	//		foreach (var step in path)
	//		{
	//			Vector3 stepMapPos = Map.Instance.MapToWorld(step.x, step.y);
	//			var dir = GetDirection(Map.Instance.WorldToMap(transform.position), step);
	//			TryMove(dir);
	//			yield return new WaitForSeconds(0.3f);

	//			if (Vector3.Distance(transform.position, stepMapPos) < pointRadius)
	//				break;
	//		}

	//		curPoint++;
	//		StartPathToNxtPoint();

	//	}

	//	private Vector2 GetDirection(Vector2Int start, Vector2Int end)
	//	{
	//		Vector2 dir;
	//		dir = end - start;
	//		if (dir != Vector2.zero)
	//			dir = dir.normalized;

	//		return dir;
	//	}

	//	private void TryMove(Vector2 dir)
	//	{
	//		Vector2Int cellSize = new Vector2Int(1,1);
	//		dir *= speed;
	//		var nxt_pos = transform.position + new Vector3(dir.x * cellSize.x, dir.y * cellSize.y, 0);
	//		if (Map.Instance.GetMapValue(nxt_pos) == (int)Items.Empty || Map.Instance.GetMapValue(nxt_pos) == (int)Items.Carpets)
	//		{
	//			transform.position = nxt_pos;
	//		}

	//	}


	//	public List<Vector2Int> BFS(Vector2Int start, Vector2Int goal)
	//	{
	//		int w = size.x;
	//		int h = size.y;

	//		Queue<Vector2Int> q = new Queue<Vector2Int>();
	//		bool[,] visited = new bool[w, h];
	//		Vector2Int[,] parent = new Vector2Int[w, h];

	//		q.Enqueue(start);
	//		visited[start.x, start.y] = true;

	//		Vector2Int[] dirs = {
	//		new Vector2Int(1, 0),
	//		new Vector2Int(-1, 0),
	//		new Vector2Int(0, 1),
	//		new Vector2Int(0, -1)
	//	};

	//		while (q.Count > 0)
	//		{
	//			Vector2Int cur = q.Dequeue();

	//			if (cur == goal)
	//				return BuildPath(parent, start, goal);

	//			foreach (var d in dirs)
	//			{
	//				Vector2Int next = cur + d;

	//				if (next.x < 0 || next.y < 0 || next.x >= w || next.y >= h)
	//					continue;

	//				if (Map.Instance.GetMapValue(next.x, next.y) == 1)
	//					continue;

	//				if (!visited[next.x, next.y])
	//				{
	//					visited[next.x, next.y] = true;
	//					parent[next.x, next.y] = cur;
	//					q.Enqueue(next);
	//				}
	//			}
	//		}

	//		return null;
	//	}


	//	List<Vector2Int> BuildPath(Vector2Int[,] parent, Vector2Int start, Vector2Int goal)
	//	{
	//		List<Vector2Int> path = new List<Vector2Int>();
	//		Vector2Int cur = goal;

	//		while (cur != start)
	//		{
	//			path.Add(cur);
	//			cur = parent[cur.x, cur.y];
	//		}

	//		path.Add(start);
	//		path.Reverse();
	//		return path;
	//	}
}
