using UnityEngine;

public class TargetFPS : MonoBehaviour
{
    private void OnEnable()
    {
        Application.targetFrameRate = 60;
    }
}
