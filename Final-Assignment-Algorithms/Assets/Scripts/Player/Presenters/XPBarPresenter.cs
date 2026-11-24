using UnityEngine;
using UnityEngine.UI;
public class XPBarPresenter : AbstractXPPresenter
{
    [SerializeField]
    private Image xpBar;
    protected override void OnXPChange(int currentXp, int maxXP)
    {
        xpBar.fillAmount = (float)currentXp / (float)maxXP;
    }
}
