using UnityEngine;

public abstract class PlayerObserver : MonoBehaviour
{
    public abstract void OnPlayerHit(PlayerData playerData);

    private void OnEnable()
    {
        PlayerController.OnPlayerHit += OnPlayerHit;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerHit -= OnPlayerHit;
    }
}
