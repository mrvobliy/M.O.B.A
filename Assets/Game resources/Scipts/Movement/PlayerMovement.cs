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
    [SerializeField] private Animator _animator;

    private CharacterController _controller;
    private Vector3 _inputDirection;
    private Vector3 _gravityDirection;
    private float _inputAngle;
    private float _rotationSmoothVelocity;
    private float _targetMagnitude;
    private bool _isAttackState;
    
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

    public void SetAttackState(bool isAttackState)
    {
        if (_isAttackState.Equals(isAttackState)) return;
        
        _isAttackState = isAttackState;
        _animator.SetBool(AnimatorHash.IsAttack, _isAttackState);
        _animator.SetLayerWeight(1, _isAttackState ? 0.7f : 0);
    }

    public void RotateToEnemy(Transform target)
    {
        var normalizeDir = (target.position - transform.position).normalized;
        _inputAngle = Mathf.Atan2(normalizeDir.x, normalizeDir.z) * Mathf.Rad2Deg;
        
        var targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
            _inputAngle, ref _rotationSmoothVelocity, _attackRotationSpeed / 100);
        
        transform.rotation = Quaternion.Euler(LockAngleValue, targetAngle, LockAngleValue);
    }
    
    private void Move()
    {
        _animator.SetFloat(AnimatorHash.Speed, _inputDirection.magnitude);
        
        if (_inputDirection.magnitude <= 0.01f) return;

        _controller.Move(transform.forward * (Time.deltaTime * (_moveSpeed / 10)));
        Rotate();
    }

    private void CreateTargetDirection()
    {
        _inputDirection = new Vector3(_joystick.Direction.x, 0.0f, _joystick.Direction.y);
    }

    private void Rotate()
    {
        if (_isAttackState) return;
        
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
