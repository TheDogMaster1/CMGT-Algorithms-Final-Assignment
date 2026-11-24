using UnityEngine;

public abstract class AbstractQuestPresenter : MonoBehaviour
{
    public abstract void PresentQuest(QuestProgress quest);

    private void OnEnable()
    {
        QuestController.updateQuestProgress += PresentQuest;
    }

    private void OnDisable()
    {
        QuestController.updateQuestProgress -= PresentQuest;
    }
}
