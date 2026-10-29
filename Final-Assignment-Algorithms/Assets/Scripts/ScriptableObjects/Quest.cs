using UnityEngine;


public abstract class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;
    //TO DO reward

    public abstract void StartQuest();

    public abstract void StopQuest();

}
