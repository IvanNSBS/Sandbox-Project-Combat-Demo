using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthAndMana : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _maxHealth = 150f;
    [SerializeField] float _maxMana = 150f;
    [SerializeField] float _passiveManaRegen = 0.5f;
    [SerializeField] float _passiveHealthRegen = 0.5f;
    [SerializeField] bool _neverUseDefaultDeath = false;
    [SerializeField] UnityEvent Die;
    [SerializeField] Image _healthImage;
    [SerializeField] Image _manaImage;
    public Image statusHighlightImage;
    public float currentHealth { get => _curHealth; set => _curHealth = Mathf.Clamp(value, -1f, _maxHealth); }
    public float currentMana { get => _curMana; set => _curMana = Mathf.Clamp(value, -1f, _maxMana); }
    public float remainingHealthPct { get => _curHealth / _maxHealth; }
    public float remainingManaPct { get => _curMana / _maxMana; }

    private float _curHealth;
    private float _curMana;


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
            Die.Invoke();
    }

    void DefaultDie()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        _curHealth = _maxHealth;
        _curMana = _maxMana;

        if (Die.GetPersistentEventCount() == 0 && !_neverUseDefaultDeath)
        {
            Debug.Log("No Custom for " + gameObject + ". Using Default");
            Die.AddListener(DefaultDie);
        }
    }

    private void Update()
    {
        _healthImage.fillAmount = remainingHealthPct;
        _manaImage.fillAmount = remainingManaPct;

        currentHealth += _passiveHealthRegen * Time.deltaTime;
        currentMana += _passiveManaRegen * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
            TakeDamage(15f);
    }
}
