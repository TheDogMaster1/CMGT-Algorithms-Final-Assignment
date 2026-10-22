using TMPro;
using UnityEngine;

public class MoneyPresenter : MonoBehaviour
{
    [SerializeField]
    private IntValue money;
    [SerializeField]
    private TextMeshProUGUI moneyText;

    private void OnEnemyDied(EnemyDieEvent enemyDieEvent)
    {
        money.value += enemyDieEvent.enemy.Money;
    }

    private void UpdateText()
    {
        moneyText.text = "€" + money.value.ToString();
    }

    private void Start()
    {
        UpdateText();
    }

    private void OnEnable()
    {
        EventBus<EnemyDieEvent>.OnEventPublished += OnEnemyDied;
        money.onValueChange += UpdateText;
    }
    private void OnDisable()
    {
        EventBus<EnemyDieEvent>.OnEventPublished -= OnEnemyDied;
        money.onValueChange -= UpdateText;
    }
}
