using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DungeonGenerator : DungeonSettings
{
    [SerializeField]
    private List<RectInt> toDoRooms;
    [SerializeField]
    private List<RectInt> doneRooms;
    [SerializeField]
    private List<RectInt> doors;
    [SerializeField]
    private RectInt BaseRoom;
    [SerializeField]
    private RectInt maximumRoomSize;

    private RectInt CurrentRoom;
    private RectInt currentDoor;

    [SerializeField]
    private int minSplitPoint;

    [SerializeField]
    private int overlapAmount = 1;

    [Header("Seed options")]

    [SerializeField]
    private bool seeded = false;
    [SerializeField]
    private int seed = 0;

    private DungeonGraphController graphController;
    private TileMapGenerator tileMapGen;
    private MarchingSquareSpawner marchingSquares;
    private FloodfillSpawner floodFillFloors;
    private TileGraphGenerator tileGraphGen;
    private DungeonAddAssets simpleAssets;

    private Random random = new Random();

    private void Start()
    {
        graphController = GetComponent<DungeonGraphController>();
        tileMapGen = GetComponent<TileMapGenerator>();
        marchingSquares = GetComponent<MarchingSquareSpawner>();
        floodFillFloors = GetComponent<FloodfillSpawner>();
        tileGraphGen = GetComponent<TileGraphGenerator>();
        simpleAssets = GetComponent<DungeonAddAssets>();
    }
    private void Update()
    {
        for (int i = 0; i < toDoRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(toDoRooms[i], Color.red);
        }

        for (int i = 0; i < doneRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doneRooms[i], Color.green, 0);
            AlgorithmsUtils.DebugRectInt(doneRooms[i], new Color(0.8f, 0, 1, 1), 0, false, 3);
        }

        for (int i = 0; i < doors.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doors[i], Color.blue, 0, false, 3);
        }
        if (CurrentRoom.width > 0) AlgorithmsUtils.DebugRectInt(CurrentRoom, Color.cyan, 0);
        if (currentDoor.width > 0) AlgorithmsUtils.DebugRectInt(currentDoor, Color.cyan, 0, false, 3);
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void GenerateDungeonWithDoors()
    {
        ResetEverything();
    }

    private void ResetEverything()
    {
        toDoRooms.Clear();
        doneRooms.Clear();
        doors.Clear();
        graphController.GetRoomGraph().ClearGraph();
        random = new();
        currentDoor = new();
        CurrentRoom = new();
        if (tileMapGen != null)
        {
            tileMapGen.ResetTileMap();
            DestroyAssets();
            tileGraphGen.ResetGraph();
        }
        if (simpleAssets != null)
        {
            simpleAssets.DestroyAssets();
        }
        StopAllCoroutines();
        StartCoroutine(Splitrooms());
    }

    private IEnumerator Splitrooms()
    {

        if (seeded == true)
        {
            random = new Random(seed);
        }
        else
        {
            random = new();
        }
        toDoRooms.Add(BaseRoom);
        while (toDoRooms.Count > 0)
        {
            if (waitType != WaitType.instant)
            {
                yield return Wait();
            }
            if (toDoRooms[0].width > toDoRooms[0].height)
            {
                int splitPoint = random.Next(minSplitPoint, toDoRooms[0].width - minSplitPoint);
                //Debug.Log(toDoRooms[0]);
                //Debug.Log("SplitPoint is " + splitPoint);
                RectInt newroom = toDoRooms[0];
                newroom.width = toDoRooms[0].width - splitPoint + overlapAmount;
                CurrentRoom = newroom;
                CheckRoomSize(newroom);
                if (waitType != WaitType.instant)
                {
                    yield return Wait();
                }

                newroom.width = toDoRooms[0].width - (toDoRooms[0].width - splitPoint);
                newroom.x = toDoRooms[0].x + (toDoRooms[0].width - splitPoint);
                CurrentRoom = newroom;
                CheckRoomSize(newroom);
                toDoRooms.Remove(toDoRooms[0]);
                if (waitType != WaitType.instant)
                {
                    yield return Wait();
                }
            }
            else
            {
                int splitPoint = random.Next(minSplitPoint, toDoRooms[0].height - minSplitPoint);
                RectInt newroom = toDoRooms[0];
                newroom.height = toDoRooms[0].height - splitPoint + overlapAmount;
                CurrentRoom = newroom;
                CheckRoomSize(newroom);
                if (waitType != WaitType.instant)
                {
                    yield return Wait();
                }

                newroom.height = toDoRooms[0].height - (toDoRooms[0].height - splitPoint);
                newroom.y = toDoRooms[0].y + (toDoRooms[0].height - splitPoint);
                CurrentRoom = newroom;
                CheckRoomSize(newroom);
                toDoRooms.Remove(toDoRooms[0]);
                if (waitType != WaitType.instant)
                {
                    yield return Wait();
                }
            }
        }
        StartCoroutine(AddDoors());
        CurrentRoom = RectInt.zero;
    }

    private void CheckRoomSize(RectInt room)
    {
        if (room.width <= maximumRoomSize.width && room.height <= maximumRoomSize.height)
        {
            doneRooms.Add(room);
        }
        else
        {
            toDoRooms.Add(room);
        }
    }


    private IEnumerator AddDoors()
    {
        for (int i = 0; i < doneRooms.Count; i++) //O(n^2)
        {
            for (int j = i + 1; j < doneRooms.Count; j++)
            {
                RectInt door = AlgorithmsUtils.Intersect(doneRooms[i], doneRooms[j]);
                if (door.height > 10)
                {
                    door.y += door.height / 2 - 1;
                    door.height = 3;
                }
                else if (door.width > 10)
                {
                    door.x += door.width / 2 - 1;
                    door.width = 3;
                }
                else
                {
                    continue;
                }
                currentDoor = door;
                doors.Add(door);
                if (waitType != WaitType.instant)
                {
                    yield return Wait();
                }
            }
        }
        currentDoor = RectInt.zero;
        if (autoContinue) onScriptComplete?.Invoke();
    }


    public List<RectInt> GetDoneRooms()
    {
        return doneRooms;
    }

    public List<RectInt> GetDoors()
    {
        return doors;
    }

    public HashSet<RectInt> GetDoorsHash()
    {
        HashSet<RectInt> doorsHashset = new();

        for (int i = 0; i < doors.Count; i++)
        {
            doorsHashset.Add(doors[i]);
        }
        return doorsHashset;
    }
    public WaitType GetSplitType()
    {
        return waitType;
    }

    public RectInt GetDungeonBounds()
    {
        return BaseRoom;
    }

    public void DestroyAssets()
    {
        foreach (Transform child in marchingSquares.GetWallParent())
        {
            if (child != null) Destroy(child.gameObject);
        }
        foreach (Transform child in floodFillFloors.GetFloorParent())
        {
            if (child != null) Destroy(child.gameObject);
        }
    }
}
