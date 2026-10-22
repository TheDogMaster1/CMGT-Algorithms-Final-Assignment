using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    public event Action<PlayerData, Enemy> onPlayerHit;

    private void GetHit(PlayerHitEvent playerHitEvent)
    {
        if (!playerData.invulnerable)
        {
            playerData.currentHp -= playerHitEvent.enemy.DamageData.damage;
            Debug.Log("Damaged");
            playerData.invulnerable = true;
            Invoke("StopInvulnerability", playerData.invulnerabilityTime);
        }
        onPlayerHit?.Invoke(playerData, playerHitEvent.enemy);
    }

    private void StopInvulnerability()
    {
        playerData.invulnerable = false;
    }

    private void OnEnable()
    {
        EventBus<PlayerHitEvent>.OnEventPublished += GetHit;
    }

    private void OnDisable()
    {
        EventBus<PlayerHitEvent>.OnEventPublished -= GetHit;
    }
}
