using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
    [SerializeField] private int _value;

    public int Value => _value;

    private void Awake()
    {
        _value = PlayerPrefs.GetInt(name, _value);
    }

    public void Set(int value)
    {
        _value = value;
        PlayerPrefs.SetInt(name, value);
    }
}
