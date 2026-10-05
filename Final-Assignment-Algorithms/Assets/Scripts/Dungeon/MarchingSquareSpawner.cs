using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquareSpawner : DungeonSettings
{
    private DungeonGenerator dGen;
    private TileMapGenerator tileMapGenerator;

    private int[,] _tileMap;

    [SerializeField]
    private GameObject[] wallAssets;

    [SerializeField]
    private Transform wallParent;

    private HashSet<Vector3> wallSpawns = new();

    private RectInt marchingSquare;
    private void Start()
    {
        dGen = GetComponent<DungeonGenerator>();
        tileMapGenerator = GetComponent<TileMapGenerator>();
    }

    private void Update()
    {
        if (marchingSquare != RectInt.zero) AlgorithmsUtils.DebugRectInt(marchingSquare, Color.yellow);
    }

    public void StartSpawningWalls()
    {
        StartCoroutine(SpawnAssets());
    }

    [Button]
    private IEnumerator SpawnAssets()
    {
        _tileMap = tileMapGenerator.GetTileMap();
        int rows = _tileMap.GetLength(0);
        int cols = _tileMap.GetLength(1);

        for (int i = 0; i < rows - 1; i++)
        {
            for (int j = 0; j < cols - 1; j++)
            {
                marchingSquare = new RectInt(new Vector2Int(j, i), new Vector2Int(2, 2));

                int binaryCase = _tileMap[i, j + 1] * 1 + _tileMap[i + 1, j + 1] * 2 + _tileMap[i + 1, j] * 4 + _tileMap[i, j] * 8;
                if (wallAssets[binaryCase] != null)
                {
                    Instantiate(wallAssets[binaryCase], new Vector3(j + 1, 0, i + 1), Quaternion.identity, wallParent);
                    wallSpawns.Add(new Vector3(j + 1, 0, i + 1));
                }
                //Debug.Log(binaryCase);
                if (waitType != WaitType.instant) yield return Wait();
            }
        }
        marchingSquare = RectInt.zero;
        if (autoContinue) onScriptComplete?.Invoke();
    }

    public Transform GetWallParent()
    {
        return wallParent;
    }

    public HashSet<Vector3> GetWallSpawns()
    {
        return wallSpawns;
    }

    public void ResetHashSet()
    {
        wallSpawns.Clear();
    }
}
