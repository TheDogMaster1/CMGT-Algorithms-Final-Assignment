using UnityEngine;
using UnityEngine.UI;

public class HPBarPresenter : AbstractHPPresenter
{
    [SerializeField]
    private Image hpBar;
    public override void PresentHP(PlayerData playerData)
    {
        hpBar.fillAmount = (float)playerData.currentHp / (float)playerData.maxHp;
    }
}
