using UnityEngine;
using UnityEngine.UI;

public class HPBarPresenter : PlayerObserver
{
    [SerializeField]
    private Image hpBar;
    protected override void OnPlayerHit(PlayerData playerData, Enemy enemy)
    {
        hpBar.fillAmount = (float)playerData.currentHp / (float)playerData.maxHp;
    }
}
