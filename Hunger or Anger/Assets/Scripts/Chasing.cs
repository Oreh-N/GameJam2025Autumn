using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chasing : MonoBehaviour
{
	public static Chasing Instance;

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

	public Vector3 FindClosestPos(Vector3 curPos, List<Vector3> path)
	{
		Vector3 closestPos = new Vector3();
		float minDist = 1000000;

		for (int i = 0; i < path.Count; i++)
		{
			var dist = Vector3.Distance(transform.position, path[i]);
			if (dist < minDist) 
			{ 
				minDist = dist;
				closestPos = path[i];
			}
		}

		return closestPos;
	}


	public List<Vector2Int> BFS(Vector2Int start, Vector2Int goal)
	{
		int w = Map.size.x;
		int h = Map.size.y;

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
