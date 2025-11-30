using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI;

	public enum Items { Empty, Walls, Furniture, Carpets }
public class Map : MonoBehaviour
{
	public static Map Instance;

	public static Vector3 mapStart = new Vector3(-11.5f, -6.5f,0);
	public static Vector2Int size = new Vector2Int(48, 27);
	public static readonly Vector3Int cellSize = new Vector3Int(1, 1, 0);

	[SerializeField] Tilemap wallTilemap;
	[SerializeField] Tilemap furnitureTilemap;

	[SerializeField] bool showGrid = false;
	[SerializeField] bool showMapGizmo = false; // only works during the game 
	[SerializeField] Transform targetForGizmo;

	int[,] map = new int[size.x, size.y];

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

	private void Start()
	{
		if (wallTilemap && furnitureTilemap)
		{
			CopyTilemapToMap(furnitureTilemap, (int)Items.Furniture);
			CopyTilemapToMap(wallTilemap, (int)Items.Walls);
		}
	}

	public int GetMapValue(Vector3 pos)
	{
		var i = WorldToMap(pos);
		if (map == null)
		{
			Debug.Log("Map not initialized");
			return -1;
		}
		if (i.x >= size.x || i.y >= size.y || i.x < 0 || i.y < 0)
		{
			Debug.Log("Out of range (GetMapValue)");
			return -1;
		}
			
		return map[i.x, i.y];
	}

	public int GetMapValue(int x, int y)
	{
		if (x >= size.x || y >= size.y || x < 0 || y < 0)
		{
			Debug.Log("Out of range (GetMapValue)");
			return -1;
		}
		return map[x, y];
	}

	/// Convert map index -> world position (center of the cell)
	public Vector3 MapToWorld(int x, int y)
	{
		return mapStart + new Vector3(x, y, 0);
	}

	/// Convert world position -> map index
	public Vector2Int WorldToMap(Vector3 pos)
	{
		int x = Mathf.FloorToInt(pos.x - mapStart.x + 0.5f);
		int y = Mathf.FloorToInt(pos.y - mapStart.y + 0.5f);


		if (x >= size.x || y >= size.y || x < 0 || y < 0)
		{
			Debug.Log("Out of range (WorldToMap)");
			return new Vector2Int(0,0);
		}

		return new Vector2Int(x,y);
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
			Vector2Int v = WorldToMap(worldPos);

			// Set wall
			if (v.x >= 0 && v.y >= 0 && v.x < size.x && v.y < size.y)
				map[v.x, v.y] = val;
		}
	}

	private void OnDrawGizmos()
	{

		if (!showGrid) return;

		Gizmos.color = new Color(0.8f, 0, 0, 0.3f);
		for (int x = 0; x < size.x; x++)
			for (int y = 0; y < size.y; y++)
			{
				Gizmos.DrawCube(MapToWorld(x, y), cellSize);
			}

		if (targetForGizmo)
		{
			Gizmos.color = new Color(0, 0.4f, 1f, 0.4f);
			Vector2Int v = WorldToMap(targetForGizmo.position);
			Gizmos.DrawCube(MapToWorld(v.x, v.y), cellSize);
		}

		if (showMapGizmo)
		{
			CopyTilemapToMap(furnitureTilemap, 2);
			CopyTilemapToMap(wallTilemap, 1);
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
				if (map[x, y] == 0) continue;	// will through an error if used not during the game

				Vector3 world = MapToWorld(x, y);
				Gizmos.DrawCube(world, cellSize);
			}
		}
	}

}
