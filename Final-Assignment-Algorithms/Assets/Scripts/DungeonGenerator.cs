using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<RectInt> ToDoRooms;
    public List<RectInt> DoneRooms;
    public int roomindex;
    public bool splitVertical;
    public int timesToSplit;

    public float secondsToWait;

    public RectInt maximumRoomSize;


    private void Update()
    {
        for (int i = 0; i < ToDoRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(ToDoRooms[i], Color.red);
        }

        for (int i = 0; i < DoneRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(DoneRooms[i], Color.green);
        }
        //for (int i = 0; i < outputRooms.Count; i++)
        //{
        //    if (i % 2 == 0)
        //    {
        //        AlgorithmsUtils.DebugRectInt(outputRooms[i], Color.red);
        //    }
        //    else
        //    {
        //        AlgorithmsUtils.DebugRectInt(outputRooms[i], Color.blue);
        //    }
        //}
    }
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void SplitRooms()
    {

        if (splitVertical == true)
        {
            RectInt newroom = ToDoRooms[roomindex];
            newroom.width = ToDoRooms[roomindex].width / 2 + 1;
            ToDoRooms.Add(newroom);

            newroom.width = ToDoRooms[roomindex].width - ToDoRooms[roomindex].width / 2;
            newroom.x = ToDoRooms[roomindex].x + ToDoRooms[roomindex].width / 2;
            ToDoRooms.Add(newroom);
            ToDoRooms.Remove(ToDoRooms[roomindex]);
        }
        else
        {
            RectInt newroom = ToDoRooms[roomindex];
            newroom.height = ToDoRooms[roomindex].height / 2 + 1;
            ToDoRooms.Add(newroom);

            newroom.height = ToDoRooms[roomindex].height - ToDoRooms[roomindex].height / 2;
            newroom.y = ToDoRooms[roomindex].y + ToDoRooms[roomindex].height / 2;
            ToDoRooms.Add(newroom);
            ToDoRooms.Remove(ToDoRooms[roomindex]);
        }
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator AutoSplitRooms()
    {
        int randomDirection = Random.Range(0, 2);
        while (ToDoRooms.Count > 0)
        {
            if (ToDoRooms[roomindex].width > ToDoRooms[roomindex].height)
            {
                RectInt newroom = ToDoRooms[roomindex];
                newroom.width = ToDoRooms[roomindex].width / 2 + 1;
                if (newroom.width <= maximumRoomSize.width && newroom.height <= maximumRoomSize.height)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                yield return new WaitForSeconds(secondsToWait);

                newroom.width = ToDoRooms[roomindex].width - ToDoRooms[roomindex].width / 2;
                newroom.x = ToDoRooms[roomindex].x + ToDoRooms[roomindex].width / 2;
                if (newroom.width <= maximumRoomSize.width && newroom.height <= maximumRoomSize.height)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                yield return new WaitForSeconds(secondsToWait);
                randomDirection = Random.Range(0, 2);
                ToDoRooms.Remove(ToDoRooms[roomindex]);
            }
            else
            {
                RectInt newroom = ToDoRooms[roomindex];
                newroom.height = ToDoRooms[roomindex].height / 2 + 1;
                if (newroom.height <= maximumRoomSize.height && newroom.width <= maximumRoomSize.width)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                yield return new WaitForSeconds(secondsToWait);

                newroom.height = ToDoRooms[roomindex].height - ToDoRooms[roomindex].height / 2;
                newroom.y = ToDoRooms[roomindex].y + ToDoRooms[roomindex].height / 2;
                if (newroom.height <= maximumRoomSize.height && newroom.width <= maximumRoomSize.width)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                yield return new WaitForSeconds(secondsToWait);
                randomDirection = Random.Range(0, 2);
                ToDoRooms.Remove(ToDoRooms[roomindex]);
            }
        }
    }



    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator SplitMultipleRooms()
    {
        for (int i = 0; i < timesToSplit; i++)
        {
            if (splitVertical == true)
            {
                RectInt newroom = ToDoRooms[roomindex];
                newroom.width = ToDoRooms[roomindex].width / 2 + 1;
                ToDoRooms.Add(newroom);
                yield return new WaitForSeconds(secondsToWait);


                newroom.width = ToDoRooms[roomindex].width - ToDoRooms[roomindex].width / 2;
                newroom.x = ToDoRooms[roomindex].x + ToDoRooms[roomindex].width / 2;
                ToDoRooms.Add(newroom);
                ToDoRooms.Remove(ToDoRooms[roomindex]);
                yield return new WaitForSeconds(secondsToWait);
            }
            else
            {
                RectInt newroom = ToDoRooms[roomindex];
                newroom.height = ToDoRooms[roomindex].height / 2 + 1;
                ToDoRooms.Add(newroom);
                yield return new WaitForSeconds(secondsToWait);

                newroom.height = ToDoRooms[roomindex].height - ToDoRooms[roomindex].height / 2;
                newroom.y = ToDoRooms[roomindex].y + ToDoRooms[roomindex].height / 2;
                ToDoRooms.Add(newroom);
                ToDoRooms.Remove(ToDoRooms[roomindex]);
                yield return new WaitForSeconds(secondsToWait);
            }
        }
    }
}
