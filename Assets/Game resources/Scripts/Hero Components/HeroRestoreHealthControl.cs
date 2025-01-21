using System.Collections;
using UnityEngine;

public class HeroRestoreHealthControl : MonoBehaviour
{
    [SerializeField] private EntityHealthControl _entityHealthControl;
    [SerializeField] private ParticleSystem _healEffect;

    private Coroutine _onRestoringCoroutine;
    private Coroutine _onCancelRestoringCoroutine;
    
    private float _delayCancelRestoring = 1.7f;
    private float _delayBetweenRestorePart = 0.7f;
    private int _valueHealthRestore = 50;
    
    public void StartRestoring()
    {
        if (_entityHealthControl.IsDead) return; 
        
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
        var waitTime = new WaitForSeconds(_delayBetweenRestorePart);
        yield return waitTime;

        while (true)
        {
            if (!_entityHealthControl.IsDead && _entityHealthControl.HealthPercent < 1)
            {
                _healEffect.Play();
                _entityHealthControl.TakeHeal(_valueHealthRestore);
            }
            else
            {
                _healEffect.Pause();
            }
            
            yield return waitTime;
        }
    }
    
    private IEnumerator OnCancelRestoring()
    {
        var waitTime = new WaitForSeconds(_delayCancelRestoring);
        yield return waitTime;
        
        //_healEffect.Stop();
        _healEffect.Pause();
        _healEffect.Clear();
        
        if (_onRestoringCoroutine != null) 
            StopCoroutine(_onRestoringCoroutine);

        _onRestoringCoroutine = null;
        _onCancelRestoringCoroutine = null;
    }
}