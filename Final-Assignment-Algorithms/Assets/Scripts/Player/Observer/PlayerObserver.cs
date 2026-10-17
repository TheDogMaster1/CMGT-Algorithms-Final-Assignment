using UnityEngine;

public abstract class PlayerObserver : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    public abstract void OnPlayerHit(PlayerData playerData, Enemy enemy);

    private void OnEnable()
    {
        playerController.OnPlayerHit += OnPlayerHit;
    }

    private void OnDisable()
    {
        playerController.OnPlayerHit -= OnPlayerHit;
    }
}
