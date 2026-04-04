using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGraphController : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;

    public DungeonGraph<RectInt> roomGraph;

    private RectInt checkedRoom;

    private HashSet<RectInt> visited = new();

    private bool canRemove = true;

    [Range(0, 100)]
    public float deletePercent = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        roomGraph = new();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (RectInt room in roomGraph.ReturnRooms())
        {
            DebugExtension.DebugWireSphere(new Vector3(room.center.x, 0, room.center.y), Color.cyan);
            foreach (RectInt door in roomGraph.ReturnRoomAdjacents(room))
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

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator GenerateGraph()
    {
        roomGraph = new();
        foreach (RectInt room in dungeonGenerator.doneRooms)
        {
            roomGraph.AddNode(room);
            if (dungeonGenerator.splitType != DungeonGenerator.SplitType.instant)
            {
                yield return dungeonGenerator.SplitWait();
            }
        }
        for (int i = 0; i < dungeonGenerator.doneRooms.Count; i++)
        {
            for (int j = 0; j < dungeonGenerator.doors.Count; j++)
            {
                if (AlgorithmsUtils.Intersects(dungeonGenerator.doneRooms[i], dungeonGenerator.doors[j]))
                {
                    roomGraph.AddEdge(dungeonGenerator.doneRooms[i], dungeonGenerator.doors[j]);
                    if (dungeonGenerator.splitType != DungeonGenerator.SplitType.instant)
                    {
                        yield return dungeonGenerator.SplitWait();
                    }
                }
            }
        }
    }
    //[Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator CheckGraph()
    {
        RectInt firstroom = roomGraph.ReturnRooms()[0];
        if (visited.Contains(firstroom))
        {
            firstroom = roomGraph.ReturnRooms()[1];
        }
        Queue<RectInt> queue = new();
        queue.Enqueue(firstroom);

        visited.Add(firstroom);
        while (queue.Count > 0)
        {
            RectInt currentRoom = queue.Dequeue();
            checkedRoom = currentRoom;
            //Debug.Log(currentRoom);
            if (dungeonGenerator.splitType != DungeonGenerator.SplitType.instant)
            {
                yield return dungeonGenerator.SplitWait();
            }
            foreach (RectInt neighbor in roomGraph.ReturnRoomAdjacents(currentRoom))
            {
                if (visited.Contains(neighbor)) continue;
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
        //Debug.Log("There are " + roomGraph.ReturnRooms().Count + "Nodes");
        //Debug.Log(visited.Count + "Has been checked");
        checkedRoom = new();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator RemoveRoom()
    {
        int deleteAmount = dungeonGenerator.doneRooms.Count - Mathf.FloorToInt(dungeonGenerator.doneRooms.Count * (deletePercent / 100));
        int initialRoomCount = dungeonGenerator.doneRooms.Count;
        canRemove = true;

        while (canRemove == true && dungeonGenerator.doneRooms.Count > deleteAmount)
        {
            RectInt roomToDelete = dungeonGenerator.doneRooms[0];

            //get the smallest room
            for (int i = 0; i < dungeonGenerator.doneRooms.Count; i++)
            {
                if (dungeonGenerator.doneRooms[i].width * dungeonGenerator.doneRooms[i].height < roomToDelete.width * roomToDelete.height)
                {
                    roomToDelete = dungeonGenerator.doneRooms[i];
                }
                else
                {
                    continue;
                }
            }

            visited.Add(roomToDelete);
            //foreach (RectInt door in roomGraph.ReturnRoomAdjacents(roomToDelete))
            //{
            //    visited.Add(door);
            //}

            yield return StartCoroutine(CheckGraph());

            if (visited.Count == roomGraph.ReturnGraphLength()) //delete room
            {
                Debug.Log(visited.Count + " " + roomGraph.ReturnGraphLength());
                foreach (RectInt door in roomGraph.ReturnRoomAdjacents(roomToDelete))
                {
                    dungeonGenerator.doors.Remove(door);
                    roomGraph.RemoveNode(door);
                }
                roomGraph.RemoveNode(roomToDelete);
                dungeonGenerator.doneRooms.Remove(roomToDelete);
                Debug.Log("deleted");
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
        Debug.Log("Deleted " + (initialRoomCount - dungeonGenerator.doneRooms.Count) + " rooms");
    }


    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator RemoveCycles()
    {
        RectInt firstroom = roomGraph.ReturnRooms()[0];
        if (visited.Contains(firstroom))
        {
            firstroom = roomGraph.ReturnRooms()[1];
        }
        Stack<RectInt> stack = new();
        stack.Push(firstroom);

        visited.Add(firstroom);
        while (stack.Count > 0)
        {
            RectInt currentRoom = stack.Pop();
            checkedRoom = currentRoom;
            //Debug.Log(currentRoom);
            if ((currentRoom.width == 1 || currentRoom.height == 1) && visited.Contains(roomGraph.ReturnRoomAdjacents(currentRoom)[0]) && visited.Contains(roomGraph.ReturnRoomAdjacents(currentRoom)[1]))
            {
                dungeonGenerator.doors.Remove(currentRoom);
                roomGraph.RemoveNode(currentRoom);
                continue;
            }
            if (dungeonGenerator.splitType != DungeonGenerator.SplitType.instant)
            {
                yield return dungeonGenerator.SplitWait();
            }
            foreach (RectInt neighbor in roomGraph.ReturnRoomAdjacents(currentRoom))
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
    }
}
