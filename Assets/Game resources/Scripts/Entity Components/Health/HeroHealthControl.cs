using UnityEngine;

public class HeroHealthControl : EntityHealthControl
{
    [SerializeField] private int _deathAnimLayer;

    protected override int _healthBase => _componentsData.HeroStatsControl.Health;
    protected override int _armorBase => _componentsData.HeroStatsControl.Armor;

    public void RestoreFullHeath()
    {
        _isDead = false;
        _currentHealth = _healthBase;
        OnHealthChanged?.Invoke();
    }

    protected override void StartDeath()
    {
        base.StartDeath();
        _componentsData.Animator.SetLayerWeight(_deathAnimLayer, 1);
        _componentsData.Animator.SetBool(AnimatorHash.IsDeath, true);
    }
}