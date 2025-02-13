using UnityEngine;

public class HeroPlayerMoveControl : MonoBehaviour
{
    [SerializeField] protected EntityComponentsData _entityComponentsData;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _rotationRoot;
    
    [SerializeField] private float _controllerMoveSpeed = 5f;
    [SerializeField] private float _controllerRotationSpeed = 10f;

    private const float GravityForce = -9.81f;
    private const float RotationSmoothTime = 0.1f;
    private const float BlendAttackLayerDuration = 0.3f;

    private Animator Animator => _entityComponentsData.EntityHealthControl.Animator;
    
    private Vector3 _targetDirection;
    private Vector3 _gravityDirection;
    private float _rotationSmoothVelocity;
    private bool _blendAttack;

    private void OnEnable()
    {
         _characterController.enabled = !_entityComponentsData.IsAi;
         _entityComponentsData.EntityHealthControl.Collider.enabled = _entityComponentsData.IsAi;
         enabled = !_entityComponentsData.IsAi;
    }

    private void Update()
    {
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
        
        _characterController.Move(_targetDirection.normalized * _controllerMoveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (_targetDirection.magnitude <= 0.01f) 
            return;

        var targetAngle = Mathf.Atan2(_targetDirection.x, _targetDirection.z) * Mathf.Rad2Deg;
        var smoothAngle = Mathf.SmoothDampAngle(_rotationRoot.eulerAngles.y, targetAngle, ref _rotationSmoothVelocity, RotationSmoothTime);
        _rotationRoot.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded)
            _gravityDirection.y = -0.5f;
        else
            _gravityDirection.y += GravityForce * Time.deltaTime;

        _characterController.Move(_gravityDirection * Time.deltaTime);
    }
    
    private void SetAnimatorLayers()
    {
        if (_targetDirection.magnitude <= 0.01f)
        {
            if (_blendAttack)
            {
                _blendAttack = false;
                Animator.DOLayerWeight(2, 0f, BlendAttackLayerDuration);
                Animator.DOLayerWeight(3, 1f, BlendAttackLayerDuration);
            }
            
            Animator.SetBool(AnimatorHash.IsRunning, false);
            Animator.SetFloat(AnimatorHash.Speed, _controllerMoveSpeed);
        }
        else
        {
            if (_blendAttack == false)
            {
                _blendAttack = true;
                Animator.DOLayerWeight(2, 1f, BlendAttackLayerDuration);
                Animator.DOLayerWeight(3, 0f, BlendAttackLayerDuration);
            }
            
            Animator.SetBool(AnimatorHash.IsRunning, true);
            Animator.SetFloat(AnimatorHash.Speed, _controllerMoveSpeed);
        }
    }
}