using NaughtyAttributes;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class TileMapGenerator : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onTileMapGenerated;

    [SerializeField]
    private bool autoContinue = false;

    private DungeonGenerator dungeonGenerator;

    private int[,] _tileMap;

    private void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
    }

    [Button]
    public void GenerateTileMap()
    {
        int[,] tileMap = new int[dungeonGenerator.GetDungeonBounds().height + 2, dungeonGenerator.GetDungeonBounds().width + 2];
        int rows = tileMap.GetLength(0);
        int cols = tileMap.GetLength(1);

        foreach (RectInt room in dungeonGenerator.GetDoneRooms())
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                tileMap[room.yMin, x] = 1;
                tileMap[room.yMax - 1, x] = 1;
            }
            for (int y = room.yMin; y < room.yMax; y++)
            {
                tileMap[y, room.xMin] = 1;
                tileMap[y, room.xMax - 1] = 1;
            }
        }

        foreach (RectInt door in dungeonGenerator.GetDoors())
        {
            if (door.width > door.height)
            {
                for (int i = 0; i < door.width; i++)
                {
                    tileMap[door.y, door.x + i] = 0;
                }
            }
            else
            {
                for (int i = 0; i < door.height; i++)
                {
                    tileMap[door.y + i, door.x] = 0;
                }
            }
        }

        _tileMap = tileMap;
        Debug.Log("Tilemap generated");

        if (autoContinue) onTileMapGenerated?.Invoke();
    }

    public string ToString(bool flip)
    {
        if (_tileMap == null) return "Tile map not generated yet.";

        int rows = _tileMap.GetLength(0);
        int cols = _tileMap.GetLength(1);

        var sb = new StringBuilder();

        int start = flip ? rows - 1 : 0;
        int end = flip ? -1 : rows;
        int step = flip ? -1 : 1;

        for (int i = start; i != end; i += step)
        {
            for (int j = 0; j < cols; j++)
            {
                sb.Append((_tileMap[i, j] == 0 ? '0' : '#')); //Replaces 1 with '#' making it easier to visualize
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public int[,] GetTileMap()
    {
        return _tileMap.Clone() as int[,];
    }

    public void ResetTileMap()
    {
        _tileMap = null;
    }

    [Button]
    public void PrintTileMap()
    {
        Debug.Log(ToString(true));
    }


}
