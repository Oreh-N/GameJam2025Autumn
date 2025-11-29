using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Map : MonoBehaviour
{
    [SerializeField] Vector3 mapStart = Vector3.zero;
	[SerializeField] Vector2 size;
	[SerializeField] bool show = false;
	Vector3Int cellSize = new Vector3Int(1, 1, 0);
    int[,] map;

	[SerializeField] int[] testPosMapToWorld = new int[2];
	[SerializeField] Vector3 testPosWorldToMap = new Vector3();

	private void Awake()
	{
		map = new int[(int)size.x, (int)size.y];
	}

	private void OnDrawGizmos()
	{
		if (!show) return;
		for (int x = 0; x < size.x; x++) { 
			for (int y = 0; y < size.y; y++)
			{
				Gizmos.color = new Color(1, 0, 0, 0.3f);
				Gizmos.DrawCube(mapStart + new Vector3(x, y, 0), cellSize);
			}
		}
		Gizmos.color = new Color(0, 0, 1, 0.3f);
		Gizmos.DrawCube(GetWorldPos(testPosMapToWorld[0], testPosMapToWorld[1]), cellSize);

		Gizmos.color = new Color(0, 1, 0, 0.3f);
		(int, int) posM = GetOnMapPos(testPosWorldToMap); 
		Gizmos.DrawCube(GetWorldPos(posM.Item1, posM.Item2), cellSize);
	}

	// Tested
	/// <summary>
	/// Converts maps coordinates to world position
	/// </summary>
	/// <param name="mapX"> Index x on map</param>
	/// <param name="mapY">Index y on map</param>
	/// <returns></returns>
	public Vector3 GetWorldPos(int mapX, int mapY)
	{
		var worldPos = new Vector3(mapX * cellSize.x + mapStart.x, mapY * cellSize.y + mapStart.y);
		return worldPos;
	}

	public (int, int) GetOnMapPos(Vector3 pos)
	{
		int[] mapPos = new int[2] { Mathf.RoundToInt(pos.x - mapStart.x), Mathf.RoundToInt(pos.y - mapStart.y) };
		mapPos[0] /= cellSize.x;
		mapPos[1] /= cellSize.y;

		return (mapPos[0], mapPos[1]);
	}

	/// <summary>
	/// Finds where wall tiles are placed and then set the map values to wall value
	/// </summary>
	private void SetWalls()
	{

	}
}
