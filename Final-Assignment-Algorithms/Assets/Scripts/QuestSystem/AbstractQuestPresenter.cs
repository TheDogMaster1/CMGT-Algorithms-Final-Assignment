using UnityEngine;

public abstract class AbstractQuestPresenter : MonoBehaviour
{
    public abstract void PresentQuest(Quest quest);

    private void OnEnable()
    {
    }
}
