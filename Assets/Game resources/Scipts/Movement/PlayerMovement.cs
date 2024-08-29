using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _attackRotationSpeed;
    [SerializeField] private float _gravityForce;
    [SerializeField] private float _forceMove;
    [Space]
    [SerializeField] private Joystick _joystick;

    private CharacterController _controller;
    private Vector3 _inputDirection;
    private Vector3 _gravityDirection;
    private float _inputAngle;
    private float _rotationSmoothVelocity;
    private float _targetMagnitude;
    
    private const float LockAngleValue = 0.0f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        SetGravity();
        CreateTargetDirection();
    }
    
    private void Move()
    {
        if (_inputDirection.magnitude <= 0.01f)
        {
            VariableBase.IsRunState = false;
            return;
        }

        VariableBase.IsRunState = true;
        _controller.Move(transform.forward * (Time.deltaTime * (_moveSpeed / 10)));
        Rotate();
    }

    private void CreateTargetDirection()
    {
        _inputDirection = new Vector3(_joystick.Direction.x, 0.0f, _joystick.Direction.y);
    }

    private void Rotate()
    {
        var normalizeDir = _inputDirection.normalized;
        _inputAngle = Mathf.Atan2(normalizeDir.x, normalizeDir.z) * Mathf.Rad2Deg;
        
        var targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
            _inputAngle, ref _rotationSmoothVelocity, _rotationSpeed / 100);
        
        transform.rotation = Quaternion.Euler(LockAngleValue, targetAngle, LockAngleValue);
    }

    private void SetGravity()
    {
        _gravityDirection.y += _gravityForce * Time.deltaTime;
        _controller.Move(_gravityDirection * _moveSpeed);
    }
}
