using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraphGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Graph<Vector3> tileGraph;

    DungeonGenerator dGen;
    TileMapGenerator tileMapGenerator;

    int[,] _tileMap;

    [SerializeField]
    private bool draw;

    private Vector3[] dirs =
    {
        new (1, 0, 0),
        new(1, 0, -1),
        new (0, 0, -1),
        new(-1, 0, -1),
        new(-1,0, 0),
        new(-1,0, 1),
        new (0, 0, 1),
        new(1, 0, 1),

    };

    private void Start()
    {
        dGen = GetComponent<DungeonGenerator>();
        tileMapGenerator = GetComponent<TileMapGenerator>();
        tileGraph = new();
    }

    private void Update()
    {
        if (!draw) return;
        foreach (Vector3 tile in tileGraph.ReturnNodes())
        {
            DebugExtension.DebugPoint(tile, Color.cyan, 0.5f);
            foreach (Vector3 tileNeighbor in tileGraph.ReturnNodeAdjacents(tile))
            {
                Debug.DrawLine(tile, tileNeighbor, Color.yellow);
            }
        }
    }

    [Button]
    private IEnumerator GenerateGraph()
    {
        tileGraph.ClearGraph();
        _tileMap = tileMapGenerator.GetTileMap();
        Vector2 roomCenter = dGen.GetDoors()[0].position;
        Vector3 startNode = new Vector3(roomCenter.x, 0, roomCenter.y);
        Vector3 offset = new(0.5f, 0, 0.5f);

        Queue<Vector3> queue = new();
        queue.Enqueue(startNode);
        tileGraph.AddNode(startNode + offset);

        HashSet<Vector3> visited = new();
        visited.Add(startNode);
        while (queue.Count > 0)
        {
            Vector3 currentNode = queue.Dequeue();
            //Debug.Log(currentRoom);
            //if (dGen.GetSplitType() != DungeonGenerator.SplitType.instant) yield return dGen.SplitWait();
            foreach (Vector3 neighbor in GetNeighbors(currentNode))
            {
                if (visited.Contains(neighbor) || _tileMap[(int)neighbor.z, (int)neighbor.x] == 1) continue;
                tileGraph.AddNode(neighbor + offset);
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
        foreach (Vector3 tile in tileGraph.ReturnNodes())
        {
            foreach (Vector3 neighbor in GetNeighbors(tile))
            {
                if (!tileGraph.ReturnNodes().Contains(neighbor) || tileGraph.ReturnNodeAdjacents(tile).Contains(neighbor)) continue;
                tileGraph.AddEdge(tile, neighbor);
                if (dGen.GetSplitType() != DungeonGenerator.SplitType.instant) yield return dGen.SplitWait();
            }
        }
    }

    private List<Vector3> GetNeighbors(Vector3 node)
    {
        List<Vector3> neighbors = new();
        foreach (Vector3 dir in dirs)
        {
            neighbors.Add(node + dir);
        }
        return neighbors;
    }
}
