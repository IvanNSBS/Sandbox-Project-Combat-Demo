using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFollow : MonoBehaviour
{
    [SerializeField] Movement _actorMovement;
    [SerializeField] CombatAttacker _actorCombat;
    [SerializeField] float _sightRadius;
    [SerializeField] MaterialManager _materialManager;

    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _sightRadius, 1 << 8);
        bool foundSomething = false;
        foreach(var col in colliders)
        {
            HealthAndMana target = col.gameObject.GetComponent<HealthAndMana>();
            if (target)
            {
                foundSomething = true;
                if(target != _actorCombat.target)
                    _actorCombat.target = target;
                break;
            }
        }
        if (!foundSomething)
        {
            _actorCombat.target = null;
            _actorMovement.moveDir = Vector3.zero;
        }
        else
        {
            _actorMovement.moveDir = (_actorCombat.target.transform.position - transform.position).normalized;
            if(Vector3.Distance(transform.position, _actorCombat.target.transform.position) < _actorCombat.atkRange)
            {
                _actorMovement.moveDir = Vector3.zero;
            }
        }

        UpdateDirection();
    }

    void UpdateDirection()
    {
        if (_rb.velocity.magnitude == 0f)
            return;

        bool flipX = true;
        int verticalDirection = -1;
        if (_rb.velocity.x < 0)
            flipX = false;
        if (_rb.velocity.z < 0)
            verticalDirection = 1;

        _materialManager.UpdateSpriteAndMaterial(verticalDirection, flipX);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _actorCombat.atkRange);
    }
}
