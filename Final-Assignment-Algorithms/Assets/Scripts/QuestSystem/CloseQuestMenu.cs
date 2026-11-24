using UnityEngine;

public class CloseQuestMenu : MonoBehaviour
{
    [SerializeField]
    private Transform questsParent;
    private void OnDisable()
    {
        foreach (Transform child in questsParent)
        {
            Debug.Log("Destroying this " + child.name);
            Destroy(child.gameObject);
        }
    }
}
