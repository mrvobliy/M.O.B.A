using System;
using UnityEngine;

public class HeroSpawnControl : MonoBehaviour
{
    [SerializeField] private Transform _lightSideSpawnPoint;
    [SerializeField] private Transform _darkSideSpawnPoint;
    [SerializeField] private ParticleSystem _lightSideSpawnEffect;
    [SerializeField] private ParticleSystem _darkSideSpawnEffect;

    public static HeroSpawnControl Instance;

    public Transform LightSideSpawnPoint => _lightSideSpawnPoint;
    public Transform DarkSideSpawnPoint => _darkSideSpawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayEffect(Team team)
    {
        _lightSideSpawnEffect?.Play();
        
        switch (team)
        {
            case Team.Light:
                _lightSideSpawnEffect?.Play();
                break;
            
            case Team.Dark:
                _darkSideSpawnEffect?.Play();
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(team), team, null);
        }
    }

    public Transform GetPoint(Team team)
    {
        switch (team)
        {
            case Team.Light:
                return _lightSideSpawnPoint;
            
            case Team.Dark:
                return _darkSideSpawnPoint;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(team), team, null);
        }
    }
}
