using System;
using UnityEngine;

public class TargetFPS : MonoBehaviour
{
    [SerializeField] private float _timeScale;
    
    private void OnEnable()
    {
        Application.targetFrameRate = 120;
    }

    private void FixedUpdate()
    {
        Time.timeScale = _timeScale;
    }
}
