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
    [SerializeField] Weapon _weapon;


    [Header("UI")]
    [SerializeField] GameObject _slotUI;
    [SerializeField] GameObject _spellUIPrefab;



    private SpellSlot weaponSlot;
    private Material _spriteMaterial;
    private HealthAndMana _target;
    private bool _canAttack = true;
    
    public float atkRange {
        get {
            if (_weapon != null)
                return _weapon.atkDamage;

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
        Sequence sq = DOTween.Sequence();
        sq.Append(_spriteMaterial.DOFloat(0.5f, "_Lerp", 0.15f));
        sq.AppendInterval(0.5f);
        sq.Append(_spriteMaterial.DOFloat(1f, "_Lerp", 0.4f));

        sq.InsertCallback(0.65f, () => {
            Vector3 direction = target.transform.position - gameObject.transform.position;
            direction = direction.normalized;
            float sine = 0.7071f;
            float cosine = 0.7071f;
            direction = new Vector3(direction.x * cosine + -direction.z * sine, direction.y, direction.x * sine + direction.z * cosine);
            _doPunchObject.transform.DOPunchPosition(direction, 0.2f, 15, 0, false); 
        });

        sq.InsertCallback(0.65f, () => {
            target.TakeDamage(_weapon.atkDamage);
            // if (_dmgSounds.Count > 0)
            // {
            //     int count = _dmgSounds.Count - 1;
            //     int idx = Random.Range(0, count);

            //     GameplayUtils.SpawnSound(_dmgSounds[idx], gameObject.transform.position);
            // }

            GameplayUtils.SpawnSound(_weapon.spellSound, gameObject.transform.position);

            if (_weapon.instantiatedGO)
            {
                var atk = Instantiate(_weapon.instantiatedGO);
                atk.transform.position = target.transform.position;
            }
        });
    }

    public void TryAttack()
    {
        weaponSlot.CastSpell(gameObject);
        if(target != null && _canAttack)
        {
            if (Vector3.Distance(target.gameObject.transform.position, gameObject.transform.position) <= atkRange)
            {
                AnimateAndAttack();
                _canAttack = weaponSlot.currentCharges == 0;
            }
        }
    }

    private void Update()
    {
        // if (target != null)
        //    TryAttack();

        weaponSlot.UpdateCooldown();
        weaponSlot.UpdateGCD(!_canAttack, 1f - _turnManager.nextTurnProgress);
        
        if (weaponSlot.spellIconAtUI)
        {
            weaponSlot.spellIconAtUI.SetIconColor(Color.white);
        }
    }

    private void Start()
    {
        _spriteMaterial = _sprite.material;

        if(_weapon != null){
            weaponSlot = new SpellSlot(_weapon);

            if(_slotUI != null){
                var uiSlot = Instantiate(_spellUIPrefab, _slotUI.transform);
                uiSlot.GetComponent<SpellUI>().SetIconAndBorder(_weapon.spellIconBG, _weapon.spellIcon);
                weaponSlot.spellIconAtUI = uiSlot.GetComponent<SpellUI>();
            }
        }
    }

    public void OnTurnPassed()
    {
        _canAttack = _weapon.charges > 0;
    }
}
