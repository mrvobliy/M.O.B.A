using UnityEngine;

public class OnAttackEvent : MonoBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;

    public void Attack() => _playerAttack.SetDamage();
}
