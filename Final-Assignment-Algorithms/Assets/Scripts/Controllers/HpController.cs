using System;
using UnityEngine;

public class HpController : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    public static Action<PlayerData> OnPlayerHit;

    public void OnHit(int damage)
    {
        playerData.currentHp -= damage;
        OnPlayerHit?.Invoke(playerData);
        Debug.Log("Damaged");
    }

}
