using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugPlayerStatsSetter : MonoBehaviour
{
    [SerializeField] private HeroAttackControl _attackControl;
    [SerializeField] private IntVariable _normalDamage;
    [SerializeField] private IntVariable _extraDamage;
    [SerializeField] private TMP_Text _text1;
    [SerializeField] private Image _image1;
    [Space] 
    [SerializeField] private EntityHealthControl _healthControl;
    [SerializeField] private IntVariable _normalHealth;
    [SerializeField] private IntVariable _extraHealth;
    [SerializeField] private TMP_Text _text2;
    [SerializeField] private Image _image2;

    private bool _isNormalDamage = true;
    private bool _isNormalHealth = true;
    
    public void SetDamage()
    {
        if (_isNormalDamage)
        {
            _isNormalDamage = false;
            _attackControl.SetDamage(_extraDamage);
            
            _text1.text = "extra damage";
            _image1.color = new Color(1, 0.124f, 0, 0.5f);
        }
        else
        {
            _isNormalDamage = true;
            _attackControl.SetDamage(_normalDamage);
            
            _text1.text = "normal damage";
            _image1.color = new Color(0, 1, 0.09f, 0.5f);
        }
    }
    
    public void SetHealth()
    {
        if (_isNormalHealth)
        {
            _isNormalHealth = false;
            _healthControl.SetHealth(_extraHealth);
            
            _text2.text = "extra health";
            _image2.color = new Color(1, 0.124f, 0, 0.5f);
        }
        else
        {
            _isNormalHealth = true;
            _healthControl.SetHealth(_normalHealth);
            
            _text2.text = "normal health";
            _image2.color = new Color(0, 1, 0.09f, 0.5f);
        }
    }
}