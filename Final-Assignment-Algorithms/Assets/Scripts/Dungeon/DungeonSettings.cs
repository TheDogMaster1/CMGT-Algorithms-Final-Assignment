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

    public enum WaitType { instant, withDelay, withSpacebar }

    [SerializeField]
    protected WaitType waitType = WaitType.withDelay;

    [SerializeField]
    protected float secondsToWait;


    protected IEnumerator Wait()
    {
        switch (waitType)
        {
            case WaitType.withDelay:
                yield return new WaitForSeconds(secondsToWait);
                break;
            case WaitType.withSpacebar:
                yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
                yield return null;
                break;
        }
    }
}
