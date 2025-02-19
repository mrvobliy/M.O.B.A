using UnityEngine;

public class HeroPlayerMoveControl : MonoBehaviour
{
    [SerializeField] protected EntityComponentsData _componentsData;
    [SerializeField] private float _controllerMoveSpeed = 5f;
    [SerializeField] private float _controllerRotationSpeed = 10f;

    private const float GravityForce = -9.81f;
    private const float RotationSmoothTime = 0.1f;
    private const float BlendAttackLayerDuration = 0.3f;
    
    private Vector3 _targetDirection;
    private Vector3 _gravityDirection;
    private float _rotationSmoothVelocity;
    private bool _blendAttack;

    private void OnEnable() => enabled = !_componentsData.IsAi;

    private void Update()
    {
        if (!_componentsData.CanComponentsWork || _componentsData.IsDead) return;
        
        Move();
        Rotate();
        ApplyGravity();
        SetAnimatorLayers();
    }

    private void Move()
    {
        var x = Joystick.Instance.Direction.x;
        var z = Joystick.Instance.Direction.y;

        _targetDirection = new Vector3(x, 0f, z);

        if (_targetDirection.magnitude <= 0.01f) 
            return;
        
        _componentsData.CharacterController.Move(_targetDirection.normalized * _controllerMoveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (_targetDirection.magnitude <= 0.01f) 
            return;

        var targetAngle = Mathf.Atan2(_targetDirection.x, _targetDirection.z) * Mathf.Rad2Deg;
        var smoothAngle = Mathf.SmoothDampAngle(_componentsData.RotationRoot.eulerAngles.y, targetAngle, ref _rotationSmoothVelocity, RotationSmoothTime);
        _componentsData.RotationRoot.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
    }

    private void ApplyGravity()
    {
        if (_componentsData.CharacterController.isGrounded)
            _gravityDirection.y = -0.5f;
        else
            _gravityDirection.y += GravityForce * Time.deltaTime;

        _componentsData.CharacterController.Move(_gravityDirection * Time.deltaTime);
    }
    
    private void SetAnimatorLayers()
    {
        if (_targetDirection.magnitude <= 0.01f)
        {
            if (_blendAttack)
            {
                _blendAttack = false;
                _componentsData.Animator.DOLayerWeight(2, 0f, BlendAttackLayerDuration);
                _componentsData.Animator.DOLayerWeight(3, 1f, BlendAttackLayerDuration);
            }
            
            _componentsData.Animator.SetBool(AnimatorHash.IsRunning, false);
            _componentsData.Animator.SetFloat(AnimatorHash.Speed, _controllerMoveSpeed);
        }
        else
        {
            if (_blendAttack == false)
            {
                _blendAttack = true;
                _componentsData.Animator.DOLayerWeight(2, 1f, BlendAttackLayerDuration);
                _componentsData.Animator.DOLayerWeight(3, 0f, BlendAttackLayerDuration);
            }
            
            _componentsData.Animator.SetBool(AnimatorHash.IsRunning, true);
            _componentsData.Animator.SetFloat(AnimatorHash.Speed, _controllerMoveSpeed);
        }
    }
}