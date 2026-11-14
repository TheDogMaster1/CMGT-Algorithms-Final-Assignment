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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Init(Quest pQuest)
    {
        quest = pQuest;
        questTitle.text = quest.questName;
        questDescription.text = quest.questDescription;
        if (quest.Started)
        {
            acceptQuestButton.gameObject.SetActive(true);
            abandonQuestButton.gameObject.SetActive(false);
        }
    }

    public void ChooseQuest()
    {
        //EventBus<NewQuestChosenEvent>.Publish(new NewQuestChosenEvent(quest));
        quest.StartQuest();
        acceptQuestButton.gameObject.SetActive(false);
        abandonQuestButton.gameObject.SetActive(true);
    }

    private void ChangeQuestButtons()
    {

    }
}
