using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI;

public class Map : MonoBehaviour
{
	public enum Items { Empty, Walls }
	[SerializeField] Tilemap wallTilemap;
	// Map
	public static Vector3Int cellSize = new Vector3Int(1, 1, 0);
	static Vector3 mapStart = new Vector3(-47.5f, -18.5f, 0);
	public static Vector2 size = new Vector2(83, 38);
	int[,] map;
	//

	// For testing
	[SerializeField] bool showGizmo = false;
	[SerializeField] bool showMapGizmo = false;
	[SerializeField] Transform target;
	[SerializeField] int[] testPosMapToWorld = new int[2];
	[SerializeField] Vector3 testPosWorldToMap = new Vector3();
	//

	private void Awake()
	{
		map = new int[(int)size.x, (int)size.y];
		if (wallTilemap != null)
		{
			SetWalls();
		}
	}

	private void OnDrawGizmos()
	{
		if (!showGizmo) return;
		for (int x = 0; x < size.x; x++)
		{
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


		Gizmos.color = new Color(0.2f, 0.1f, 0.5f, 0.3f);
		(int, int) posT = GetWorldToMapPos(target.position);
		Gizmos.DrawCube(GetMapToWorldPos(posT.Item1, posT.Item2), cellSize);

		if (showMapGizmo)
			ShowMapGizmo();
	}

	public void ShowMapGizmo()
	{
		BoundsInt bounds = wallTilemap.cellBounds;
		foreach (var pos in bounds.allPositionsWithin)
		{
			TileBase tile = wallTilemap.GetTile(pos);
			if (tile != null)
			{
				Gizmos.color = new Color(0.4f, 0, 0.2f, 0.3f);
				var pos2 = GetWorldToMapPos(pos);
				Gizmos.DrawCube(GetMapToWorldPos(pos2.Item1, pos2.Item2), cellSize);
			}
		}
	}

	// Tested
	/// <summary>
	/// Converts maps coordinates to world position
	/// </summary>
	/// <param name="mapX"> Index x on map</param>
	/// <param name="mapY">Index y on map</param>
	/// <returns></returns>
	public Vector3 GetMapToWorldPos(int mapX, int mapY)
	{	// +1 because the index starts from 0 (we don't want that)
		var worldPos = new Vector3((mapX+1) * cellSize.x + mapStart.x, (mapY+1) * cellSize.y + mapStart.y);
		return worldPos;
	}

	public (int, int) GetWorldToMapPos(Vector3 pos)
	{
		int x = (int)((pos.x - mapStart.x) / cellSize.x);
		int y = (int)((pos.y - mapStart.y) / cellSize.y);
		if (x >= size.x || x < 0 || y < 0 || y >= size.y)
		{ x = 0; y = 0; Debug.Log("Out of map position passed"); }
		return (x, y);
	}

	public int WhatOnPos(Vector3 pos)
	{
		var i = GetWorldToMapPos(pos);
		return map[i.Item1, i.Item2];
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
}
