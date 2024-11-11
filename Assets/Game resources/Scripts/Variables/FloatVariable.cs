using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    [SerializeField] private int _value;

    public int Value => _value;
    
    public void Set(int value)
    {
        _value = value;
    }
}
