using UnityEngine;
using UnityEngine.UI;

public class HPBarPresenter : PlayerObserver
{
    [SerializeField]
    private Image hpBar;
    public override void OnPlayerHit(PlayerData playerData)
    {
        hpBar.fillAmount = (float)playerData.currentHp / (float)playerData.maxHp;
    }
}
