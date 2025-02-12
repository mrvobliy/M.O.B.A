using System.Collections;
using UnityEngine;

public class HeroRestoreHealthControl : MonoBehaviour
{
    private const float DelayCancelRestoring = 1.7f;
    private const float DelayBetweenRestorePart = 0.7f;
    private const int ValueHealthRestore = 50;
    
    [SerializeField] private EntityComponentsData _componentsData;
    [SerializeField] private ParticleSystem _healEffect;

    public EntityComponentsData ComponentsData => _componentsData;
    
    private Coroutine _onRestoringCoroutine;
    private Coroutine _onCancelRestoringCoroutine;

    public void StartRestoring()
    {
        if (_componentsData.IsDead) return; 
        
        if (_onCancelRestoringCoroutine != null)
        {
            StopCoroutine(_onCancelRestoringCoroutine);
            return;
        }
        
        _onRestoringCoroutine = StartCoroutine(OnRestoring());
    }
    
    public void StopRestoring()
    {
        if (_onCancelRestoringCoroutine != null) 
            StopCoroutine(_onCancelRestoringCoroutine);
        
        if (_onRestoringCoroutine == null) return;
        
        _onCancelRestoringCoroutine = StartCoroutine(OnCancelRestoring());
    }
    
    private IEnumerator OnRestoring()
    {
        var waitTime = new WaitForSeconds(DelayBetweenRestorePart);
        yield return waitTime;

        while (true)
        {
            if (!_componentsData.IsDead && _componentsData.EntityHealthControl.HealthPercent < 1)
            {
                _healEffect.Play();
                _componentsData.EntityHealthControl.TakeHeal(ValueHealthRestore);
            }
            else
            {
                _healEffect.Pause();
                _healEffect.Clear();
            }
            
            yield return waitTime;
        }
    }
    
    private IEnumerator OnCancelRestoring()
    {
        var waitTime = new WaitForSeconds(DelayCancelRestoring);
        yield return waitTime;
        
        _healEffect.Pause();
        _healEffect.Clear();
        
        if (_onRestoringCoroutine != null) 
            StopCoroutine(_onRestoringCoroutine);

        _onRestoringCoroutine = null;
        _onCancelRestoringCoroutine = null;
    }
}