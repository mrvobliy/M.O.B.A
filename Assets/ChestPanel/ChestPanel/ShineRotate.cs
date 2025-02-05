using UnityEngine;

public class ShineRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    private void Update() => transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
}
