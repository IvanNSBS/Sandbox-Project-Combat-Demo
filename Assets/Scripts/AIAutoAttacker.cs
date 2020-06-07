using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAutoAttacker : MonoBehaviour, IParticipant
{
    [SerializeField] CombatAttacker _atker;    
    [SerializeField] GlobalTurnManager _turnManager;
    [SerializeField] float _maxDelayBetweenAttacks;


    private float _curTime;
    private bool _canAttack = true;

    void Update()
    {
        if(!_canAttack)
            return;

        if(_curTime > 0f)
            _curTime -= Time.deltaTime;

        if(_curTime <= 0f){
            bool attacked = _atker.TryAttack();
            if(attacked){
                _curTime = Random.Range(0.3f, _maxDelayBetweenAttacks);
                _canAttack = _atker.currentCharges > 0;
            }
        }
    }

    void Start()
    {
        _curTime = _maxDelayBetweenAttacks;
        _turnManager.Subscribe(this);        
    }

    public void OnTurnPassed(){
        _canAttack = true;
    }
}
