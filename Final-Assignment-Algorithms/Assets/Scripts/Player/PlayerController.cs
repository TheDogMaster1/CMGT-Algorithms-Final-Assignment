using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    public Action<PlayerData, Enemy> OnPlayerHit;

    private void OnHit(PlayerHitEvent playerHitEvent)
    {
        if (!playerData.invulnerable)
        {
            playerData.currentHp -= playerHitEvent.enemy.DamageData.damage;
        Debug.Log("Damaged");
        }
        OnPlayerHit?.Invoke(playerData, playerHitEvent.enemy);
            playerData.invulnerable = true;
        Invoke("StopInvulnerability", playerData.invulnerabilityTime);
    }

    private void StopInvulnerability()
    {
        playerData.invulnerable = false;
    }

    private void OnEnable()
    {
        EventBus<PlayerHitEvent>.OnEventPublished += OnHit;
    }

    private void OnDisable()
    {
        EventBus<PlayerHitEvent>.OnEventPublished -= OnHit;
    }
}
