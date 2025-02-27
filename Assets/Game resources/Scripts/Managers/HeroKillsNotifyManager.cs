using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKillsNotifyManager : MonoBehaviour
{
    [SerializeField] private List<NotifyKillView> _killViews;
    
    private List<NotifyPullObject> _notifyWaitPull;
    private Coroutine _onShowNotifyCoroutine;

    private void OnEnable()
    {
        _notifyWaitPull = new List<NotifyPullObject>();
        EventsBase.HeroKillHero += AddNotifyInPull;
    }

    private void OnDisable() => EventsBase.HeroKillHero += AddNotifyInPull;
    
    private void AddNotifyInPull(EntityComponentsData killer, EntityComponentsData dead)
    {
        _notifyWaitPull.Add(new NotifyPullObject(killer, dead));
        StartOnShowNotify();
    }

    private void StartOnShowNotify() => _onShowNotifyCoroutine ??= StartCoroutine(OnShowNotify());

    private IEnumerator OnShowNotify()
    {
        if (_notifyWaitPull.Count <= 0) 
            yield break;
        
        var waitTime = new WaitForSeconds(0.1f);
        var isAllViewBusy = false;

        while (!isAllViewBusy && _notifyWaitPull.Count > 0)
        {
            isAllViewBusy = true;
            var notify = _notifyWaitPull[^1];
            
            foreach (var view in _killViews)
            {
                if (view.IsBusy) 
                    continue;

                view.SetInfo(notify.Killer, notify.Dead);
                view.Show();
                isAllViewBusy = false;
                _notifyWaitPull.Remove(notify);
                break;
            }

            yield return waitTime;
        }
        
        _killViews[^1].SetShowCallback(StartOnShowNotify);
        _onShowNotifyCoroutine = null;
    }
}

[Serializable]
public class NotifyPullObject
{
    public EntityComponentsData Killer;
    public EntityComponentsData Dead;

    public NotifyPullObject(EntityComponentsData killer, EntityComponentsData dead)
    {
        Killer = killer;
        Dead = dead;
    }
}