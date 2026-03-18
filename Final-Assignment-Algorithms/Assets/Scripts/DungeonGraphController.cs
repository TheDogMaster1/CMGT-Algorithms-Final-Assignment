using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGraphController : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;

    public DungeonGraph<RectInt> roomGraph;
    public DungeonGraph<RectInt> doorGraph;

    private RectInt checkedRoom;

    private HashSet<RectInt> visited = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        roomGraph = new();
        doorGraph = new();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (RectInt room in roomGraph.ReturnRooms())
        {
            DebugExtension.DebugWireSphere(new Vector3(room.x + room.width / 2f, 0, room.y + room.height / 2f), Color.cyan);
            foreach (RectInt door in roomGraph.ReturnRoomAdjacents(room))
            {
                Debug.DrawLine(new Vector3(room.x + room.width / 2f, 0, room.y + room.height / 2f), new Vector3(door.x + door.width / 2f, 0, door.y + door.height / 2f), Color.yellow);
            }
        }
        foreach (RectInt door in doorGraph.ReturnRooms())
        {
            DebugExtension.DebugWireSphere(new Vector3(door.x + door.width / 2f, 0, door.y + door.height / 2f), Color.cyan);
        }
        if (checkedRoom.width > 0) AlgorithmsUtils.DebugRectInt(checkedRoom, Color.white, 0, false, 3);
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void GenerateGraph()
    {
        roomGraph = new();
        foreach (RectInt room in dungeonGenerator.doneRooms)
        {
            roomGraph.AddNode(room);
        }
        //foreach (RectInt door in dungeonGenerator.doors)
        //{
        //    doorGraph.AddNode(door);
        //}
        for (int i = 0; i < dungeonGenerator.doneRooms.Count; i++)
        {
            for (int j = 0; j < dungeonGenerator.doors.Count; j++)
            {
                if (AlgorithmsUtils.Intersects(dungeonGenerator.doneRooms[i], dungeonGenerator.doors[j]))
                {
                    roomGraph.AddEdge(dungeonGenerator.doneRooms[i], dungeonGenerator.doors[j]);
                }
            }
        }
    }
    //[Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator CheckGraph()
    {
        RectInt firstroom = roomGraph.ReturnRooms()[0];
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
        RectInt roomToDelete = dungeonGenerator.doneRooms[0];
        List<RectInt> roomWithDoors = new();

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

        roomWithDoors.Add(roomToDelete);
        visited.Add(roomToDelete);
        foreach (RectInt door in roomGraph.ReturnRoomAdjacents(roomToDelete))
        {
            roomWithDoors.Add(door);
            visited.Add(door);
        }

        yield return StartCoroutine(CheckGraph());

        if (visited.Count == roomGraph.ReturnGraphLength())
        {
            roomGraph.RemoveNode(roomToDelete);
            dungeonGenerator.doneRooms.Remove(roomToDelete);
            roomWithDoors.Remove(roomToDelete);
            foreach (RectInt door in roomWithDoors)
            {
                dungeonGenerator.doors.Remove(door);
                roomGraph.RemoveNode(door);
            }
            Debug.Log("deleted");
            visited.Clear();
        }
        else
        {
            roomWithDoors.Clear();
            Debug.Log("Can't be deleted");
            Debug.Log("tried to delete" + roomToDelete);
            Debug.Log(visited.Count + "  " + roomGraph.ReturnGraphLength());
            AlgorithmsUtils.DebugRectInt(roomToDelete, Color.cyan, 10, false, 3);
        }

        //Debug.Log("room: " + roomToDelete + "Size: " + roomToDelete.width * roomToDelete.height);
    }
}
