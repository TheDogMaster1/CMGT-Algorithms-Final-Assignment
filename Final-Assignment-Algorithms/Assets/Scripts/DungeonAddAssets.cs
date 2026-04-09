using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAddAssets : MonoBehaviour
{
    private HashSet<Vector3> wallPositions = new();
    private HashSet<Vector3> floorPositions = new();

    private DungeonGenerator dGen;

    public GameObject wallPrefab;
    public Transform wallParent;

    public GameObject floorPrefab;
    public Transform floorParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dGen = GetComponent<DungeonGenerator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator AddAssets()
    {
        Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f);
        foreach (RectInt door in dGen.doors)
        {
            if (door.width > door.height)
            {
                for (int i = 0; i < door.width; i++)
                {
                    wallPositions.Add(new Vector3(door.x + i, 0, door.y) + offset);
                }
            }
            else
            {
                for (int i = 0; i < door.height; i++)
                {
                    wallPositions.Add(new Vector3(door.x, 0, door.y + i) + offset);
                }
            }
        }

        foreach (RectInt room in dGen.doneRooms)
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                AddWall(new Vector2Int(x, room.yMin), offset);
                if (dGen.splitType != DungeonGenerator.SplitType.instant) yield return dGen.SplitWait();

                AddWall(new Vector2Int(x, room.yMax - 1), offset);
                if (dGen.splitType != DungeonGenerator.SplitType.instant) yield return dGen.SplitWait();
            }
            for (int y = room.yMin; y < room.yMax; y++)
            {
                AddWall(new Vector2Int(room.xMin, y), offset);
                if (dGen.splitType != DungeonGenerator.SplitType.instant) yield return dGen.SplitWait();

                AddWall(new Vector2Int(room.xMax - 1, y), offset);
                if (dGen.splitType != DungeonGenerator.SplitType.instant) yield return dGen.SplitWait();
            }
        }

        foreach (RectInt room in dGen.doneRooms)
        {
            for (int i = room.xMin; i < room.xMax; i++)
            {
                for (int j = room.yMin; j < room.yMax; j++)
                {
                    AddFloor(new Vector2Int(i, j), new Vector3(0.5f, 0, 0.5f));
                    if (dGen.splitType != DungeonGenerator.SplitType.instant) yield return dGen.SplitWait();
                }
            }
        }
    }

    private void AddFloor(Vector2Int pFloorPosition, Vector3 pOffset)
    {
        Vector3 spawnPos = new Vector3(pFloorPosition.x, 0, pFloorPosition.y) + pOffset;

        if (floorPositions.Contains(spawnPos)) return;
        Instantiate(floorPrefab, spawnPos, Quaternion.Euler(90, 0, 0), floorParent);
        floorPositions.Add(spawnPos);
    }

    private void AddWall(Vector2Int pWallPosition, Vector3 pOffset)
    {
        Vector3 spawnPos = new Vector3(pWallPosition.x, 0, pWallPosition.y) + pOffset;

        if (wallPositions.Contains(spawnPos)) return;
        Instantiate(wallPrefab, spawnPos, Quaternion.identity, wallParent);
        wallPositions.Add(spawnPos);
    }
}
