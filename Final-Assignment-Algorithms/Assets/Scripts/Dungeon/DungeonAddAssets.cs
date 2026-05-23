using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAddAssets : DungeonSettings
{
    private HashSet<Vector3> wallPositions = new();
    private HashSet<Vector3> floorPositions = new();

    private DungeonGenerator dGen;

    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private Transform wallParent;

    [SerializeField]
    private GameObject floorPrefab;
    [SerializeField]
    private Transform floorParent;

    [SerializeField]
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dGen = GetComponent<DungeonGenerator>();
    }

    public void AssetAdder()
    {
        StartCoroutine(AddAssets());
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator AddAssets()
    {
        Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f);
        foreach (RectInt door in dGen.GetDoors())
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

        foreach (RectInt room in dGen.GetDoneRooms())
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                StartCoroutine(AddWall(new Vector2Int(x, room.yMin), offset));
                if (splitType != SplitType.instant) yield return null;
                StartCoroutine(AddWall(new Vector2Int(x, room.yMax - 1), offset));
                if (splitType != SplitType.instant) yield return null;
            }
            for (int y = room.yMin; y < room.yMax; y++)
            {
                StartCoroutine(AddWall(new Vector2Int(room.xMin, y), offset));
                if (splitType != SplitType.instant) yield return null;
                StartCoroutine(AddWall(new Vector2Int(room.xMax - 1, y), offset));
                if (splitType != SplitType.instant) yield return null;
            }
        }

        foreach (RectInt room in dGen.GetDoneRooms())
        {
            for (int i = room.xMin; i < room.xMax; i++)
            {
                for (int j = room.yMin; j < room.yMax; j++)
                {
                    StartCoroutine(AddFloor(new Vector2Int(i, j), new Vector3(0.5f, 0, 0.5f)));
                    if (splitType != SplitType.instant) yield return null;
                }
            }
        }
        if (splitType != SplitType.instant) yield return null;
        player.transform.position = new Vector3(dGen.GetDoneRooms()[0].center.x, 1, dGen.GetDoneRooms()[0].center.y);
        if (autoContinue) onScriptComplete?.Invoke();
    }

    private IEnumerator AddFloor(Vector2Int pFloorPosition, Vector3 pOffset)
    {
        Vector3 spawnPos = new Vector3(pFloorPosition.x, 0, pFloorPosition.y) + pOffset;

        if (!floorPositions.Contains(spawnPos))
        {
            Instantiate(floorPrefab, spawnPos, Quaternion.Euler(90, 0, 0), floorParent);
            floorPositions.Add(spawnPos);
            if (splitType != SplitType.instant) yield return SplitWait();
        }
    }

    private IEnumerator AddWall(Vector2Int pWallPosition, Vector3 pOffset)
    {
        Vector3 spawnPos = new Vector3(pWallPosition.x, 0, pWallPosition.y) + pOffset;

        if (!wallPositions.Contains(spawnPos))
        {
            Instantiate(wallPrefab, spawnPos, Quaternion.identity, wallParent);
            wallPositions.Add(spawnPos);
            if (splitType != SplitType.instant) yield return SplitWait();
        }
    }

    public void DestroyAssets()
    {
        foreach (Transform child in wallParent.transform)
        {
            if (child != null) Destroy(child.gameObject);
        }
        foreach (Transform child in floorParent.transform)
        {
            if (child != null) Destroy(child.gameObject);
        }
        wallPositions.Clear();
        floorPositions.Clear();
    }
}
