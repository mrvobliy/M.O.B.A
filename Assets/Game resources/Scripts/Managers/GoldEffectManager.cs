using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoldEffectManager : MonoBehaviour
{
    [SerializeField] private List<GoldEffect> _goldEffectsPool;
    
    public static GoldEffectManager Instance;

    private void Awake() => Instance = this;
    
    public void SpawnEffect(Transform point, int value)
    {
        var effect = _goldEffectsPool.FirstOrDefault(x => !x.gameObject.activeSelf);
        
        if (effect == null) return;
        
        effect.gameObject.SetActive(true);
        effect.Play(point, value);
    }
}