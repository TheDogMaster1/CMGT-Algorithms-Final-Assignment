using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;
    public bool repeatable;
    //TO DO reward

    public abstract void StartQuest();

    public abstract void StopQuest();

}
