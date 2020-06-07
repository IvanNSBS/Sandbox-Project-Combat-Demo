using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class CombatAttacker : MonoBehaviour, IParticipant
{

    [Header("Animation")]
    [SerializeField] GameObject _doPunchObject; // object DOTween will animate with punch function
    [SerializeField] SpriteRenderer _sprite; // reference to sprite render so AnimateAttack can be animated


    [Header("Data")]
    [SerializeField] private GlobalTurnManager _turnManager;
    [SerializeField] public Weapon weapon;


    [Header("UI")]
    [SerializeField] GameObject _slotUI;
    [SerializeField] GameObject _spellUIPrefab;


    [HideInInspector] public int currentCharges {get => weaponSlot.currentCharges; } 


    private SpellSlot weaponSlot;
    private Material _spriteMaterial;
    private HealthAndMana _target;
    private bool _canAttack = true;
    
    public float atkRange {
        get {
            if (weapon != null)
                return weapon.atkRange;

            return -1f;
        } 
    }

    public HealthAndMana target {
        get => _target; 
        set {
            _target = value;
            TargetUpdated(value);
        }
    }

    private void TargetUpdated(HealthAndMana newTarget)
    {
        if (newTarget == null)
            _turnManager.Unsubscribe(this);

        else if(!_turnManager.HasParticipant(this))
            _turnManager.Subscribe(this);
    }

    private void AnimateAndAttack()
    {
        _spriteMaterial.SetFloat("_Lerp", 0.5f);
        Sequence sq = DOTween.Sequence();
        sq.Append(_spriteMaterial.DOFloat(1f, "_Lerp", 0.4f));
        sq.InsertCallback(0f, () => {
            Vector3 direction = target.transform.position - gameObject.transform.position;
            direction = direction.normalized;
            float sine = 0.7071f;
            float cosine = 0.7071f;
            direction = new Vector3(direction.x * cosine + -direction.z * sine, direction.y, direction.x * sine + direction.z * cosine);
            _doPunchObject.transform.DOPunchPosition(direction, 0.2f, 15, 0, false); 
        });

        sq.InsertCallback(0f, () => {
            weaponSlot.PrepareCast(gameObject);

            GameplayUtils.SpawnSound(weapon.spellSound, gameObject.transform.position);

            if (weapon.instantiatedGO)
            {
                var atk = Instantiate(weapon.instantiatedGO);
                WeaponFXFollow fxFollow = atk.GetComponent<WeaponFXFollow>();
                if(fxFollow){
                    fxFollow.transform.position = gameObject.transform.position;
                    fxFollow.MoveTo(target.gameObject.transform.position);
                }
                else
                    atk.transform.position = target.transform.position;
            }
        });
    }

    public bool TryAttack()
    {
        if(weaponSlot.currentCharges == 0)
            return false;

        if(target != null && _canAttack)
        {
            if (Vector3.Distance(target.gameObject.transform.position, gameObject.transform.position) <= atkRange)
            {
                AnimateAndAttack();
                _canAttack = weaponSlot.currentCharges > 0;
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        if(_slotUI != null){
            weaponSlot.UpdateGCD(weaponSlot.currentCharges == 0, 1f - _turnManager.nextTurnProgress);
            weaponSlot.spellIconAtUI.UpdateChargesText(weaponSlot.currentCharges, true);

            if (weaponSlot.currentCharges == 0)
                weaponSlot.spellIconAtUI.SetIconColor(new Color(0.4f, 0.4f, 0.4f));
            else
                weaponSlot.spellIconAtUI.SetIconColor(Color.white);
        }
    }

    public void RefreshCharges(){
        weaponSlot.ResetCharges();
    }

    private void Start()
    {
        _spriteMaterial = _sprite.material;

        if(weapon != null){
            weaponSlot = new SpellSlot(weapon);

            if(_slotUI != null){
                var uiSlot = Instantiate(_spellUIPrefab, _slotUI.transform);
                uiSlot.GetComponent<SpellUI>().SetIconAndBorder(weapon.spellIconBG, weapon.spellIcon);
                weaponSlot.spellIconAtUI = uiSlot.GetComponent<SpellUI>();
                weaponSlot.spellIconAtUI.UpdateCooldownText(0f);
                weaponSlot.spellIconAtUI.UpdateRadialFill(0f);
            }
        }
    }

    public void OnTurnPassed()
    {
        weaponSlot.ResetCharges();
        _canAttack = weapon.charges > 0;
    }
}
