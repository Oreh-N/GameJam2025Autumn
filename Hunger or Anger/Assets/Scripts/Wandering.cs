using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Wandering : MonoBehaviour
{
    [SerializeField] Transform[] points;
	public List<Vector2Int> path = new List<Vector2Int>();
	int curPoint = 0;
	int curStep = 0;
	int speed = 1;
	float pointRadius = 0.05f;


	private void Start()
	{
		StartNewPath();
	}


	IEnumerator FollowPath()
	{
		for (int i = 1; i < path.Count; i++)
		{
			Vector3 targetPos = Map.Instance.MapToWorld(path[i].x, path[i].y);

			while (Vector3.Distance(transform.position, targetPos) > pointRadius)
			{
				var dir = GetDirection(Map.Instance.WorldToMap(points[curPoint].transform.position), path[i]);
				TryMove(dir);
				yield return null;
			}
		}

		curPoint = (curPoint + 1) % points.Length;

		yield return new WaitForSeconds(0.5f);
		StartNewPath();
	}

	private Vector2 GetDirection(Vector2Int vector2Int, Vector2Int targetPos)
	{
		Vector2 dir;
		dir = targetPos - vector2Int;
		if (dir != Vector2.zero)
			dir = dir.normalized;

		return dir;
	}

	private void TryMove(Vector2 dir)
	{
		dir *= speed * Time.deltaTime;
		var nxt_pos = transform.position + new Vector3(dir.x * Map.cellSize.x, dir.y * Map.cellSize.y, 0);
		if (Map.Instance.GetMapValue(nxt_pos) == (int)Map.Items.Empty || Map.Instance.GetMapValue(nxt_pos) == (int)Map.Items.Carpets)
		{ 
			transform.position = nxt_pos;
			curStep++;
		}

	}

	public void StartNewPath()
	{
		path = BFS(Map.Instance.WorldToMap(transform.position), Map.Instance.WorldToMap(points[curPoint].transform.position));

		if (path != null && path.Count > 0)
		{ StartCoroutine(FollowPath()); }
	}

	public List<Vector2Int> BFS(Vector2Int start, Vector2Int goal)
	{
		int w = Map.Instance.size.x;
		int h = Map.Instance.size.y;

		Queue<Vector2Int> q = new Queue<Vector2Int>();
		bool[,] visited = new bool[w, h];
		Vector2Int[,] parent = new Vector2Int[w, h];

		q.Enqueue(start);
		visited[start.x, start.y] = true;

		Vector2Int[] dirs = {
		new Vector2Int(1, 0),
		new Vector2Int(-1, 0),
		new Vector2Int(0, 1),
		new Vector2Int(0, -1)
	};

		while (q.Count > 0)
		{
			Vector2Int cur = q.Dequeue();

			if (cur == goal)
				return BuildPath(parent, start, goal);

			foreach (var d in dirs)
			{
				Vector2Int next = cur + d;

				if (next.x < 0 || next.y < 0 || next.x >= w || next.y >= h)
					continue;

				if (Map.Instance.GetMapValue(next.x, next.y) == 1)
					continue;

				if (!visited[next.x, next.y])
				{
					visited[next.x, next.y] = true;
					parent[next.x, next.y] = cur;
					q.Enqueue(next);
				}
			}
		}

		return null;
	}


	List<Vector2Int> BuildPath(Vector2Int[,] parent, Vector2Int start, Vector2Int goal)
	{
		List<Vector2Int> path = new List<Vector2Int>();
		Vector2Int cur = goal;

		while (cur != start)
		{
			path.Add(cur);
			cur = parent[cur.x, cur.y];
		}

		path.Add(start);
		path.Reverse();
		return path;
	}
}
