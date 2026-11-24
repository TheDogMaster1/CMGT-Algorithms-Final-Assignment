using TMPro;
using UnityEngine;

public class TMPQuestPresenter : AbstractQuestPresenter
{
    [SerializeField]
    private TextMeshProUGUI questText;
    public override void PresentQuest(QuestProgress questProgress)
    {
        Debug.Log("got the action, the quest is " + questProgress.quest);
        if(questProgress.quest == null)
        {
            questText.text = "No quest selected.";
            return;
        }
        questText.text = questProgress.quest.questName + "\n";
        foreach (var objective in questProgress.objectives)
        {
            questText.text += objective.description + " (" + objective.currentAmount + "/" + objective.neededAmount + ")\n";
        }
    }
}
