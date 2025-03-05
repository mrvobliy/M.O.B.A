using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class IKWeightController : MonoBehaviour
{
    public IKController ikController;
    public float transitionDuration = 1.0f;
    
    private Coroutine weightCoroutine;
    
    [Button]
    public void StartIK(float duration)
    {
        if (weightCoroutine != null)
            StopCoroutine(weightCoroutine);
        
        weightCoroutine = StartCoroutine(ChangeIKWeight(1f, duration));
    }
    
    [Button]
    public void StopIK(float duration)
    {
        if (weightCoroutine != null)
            StopCoroutine(weightCoroutine);
        
        weightCoroutine = StartCoroutine(ChangeIKWeight(0.0f, duration));
    }
    
    private IEnumerator ChangeIKWeight(float targetWeight, float duration)
    {
        float startWeight = ikController.ikWeight;
        float weightDifference = Mathf.Abs(targetWeight - startWeight);
        float adjustedDuration = duration * weightDifference;
        float elapsedTime = 0f;

        while (elapsedTime < adjustedDuration)
        {
            elapsedTime += Time.deltaTime;
            ikController.ikWeight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / adjustedDuration);
            yield return null;
        }

        ikController.ikWeight = targetWeight;
    }
}