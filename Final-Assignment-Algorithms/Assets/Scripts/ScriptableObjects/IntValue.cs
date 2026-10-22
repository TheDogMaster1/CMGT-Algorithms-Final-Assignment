using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntValue", menuName = "Scriptable Objects/IntValue")]
public class IntValue : ScriptableObject
{
    public int initialValue;

    private int _value;

    public int value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                onValueChange?.Invoke();
            }
        }
    }

    public Action onValueChange;

    private void OnEnable()
    {
        value = initialValue;
    }
}
