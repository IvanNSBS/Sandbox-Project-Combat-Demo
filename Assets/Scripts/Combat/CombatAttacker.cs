using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CombatAttacker : MonoBehaviour, IParticipant
{

    [SerializeField] private GlobalTurnManager _turnManager;
    [SerializeField] private float _atkDamage = 20f;
    [SerializeField] private float _atkRange = 3f;
    [SerializeField] List<AudioClip> _dmgSounds;
    [SerializeField] GameObject _doPunchObject; // object DOTween will animate with punch function
    [SerializeField] SpriteRenderer _sprite; // reference to sprite render so AnimateAttack can be animated
    [SerializeField] GameObject _attackEffect;

    private Material _spriteMaterial;
    private HealthAndMana _target;
    private bool _canAttack = true;
    

    public float atkRange { get => _atkRange; }
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
            target.TakeDamage(_atkDamage);
            
            if (_dmgSounds.Count > 0)
            {
                int count = _dmgSounds.Count - 1;
                int idx = Random.Range(0, count);

                GameplayUtils.SpawnSound(_dmgSounds[idx], gameObject.transform.position);
            }

            if (_attackEffect)
            {
                var atk = Instantiate(_attackEffect);
                atk.transform.position = target.transform.position;
            }
        });
    }

    public void TryAttack()
    {
        if(target != null && _canAttack)
        {
            if (Vector3.Distance(target.gameObject.transform.position, gameObject.transform.position) <= _atkRange)
            {
                AnimateAndAttack();
                _canAttack = false;
            }
        }
    }

    private void Update()
    {
        if (target != null)
            TryAttack();
    }

    private void Start()
    {
        _spriteMaterial = _sprite.material;   
    }

    public void OnTurnPassed()
    {
        _canAttack = true;
    }
}
