using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _gravityForce;
    [SerializeField] private Joystick _joystick;

    private CharacterController _controller;
    private Vector3 _targetDirection;
    private Vector3 _gravityDirection;
    private float _inputAngle;
    private float _rotationSmoothVelocity;
    
    private const float LockAngleValue = 0.0f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CreateTargetDirection();
        Move();
        SetGravity();
    }
    
    private void Move()
    {
        if (_targetDirection.magnitude <= 0.01f) return;
        
        _controller.Move(_targetDirection * (Time.deltaTime * _moveSpeed));
        Rotate();
    }

    private void CreateTargetDirection()
    {
        _targetDirection = new Vector3(_joystick.Direction.x, 0.0f, _joystick.Direction.y).normalized;  
    }

    private void Rotate()
    {
        _inputAngle = Mathf.Atan2(_targetDirection.x, _targetDirection.z) * Mathf.Rad2Deg;
        
        var targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
            _inputAngle, ref _rotationSmoothVelocity, _rotationSpeed / 10);
        
        transform.rotation = Quaternion.Euler(LockAngleValue, targetAngle, LockAngleValue);
    }

    private void SetGravity()
    {
        _gravityDirection.y += _gravityForce * Time.deltaTime;
        _controller.Move(_gravityDirection * _moveSpeed);
    }
}
