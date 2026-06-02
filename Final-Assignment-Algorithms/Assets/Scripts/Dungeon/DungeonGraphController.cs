using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGraphController : DungeonSettings
{
    private DungeonGenerator dungeonGenerator;
    private DungeonAddAssets dungeonAddAssets;

    private Graph<RectInt> roomGraph;
    private BFS<RectInt> bfs;

    private RectInt checkedRoom;

    private HashSet<RectInt> visited = new();

    private bool canRemove = true;

    [Range(0, 100)]
    [SerializeField]
    private float deletePercent = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        dungeonAddAssets = GetComponent<DungeonAddAssets>();
        roomGraph = new();
        bfs = new();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (RectInt room in roomGraph.ReturnNodesList())
        {
            DebugExtension.DebugWireSphere(new Vector3(room.center.x, 0, room.center.y), Color.cyan);
            foreach (RectInt door in roomGraph.ReturnAdjacents(room))
            {
                Debug.DrawLine(new Vector3(room.center.x, 0, room.center.y), new Vector3(door.center.x, 0, door.center.y), Color.yellow);
            }
        }
        if (checkedRoom.width > 0) AlgorithmsUtils.DebugRectInt(checkedRoom, Color.white, 0, false, 3);

        foreach (RectInt visitedroom in visited)
        {
            AlgorithmsUtils.DebugRectInt(visitedroom, Color.yellow, 0, false, 2);
        }
    }

    public void StartDungeonGraphGeneration()
    {
        StartCoroutine(GenerateGraph());
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator GenerateGraph()
    {
        roomGraph = new();
        foreach (RectInt room in dungeonGenerator.GetDoneRooms())
        {
            roomGraph.AddNode(room);
            if (splitType != SplitType.instant)
            {
                yield return Wait();
            }
        }
        for (int i = 0; i < dungeonGenerator.GetDoneRooms().Count; i++)
        {
            for (int j = 0; j < dungeonGenerator.GetDoors().Count; j++)
            {
                if (AlgorithmsUtils.Intersects(dungeonGenerator.GetDoneRooms()[i], dungeonGenerator.GetDoors()[j]))
                {
                    roomGraph.AddEdge(dungeonGenerator.GetDoneRooms()[i], dungeonGenerator.GetDoors()[j]);
                    if (splitType != SplitType.instant)
                    {
                        yield return Wait();
                    }
                }
            }
        }
        if (autoContinue) StartCoroutine(RemoveRooms());
    }
    private IEnumerator CheckGraph()
    {
        RectInt firstroom = roomGraph.ReturnNodesList()[0];
        if (visited.Contains(firstroom))
        {
            firstroom = roomGraph.ReturnNodesList()[1];
        }
        Queue<RectInt> queue = new();
        queue.Enqueue(firstroom);

        visited.Add(firstroom);
        while (queue.Count > 0)
        {
            RectInt currentRoom = queue.Dequeue();
            checkedRoom = currentRoom;
            //Debug.Log(currentRoom);
            if (splitType != SplitType.instant)
            {
                yield return Wait();
            }
            foreach (RectInt neighbor in roomGraph.ReturnAdjacents(currentRoom))
            {
                if (visited.Contains(neighbor)) continue;
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
        checkedRoom = new();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator RemoveRooms()
    {
        int deleteAmount = dungeonGenerator.GetDoneRooms().Count - Mathf.FloorToInt(dungeonGenerator.GetDoneRooms().Count * (deletePercent / 100));
        int initialRoomCount = dungeonGenerator.GetDoneRooms().Count;
        canRemove = true;

        while (canRemove == true && dungeonGenerator.GetDoneRooms().Count > deleteAmount)
        {
            RectInt roomToDelete = GetSmallestRoom(dungeonGenerator.GetDoneRooms()[0]);

            visited.Add(roomToDelete);

            var deletable = bfs.GraphSearch(roomGraph, visited);

            if (deletable) //delete room
            {
                DeleteRoom(roomToDelete);
                if (splitType != SplitType.instant)
                {
                    yield return Wait();
                }
            }
            else // keep room
            {
                Debug.Log("Can't be deleted");
                Debug.Log("tried to delete" + roomToDelete);
                Debug.Log(visited.Count + "  " + roomGraph.ReturnGraphLength());
                AlgorithmsUtils.DebugRectInt(roomToDelete, Color.cyan, 3, false, 3);
                canRemove = false;
            }
            visited.Clear();

        }
        Debug.Log("done deleting");
        Debug.Log("Deleted " + (initialRoomCount - dungeonGenerator.GetDoneRooms().Count) + " rooms");
        if (autoContinue) StartCoroutine(RemoveCycles());
    }

    private RectInt GetSmallestRoom(RectInt room)
    {
        for (int i = 0; i < dungeonGenerator.GetDoneRooms().Count; i++)
        {
            if (dungeonGenerator.GetDoneRooms()[i].width * dungeonGenerator.GetDoneRooms()[i].height < room.width * room.height)
            {
                room = dungeonGenerator.GetDoneRooms()[i];
            }
            else
            {
                continue;
            }
        }
        return room;
    }

    private void DeleteRoom(RectInt room)
    {
        Debug.Log(visited.Count + " " + roomGraph.ReturnGraphLength());
        foreach (RectInt door in roomGraph.ReturnAdjacents(room))
        {
            dungeonGenerator.GetDoors().Remove(door);
            roomGraph.RemoveNode(door);
        }
        roomGraph.RemoveNode(room);
        dungeonGenerator.GetDoneRooms().Remove(room);
        Debug.Log("deleted");

    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator RemoveCycles()
    {
        Graph<RectInt> dfsGraph = DFSGraphMaker();

        foreach (RectInt node in dfsGraph.ReturnNodesList())
        {
            if (dungeonGenerator.GetDoors().Contains(node) && dfsGraph.ReturnAdjacents(node).Count == 1)
            {
                dungeonGenerator.GetDoors().Remove(node);
                roomGraph.RemoveNode(node);
                if (splitType != SplitType.instant) yield return Wait();
            }
        }
        if (autoContinue) onScriptComplete?.Invoke();
    }
    private Graph<RectInt> DFSGraphMaker()
    {
        RectInt firstroom = roomGraph.ReturnNodesList()[0];
        Graph<RectInt> dfsGraph = new();
        Stack<RectInt> stack = new();
        stack.Push(firstroom);

        visited.Add(firstroom);
        while (stack.Count > 0)
        {
            RectInt currentRoom = stack.Pop();
            checkedRoom = currentRoom;
            foreach (RectInt neighbor in roomGraph.ReturnAdjacents(currentRoom))
            {
                if (visited.Contains(neighbor)) continue;
                stack.Push(neighbor);
                visited.Add(neighbor);
                dfsGraph.AddEdge(currentRoom, neighbor);
            }
        }
        visited.Clear();
        checkedRoom = new();
        return dfsGraph;
    }

    public Graph<RectInt> GetRoomGraph()
    {
        return roomGraph;
    }
}
