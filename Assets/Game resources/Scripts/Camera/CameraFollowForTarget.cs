using UnityEngine;

public class CameraFollowForTarget : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Transform _target;

    private const float Magnitude = 6f;

    private void Update()
    {
        if (_target == null) return;
        
        transform.position = Vector3.Lerp(transform.position, _target.transform.position + _offset, Time.deltaTime * Magnitude);
    }
}
