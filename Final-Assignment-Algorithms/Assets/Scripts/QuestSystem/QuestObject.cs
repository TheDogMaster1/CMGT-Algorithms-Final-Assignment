using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestObject : MonoBehaviour
{
    private Quest quest;

    [SerializeField]
    private TextMeshProUGUI questTitle;
    [SerializeField]
    private TextMeshProUGUI questDescription;
    [SerializeField]
    private Button acceptQuestButton;
    [SerializeField]
    private Button abandonQuestButton;
    [SerializeField]
    private Button handInQuestButton;

    private void OnEnable()
    {
        EventBus<NewQuestChosenEvent>.OnEventPublished += UpdateButtons;
    }

    public void Init(Quest pQuest)
    {
        quest = pQuest;
        questTitle.text = quest.questName;
        questDescription.text = quest.questDescription;
    }

    public void ChooseQuest()
    {
        EventBus<NewQuestChosenEvent>.Publish(new NewQuestChosenEvent(quest));
        QuestController.instance.SetQuest(quest);
    }

    public void AbandonQuest()
    {
        EventBus<NewQuestChosenEvent>.Publish(new NewQuestChosenEvent(null));
        QuestController.instance.SetQuest(null);
    }

    public void UpdateButtons(NewQuestChosenEvent newQuestEvent)
    {
        if (newQuestEvent.quest == null || newQuestEvent.quest.questID != quest.questID)
        {
            SetButtonToAccept();
        }
        else
        {
            SetButtonToAbandon();
        }
    }

    private void SetButtonToAccept()
    {
        acceptQuestButton.gameObject.SetActive(true);
        abandonQuestButton.gameObject.SetActive(false);
        handInQuestButton.gameObject.SetActive(false);
    }

    private void SetButtonToAbandon()
    {
        acceptQuestButton.gameObject.SetActive(false);
        abandonQuestButton.gameObject.SetActive(true);
        handInQuestButton.gameObject.SetActive(false);
    }

    private void SetButtonToHandIn()
    {
        acceptQuestButton.gameObject.SetActive(false);
        abandonQuestButton.gameObject.SetActive(false);
        handInQuestButton.gameObject.SetActive(true);
    }
}
