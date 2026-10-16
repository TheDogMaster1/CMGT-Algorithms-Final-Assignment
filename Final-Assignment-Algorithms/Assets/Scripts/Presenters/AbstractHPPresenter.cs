using UnityEngine;

public abstract class AbstractHPPresenter : MonoBehaviour
{

    public abstract void PresentHP(PlayerData playerData);

    private void OnEnable()
    {
        HpController.OnPlayerHit += PresentHP;
    }

    private void OnDisable()
    {
        HpController.OnPlayerHit -= PresentHP;
    }
}
