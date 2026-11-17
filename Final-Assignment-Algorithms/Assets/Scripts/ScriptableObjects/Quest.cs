using System;
using System.Collections.Generic;
using UnityEngine;


//used the tutorial from Game Code Library for this code https://www.youtube.com/watch?v=_hA3y45P4Ow&list=PLaaFfzxy_80HtVvBnpK_IjSC8_Y9AOhuP&index=27
[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string questDescription;
    public List<QuestObjective> objectives;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();
        }
    }
}
public enum EObjectiveType
{
    killEnemies,
    collectGold
}


