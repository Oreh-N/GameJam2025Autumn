using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI;

public class Map : MonoBehaviour
{
	enum Items { Empty, Walls}
    [SerializeField] Vector3 mapStart = Vector3.zero;
	[SerializeField] Vector2 size;
	[SerializeField] bool show = false;
	Vector3Int cellSize = new Vector3Int(1, 1, 0);
    int[,] map;

	[SerializeField] int[] testPosMapToWorld = new int[2];
	[SerializeField] Vector3 testPosWorldToMap = new Vector3();
	[SerializeField] Tilemap wallTilemap;

	private void Awake()
	{
		map = new int[(int)size.x, (int)size.y];
		if (wallTilemap != null)
		{
			SetWalls();
			PrintMap();
		}
	}

	private void OnDrawGizmos()
	{
		if (!show) return;
		for (int x = 0; x < size.x; x++) { 
			for (int y = 0; y < size.y; y++)
			{
				Gizmos.color = new Color(0.5f, 0, 0, 0.3f);
				Gizmos.DrawCube(mapStart + new Vector3(x, y, 0), cellSize);
			}
		}
		Gizmos.color = new Color(0, 0, 0.5f, 0.3f);
		Gizmos.DrawCube(GetMapToWorldPos(testPosMapToWorld[0], testPosMapToWorld[1]), cellSize);

		Gizmos.color = new Color(0, 0.5f, 0, 0.3f);
		(int, int) posM = GetWorldToMapPos(testPosWorldToMap); 
		Gizmos.DrawCube(GetMapToWorldPos(posM.Item1, posM.Item2), cellSize);

		//BoundsInt bounds = wallTilemap.cellBounds;
		//foreach (var pos in bounds.allPositionsWithin)
		//{
		//	TileBase tile = wallTilemap.GetTile(pos);
		//	if (tile != null)
		//	{
		//		Gizmos.color = new Color(0.4f, 0, 0.2f, 0.3f);
		//		var pos2 = GetWorldToMapPos(pos);
		//		Gizmos.DrawCube(GetMapToWorldPos(pos2.Item1, pos2.Item2), cellSize);
		//	}
		//}
	}

	// Tested
	/// <summary>
	/// Converts maps coordinates to world position
	/// </summary>
	/// <param name="mapX"> Index x on map</param>
	/// <param name="mapY">Index y on map</param>
	/// <returns></returns>
	public Vector3 GetMapToWorldPos(int mapX, int mapY)
	{
		var worldPos = new Vector3(mapX * cellSize.x + mapStart.x + 1, mapY * cellSize.y + mapStart.y + 1);
		return worldPos;
	}

	public (int, int) GetWorldToMapPos(Vector3 pos)
	{
		int x = Mathf.FloorToInt((pos.x - mapStart.x) / cellSize.x);
		int y = Mathf.FloorToInt((pos.y - mapStart.y) / cellSize.y);
		return (x, y);
	}

	/// <summary>
	/// Finds where wall tiles are placed and then set the map values to wall value
	/// </summary>
	private void SetWalls()
	{
		BoundsInt bounds = wallTilemap.cellBounds;
		Debug.Log(bounds + $"Cell in row count: {bounds.yMax}");

		foreach (var pos in bounds.allPositionsWithin)
		{
			TileBase tile = wallTilemap.GetTile(pos);
			if (tile != null)
			{
				(int, int) mapPos = GetWorldToMapPos(pos);
				map[mapPos.Item1, mapPos.Item2] = (int)Items.Walls;
			}
		}
	}

	private void PrintMap()
	{
		string row = string.Empty;
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				row += map[x, y].ToString();
			}
			Debug.Log($"x: {x};   " + row);
			row = string.Empty;
		}
	}
}
