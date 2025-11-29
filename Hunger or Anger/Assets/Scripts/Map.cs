using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI;

public class Map : MonoBehaviour
{
	public enum Items { Empty, Walls, Furniture, Carpets }

	[SerializeField] Vector3 mapStart = Vector3.zero;
	[SerializeField] Vector2Int size = new Vector2Int(83, 38);
	public static readonly Vector3Int cellSize = new Vector3Int(1, 1, 0);

	[SerializeField] Tilemap wallTilemap;
	[SerializeField] Tilemap furnitureTilemap;

	[SerializeField] bool showGrid = false;
	[SerializeField] bool showMapGizmo = false; // only works during the game 
	[SerializeField] Transform targetForGizmo;

	int[,] map;

	private void Awake()
	{
		map = new int[size.x, size.y];

		if (wallTilemap)
		{
			CopyTilemapToMap(furnitureTilemap, (int)Items.Furniture);
			CopyTilemapToMap(wallTilemap, (int)Items.Walls);
		}
	}

	public int GetMapValue(Vector3 pos)
	{
		var i = WorldToMap(pos);
		return map[i.Item1, i.Item2];
	}

	public int GetMapValue(int x, int y)
	{
		return map[x, y];
	}

	/// Convert map index -> world position (center of the cell)
	public Vector3 MapToWorld(int x, int y)
	{
		return mapStart + new Vector3(x, y, 0);
	}

	/// Convert world position -> map index
	public (int, int) WorldToMap(Vector3 pos)
	{
		int x = Mathf.FloorToInt(pos.x - mapStart.x + 0.5f);
		int y = Mathf.FloorToInt(pos.y - mapStart.y + 0.5f);

		return (Mathf.Clamp(x, 0, size.x - 1),
				Mathf.Clamp(y, 0, size.y - 1));
	}

	void CopyTilemapToMap(Tilemap tMap, int val)
	{
		BoundsInt bounds = tMap.cellBounds;

		foreach (var cell in bounds.allPositionsWithin)
		{
			TileBase tile = tMap.GetTile(cell);
			if (tile == null) continue;

			// Tilemap cell -> world center
			Vector3 worldPos = tMap.GetCellCenterWorld(cell);

			// World -> map cell
			(int x, int y) = WorldToMap(worldPos);

			// Set wall
			if (x >= 0 && y >= 0 && x < size.x && y < size.y)
				map[x, y] = val;
		}
	}

	private void OnDrawGizmos()
	{

		if (!showGrid) return;

		Gizmos.color = new Color(0.4f, 0, 0, 0.2f);
		for (int x = 0; x < size.x; x++)
			for (int y = 0; y < size.y; y++)
			{
				Gizmos.DrawCube(MapToWorld(x, y), cellSize);
			}

		if (targetForGizmo)
		{
			Gizmos.color = new Color(0, 0.4f, 1f, 0.4f);
			var (mx, my) = WorldToMap(targetForGizmo.position);
			Gizmos.DrawCube(MapToWorld(mx, my), cellSize);
		}

		if (showMapGizmo)
		{
			ShowMapGizmo();
		}
	}
	void ShowMapGizmo()
	{
		Gizmos.color = new Color(0.7f, 0, 0.4f, 0.3f);

		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				if (map[x, y] == 0) continue;

				Vector3 world = MapToWorld(x, y);
				Gizmos.DrawCube(world, cellSize);
			}
		}
	}

}
