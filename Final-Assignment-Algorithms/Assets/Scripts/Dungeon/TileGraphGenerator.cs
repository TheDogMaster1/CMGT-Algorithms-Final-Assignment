using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraphGenerator : DungeonSettings
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Graph<Vector3> tileGraph;

    DungeonGenerator dGen;
    TileMapGenerator tileMapGenerator;

    [SerializeField]
    private GameObject player;

    int[,] _tileMap;

    [SerializeField]
    private bool draw;

    [SerializeField]
    private bool fourDirections = false;

    private Vector3[] dirs8 =
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

    private Vector3[] dirs4 =
    {
        new(1, 0, 0),
        new(0, 0, -1),
        new(-1, 0, 0),
        new(0, 0, 1),
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
        foreach (Vector3 tile in tileGraph.ReturnNodesList())
        {
            DebugExtension.DebugPoint(tile, Color.cyan, 0.5f);
            foreach (Vector3 tileNeighbor in tileGraph.ReturnAdjacents(tile))
            {
                Debug.DrawLine(tile, tileNeighbor, Color.yellow);
            }
        }
    }

    [Button(enabledMode: EButtonEnableMode.Always)]
    public void StartTileGraphGen()
    {
        StartCoroutine(GenerateGraph());
    }

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
            if (waitType != WaitType.instant) yield return Wait();
            foreach (Vector3 neighbor in GetNeighbors(currentNode))
            {
                if (_tileMap[(int)neighbor.z, (int)neighbor.x] != 1 && !tileGraph.ReturnAdjacentsHashSet(currentNode + offset).Contains(neighbor + offset))
                {
                    tileGraph.AddEdge(currentNode + offset, neighbor + offset);
                }
                if (visited.Contains(neighbor) || _tileMap[(int)neighbor.z, (int)neighbor.x] == 1) continue;
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }

        player.transform.position = new Vector3(dGen.GetDoneRooms()[0].center.x, 1, dGen.GetDoneRooms()[0].center.y);
    }

    private List<Vector3> GetNeighbors(Vector3 node)
    {
        List<Vector3> neighbors = new();
        if (!fourDirections)
        {
            foreach (Vector3 dir in dirs8)
            {
                neighbors.Add(node + dir);
            }
        }
        else
        {
            foreach (Vector3 dir in dirs4)
            {
                neighbors.Add(node + dir);
            }
        }
        return neighbors;
    }

    public Graph<Vector3> GetGraph()
    {
        return tileGraph;
    }

    public void ResetGraph()
    {
        tileGraph.ClearGraph();
    }
}
