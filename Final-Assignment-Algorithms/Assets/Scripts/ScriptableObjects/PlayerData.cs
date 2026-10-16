using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int maxHp;
    public int currentHp;

    private void OnEnable()
    {
        ResetHP();
    }

    public void ResetHP()
    {
        currentHp = maxHp;
    }
}
