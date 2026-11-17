using System.Collections.Generic;

[System.Serializable]
public class QuestProgress
{
    public Quest quest;
    public List<QuestObjective> objectives;

    public QuestProgress(Quest pQuest)
    {
        quest = pQuest;
        objectives = new List<QuestObjective>();

        foreach (var obj in quest.objectives)
        {
            objectives.Add(new QuestObjective
            {
                objectiveID = obj.objectiveID,
                description = obj.description,
                type = obj.type,
                neededAmount = obj.neededAmount,
                currentAmount = 0
            });
        }
    }

    public bool allObjectivesComplete => objectives.TrueForAll(o => o.isCompleted);

    public string questID => quest.questID;
}
