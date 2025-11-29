using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI;

public class Map : MonoBehaviour
{
	enum Items { Empty, Walls }
	Vector3 mapStart = new Vector3(-48, -18, 0);
	Vector2 size = new Vector2(43, 34);
	[SerializeField] bool showGizmo = false;
	public static Vector3Int cellSize = new Vector3Int(1, 1, 0);
	int[,] map;

	[SerializeField] int[] testPosMapToWorld = new int[2];
	[SerializeField] Vector3 testPosWorldToMap = new Vector3();
	[SerializeField] Tilemap wallTilemap;
	[SerializeField] Transform target;
	[SerializeField] bool showMapGizmo = false;


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
	{   // +1 because the index starts from 0 (we don't want that)
		var worldPos = new Vector3((mapX) * cellSize.x + mapStart.x, (mapY) * cellSize.y + mapStart.y);
		return worldPos;
	}

	public (int, int) GetWorldToMapPos(Vector3 pos)
	{
		int x = (int)((pos.x - mapStart.x) / cellSize.x);
		int y = (int)((pos.y - mapStart.y) / cellSize.y);
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
}
