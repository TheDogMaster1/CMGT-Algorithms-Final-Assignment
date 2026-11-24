using UnityEngine;

public abstract class AbstractXPPresenter : MonoBehaviour
{
    protected abstract void OnXPChange(int currentXp, int maxXP);

    private void OnEnable()
    {
        PlayerLevelController.onXpChange += OnXPChange;
    }

    private void OnDisable()
    {
        PlayerLevelController.onXpChange -= OnXPChange;
    }
}
