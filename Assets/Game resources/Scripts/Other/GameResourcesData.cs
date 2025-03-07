using UnityEngine;

[CreateAssetMenu]
public class GameResourcesData : ScriptableObject
{
    [SerializeField] private GameObject _stunEffect;

    public GameObject StunEffect => _stunEffect;
}
