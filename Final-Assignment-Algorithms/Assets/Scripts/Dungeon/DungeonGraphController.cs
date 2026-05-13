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
                yield return SplitWait();
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
                        yield return SplitWait();
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
                yield return SplitWait();
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
            RectInt roomToDelete = dungeonGenerator.GetDoneRooms()[0];

            //get the smallest room
            for (int i = 0; i < dungeonGenerator.GetDoneRooms().Count; i++)
            {
                if (dungeonGenerator.GetDoneRooms()[i].width * dungeonGenerator.GetDoneRooms()[i].height < roomToDelete.width * roomToDelete.height)
                {
                    roomToDelete = dungeonGenerator.GetDoneRooms()[i];
                }
                else
                {
                    continue;
                }
            }

            visited.Add(roomToDelete);

            //yield return StartCoroutine(CheckGraph());

            var deletable = bfs.GraphSearch(roomGraph, visited);

            if (deletable) //delete room
            {
                Debug.Log(visited.Count + " " + roomGraph.ReturnGraphLength());
                foreach (RectInt door in roomGraph.ReturnAdjacents(roomToDelete))
                {
                    dungeonGenerator.GetDoors().Remove(door);
                    roomGraph.RemoveNode(door);
                }
                roomGraph.RemoveNode(roomToDelete);
                dungeonGenerator.GetDoneRooms().Remove(roomToDelete);
                Debug.Log("deleted");
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
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


    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator RemoveCycles()
    {
        RectInt firstroom = roomGraph.ReturnNodesList()[0];
        Stack<RectInt> stack = new();
        stack.Push(firstroom);

        visited.Add(firstroom);
        while (stack.Count > 0)
        {
            RectInt currentRoom = stack.Pop();
            checkedRoom = currentRoom;
            //Debug.Log(currentRoom);
            if ((currentRoom.width == 1 || currentRoom.height == 1) && visited.Contains(roomGraph.ReturnAdjacents(currentRoom)[0]) && visited.Contains(roomGraph.ReturnAdjacents(currentRoom)[1]))
            {
                dungeonGenerator.GetDoors().Remove(currentRoom);
                roomGraph.RemoveNode(currentRoom);
                continue;
            }
            if (splitType != SplitType.instant)
            {
                yield return SplitWait();
            }
            foreach (RectInt neighbor in roomGraph.ReturnAdjacents(currentRoom))
            {
                if (visited.Contains(neighbor)) continue;
                stack.Push(neighbor);
                visited.Add(neighbor);
            }
        }
        visited.Clear();
        //Debug.Log("There are " + roomGraph.ReturnRooms().Count + "Nodes");
        //Debug.Log(visited.Count + "Has been checked");
        checkedRoom = new();
        if (autoContinue) onScriptComplete?.Invoke();
    }

    public Graph<RectInt> GetRoomGraph()
    {
        return roomGraph;
    }
}
