using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGraphController : DungeonSettings
{
    private DungeonGenerator dungeonGenerator;

    private Graph<RectInt> roomGraph;
    private Graph<RectInt> dfsGraph;

    private RectInt checkedRoom;

    private bool canRemove = true;

    [Range(0, 100)]
    [SerializeField]
    private float deletePercent = 0;
    [SerializeField]
    private bool showDFSGraph = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        roomGraph = new();
        dfsGraph = new();
    }

    // Update is called once per frame
    void Update()
    {
        if (!showDFSGraph)
        {
            foreach (RectInt room in roomGraph.ReturnNodesList())
            {
                DebugExtension.DebugWireSphere(new Vector3(room.center.x, 0, room.center.y), Color.cyan);
                foreach (RectInt door in roomGraph.ReturnAdjacents(room))
                {
                    Debug.DrawLine(new Vector3(room.center.x, 0, room.center.y), new Vector3(door.center.x, 0, door.center.y), Color.yellow);
                }
            }
        }
        else
        {
            foreach (RectInt room in dfsGraph.ReturnNodesList())
            {
                DebugExtension.DebugWireSphere(new Vector3(room.center.x, 0, room.center.y), Color.cyan);
                foreach (RectInt door in dfsGraph.ReturnAdjacents(room))
                {
                    Debug.DrawLine(new Vector3(room.center.x, 0, room.center.y), new Vector3(door.center.x, 0, door.center.y), Color.yellow);
                }
            }
        }
        if (checkedRoom.width > 0) AlgorithmsUtils.DebugRectInt(checkedRoom, Color.white, 0, false, 3);
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
            if (waitType != WaitType.instant) yield return Wait();
        }
        for (int i = 0; i < dungeonGenerator.GetDoneRooms().Count; i++)
        {
            for (int j = 0; j < dungeonGenerator.GetDoors().Count; j++)
            {
                if (AlgorithmsUtils.Intersects(dungeonGenerator.GetDoneRooms()[i], dungeonGenerator.GetDoors()[j]))
                {
                    roomGraph.AddEdge(dungeonGenerator.GetDoneRooms()[i], dungeonGenerator.GetDoors()[j]);
                    if (waitType != WaitType.instant) yield return Wait();
                }
            }
        }
        if (autoContinue) StartCoroutine(RemoveRooms());
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator RemoveRooms()
    {
        int deleteAmount = dungeonGenerator.GetDoneRooms().Count - Mathf.FloorToInt(dungeonGenerator.GetDoneRooms().Count * (deletePercent / 100));
        int initialRoomCount = dungeonGenerator.GetDoneRooms().Count;
        canRemove = true;


        while (canRemove == true && dungeonGenerator.GetDoneRooms().Count > deleteAmount)
        {
            HashSet<RectInt> visited = new();
            RectInt roomToDelete = GetSmallestRoom(dungeonGenerator.GetDoneRooms()[0]);

            visited.Add(roomToDelete);

            var deletable = roomGraph.BFSGraphSearch(visited);

            if (deletable) //delete room
            {
                DeleteRoom(roomToDelete);
                if (waitType != WaitType.instant) yield return Wait();
            }
            else // keep room
            {
                Debug.Log("Can't be deleted");
                Debug.Log("tried to delete" + roomToDelete);
                Debug.Log(visited.Count + "  " + roomGraph.ReturnGraphLength());
                AlgorithmsUtils.DebugRectInt(roomToDelete, Color.cyan, 3, false, 3);
                canRemove = false;
            }
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
        }
        return room;
    }

    private void DeleteRoom(RectInt room)
    {
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
        HashSet<RectInt> visited = new();
        dfsGraph.DFSGraphMaker(visited, roomGraph.ReturnNodesList()[0], roomGraph);
        HashSet<RectInt> doors = dungeonGenerator.GetDoorsHash();

        foreach (RectInt node in dfsGraph.ReturnNodesList())
        {
            if (doors.Contains(node) && dfsGraph.ReturnAdjacents(node).Count == 1)
            {
                dungeonGenerator.GetDoors().Remove(node);
                roomGraph.RemoveNode(node);
                if (waitType != WaitType.instant) yield return Wait();
            }
        }
        if (autoContinue) onScriptComplete?.Invoke();
    }

    public Graph<RectInt> GetRoomGraph()
    {
        return roomGraph;
    }
}
