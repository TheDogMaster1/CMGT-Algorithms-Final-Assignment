using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DungeonSettings : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField]
    protected UnityEvent onScriptComplete;

    [SerializeField]
    protected bool autoContinue = false;

    public enum SplitType { instant, withDelay, withSpacebar }

    [SerializeField]
    protected SplitType splitType = SplitType.withDelay;

    [SerializeField]
    protected float secondsToWait;


    protected IEnumerator Wait()
    {
        switch (splitType)
        {
            case SplitType.withDelay:
                yield return new WaitForSeconds(secondsToWait);
                break;
            case SplitType.withSpacebar:
                yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
                yield return null;
                break;
        }
    }
}
