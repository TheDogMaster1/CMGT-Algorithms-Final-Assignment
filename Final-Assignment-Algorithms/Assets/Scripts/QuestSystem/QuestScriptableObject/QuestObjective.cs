[System.Serializable]
public class QuestObjective
{
    public string objectiveID;
    public string description;
    public EObjectiveType type;
    public int neededAmount;
    public int currentAmount;

    public bool isCompleted => currentAmount >= neededAmount;
}
