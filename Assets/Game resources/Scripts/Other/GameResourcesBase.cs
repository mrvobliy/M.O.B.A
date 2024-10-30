using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class GameResourcesBase : ScriptableObject
{
    [SerializeField] private GameObject _stunEffect;

    public GameObject StunEffect => _stunEffect;

    private static GameResourcesBase _instance;

    public static GameResourcesBase Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.LoadAll<GameResourcesBase>("Data/").FirstOrDefault();
                
                if (!_instance) 
                    Debug.LogWarning("Could not find instance of " + typeof(GameResourcesBase));
            }

            return _instance;
        }
    }
}
