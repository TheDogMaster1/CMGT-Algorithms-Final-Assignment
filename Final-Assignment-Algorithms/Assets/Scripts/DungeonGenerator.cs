using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<RectInt> inputRooms;
    //public List<RectInt> outputRooms;
    public int roomindex;
    public bool splitVertical;
    public int timesToSplit;
    private void Update()
    {
        for (int i = 0; i < inputRooms.Count; i++)
        {
            if (i % 2 == 0)
            {
                AlgorithmsUtils.DebugRectInt(inputRooms[i], Color.blue);
            }
            else
            {
                AlgorithmsUtils.DebugRectInt(inputRooms[i], Color.red);
            }
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
            RectInt newroom = inputRooms[roomindex];
            newroom.width = inputRooms[roomindex].width / 2 + 1;
            inputRooms.Add(newroom);

            newroom.width = inputRooms[roomindex].width - inputRooms[roomindex].width / 2;
            newroom.x = inputRooms[roomindex].x + inputRooms[roomindex].width / 2;
            inputRooms.Add(newroom);
            inputRooms.Remove(inputRooms[roomindex]);
        }
        else
        {
            RectInt newroom = inputRooms[roomindex];
            newroom.height = inputRooms[roomindex].height / 2 + 1;
            inputRooms.Add(newroom);

            newroom.height = inputRooms[roomindex].height - inputRooms[roomindex].height / 2;
            newroom.y = inputRooms[roomindex].y + inputRooms[roomindex].height / 2;
            inputRooms.Add(newroom);
            inputRooms.Remove(inputRooms[roomindex]);
        }
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator SplitMultipleRooms()
    {
        for (int i = 0; i < timesToSplit; i++)
        {
            if (splitVertical == true)
            {
                RectInt newroom = inputRooms[roomindex];
                newroom.width = inputRooms[roomindex].width / 2 + 1;
                inputRooms.Add(newroom);
                yield return new WaitForSeconds(.1f);


                newroom.width = inputRooms[roomindex].width - inputRooms[roomindex].width / 2;
                newroom.x = inputRooms[roomindex].x + inputRooms[roomindex].width / 2;
                inputRooms.Add(newroom);
                inputRooms.Remove(inputRooms[roomindex]);
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                RectInt newroom = inputRooms[roomindex];
                newroom.height = inputRooms[roomindex].height / 2 + 1;
                inputRooms.Add(newroom);
                yield return new WaitForSeconds(.1f);

                newroom.height = inputRooms[roomindex].height - inputRooms[roomindex].height / 2;
                newroom.y = inputRooms[roomindex].y + inputRooms[roomindex].height / 2;
                inputRooms.Add(newroom);
                inputRooms.Remove(inputRooms[roomindex]);
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
