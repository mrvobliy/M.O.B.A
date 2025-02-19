using UnityEngine;

public class HeroHealthControl : EntityHealthControl
{
    [SerializeField] private int _deathAnimLayer;

    public void RestoreFullHeath()
    {
        _isDead = false;
        _currentHealth = _maxHealth.Value;
        OnHealthChanged?.Invoke();
    }

    protected override void StartDeath()
    {
        base.StartDeath();
        _componentsData.Animator.SetLayerWeight(_deathAnimLayer, 1);
        _componentsData.Animator.SetBool(AnimatorHash.IsDeath, true);
    }
}