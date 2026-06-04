using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodfillSpawner : DungeonSettings
{
    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private Transform floorParent;

    private DungeonGenerator dGen;
    private TileMapGenerator tileMapGenerator;

    public List<Vector2Int> test = new();

    private Vector2Int[] dir =
    {
        new (1, 0),
        //new(1,-1),
        new (0, -1),
        //new(-1,-1),
        new(-1, 0),
        //new(-1,1),
        new (0, 1),
        //new(1,1),

    };

    private int[,] _tileMap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dGen = GetComponent<DungeonGenerator>();
        tileMapGenerator = GetComponent<TileMapGenerator>();
    }

    public void StartSpawningFloors()
    {
        StartCoroutine(FloodfillAlgorithm());
        //HashSet<Vector2Int> visited = new();
        //RecursiveFloodFill(tileMapGenerator.GetTileMap(), dGen.GetDoors()[0].position, visited);
    }

    private List<Vector2Int> GetNeighbors(Vector2Int Node)
    {
        List<Vector2Int> neighbors = new();
        for (int i = 0; i < dir.Length; i++)
        {
            neighbors.Add(Node + dir[i]);
        }
        return neighbors;
    }

    [Button]
    private IEnumerator FloodfillAlgorithm()
    {
        _tileMap = tileMapGenerator.GetTileMap();
        Vector2Int doorCenter = dGen.GetDoors()[0].position;
        Debug.Log(doorCenter);
        Queue<Vector2Int> queue = new();
        queue.Enqueue(doorCenter);

        HashSet<Vector2Int> visited = new();
        visited.Add(doorCenter);
        while (queue.Count > 0)
        {
            Vector2Int currentNode = queue.Dequeue();
            //Debug.Log(currentRoom);
            Instantiate(floorPrefab, new Vector3(currentNode.x + 0.5f, 0, currentNode.y + 0.5f), Quaternion.identity, floorParent);
            if (waitType != WaitType.instant) yield return Wait();
            foreach (Vector2Int neighbor in GetNeighbors(currentNode))
            {
                if (visited.Contains(neighbor) || _tileMap[neighbor.y, neighbor.x] == 1) continue;
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
        if (autoContinue) onScriptComplete?.Invoke();
    }

    private Vector2Int RecursiveFloodFill(int[,] _tileMap, Vector2Int currentNode, HashSet<Vector2Int> visited)
    {
        visited.Add(currentNode);
        test.Add(currentNode);
        //Instantiate(floorPrefab, new Vector3(currentNode.x + 0.5f, 0, currentNode.y + 0.5f), Quaternion.identity, floorParent);
        foreach (Vector2Int neighbor in GetNeighbors(currentNode))
        {
            if (visited.Contains(neighbor) || _tileMap[neighbor.y, neighbor.x] == 1) continue;
            RecursiveFloodFill(_tileMap, neighbor, visited);
        }
        return currentNode;
    }
    public Transform GetFloorParent()
    {
        return floorParent;
    }
}
