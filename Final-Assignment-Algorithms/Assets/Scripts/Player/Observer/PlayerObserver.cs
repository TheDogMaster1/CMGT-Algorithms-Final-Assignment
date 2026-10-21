using UnityEngine;

public abstract class PlayerObserver : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    protected abstract void OnPlayerHit(PlayerData playerData, Enemy enemy);

    private void OnEnable()
    {
        playerController.onPlayerHit += OnPlayerHit;
    }

    private void OnDisable()
    {
        playerController.onPlayerHit -= OnPlayerHit;
    }
}
