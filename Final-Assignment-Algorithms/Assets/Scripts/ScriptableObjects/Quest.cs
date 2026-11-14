using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;
    public bool repeatable;
    //TO DO reward

    //[HideInInspector]
    public bool Started = false;

    public virtual void StartQuest()
    {
        Started = true;
    }

    public virtual void StopQuest()
    {
        Started = false;
    }

    private void OnDisable()
    {
        Started = false;
    }
}
