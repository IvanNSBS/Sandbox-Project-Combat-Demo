using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AISpellCaster : MonoBehaviour, IParticipant
{
    [SerializeField] SpellManager _spellManager;
    [SerializeField] CombatAttacker _attacker;
    [SerializeField] GlobalTurnManager _turnManager;
    [SerializeField] GameObject _spriteObject; // user for dotween animations
    [SerializeField] int _maxTurnsWithoutCasting = 1; // max number of turns without casting spells

    private int _curTurnsWithoutCasting;

    int RandomNumberOfActionlessTurns()
    {
        return Random.Range(0, _maxTurnsWithoutCasting);
    }

    // Start is called before the first frame update
    void Start()
    {
        _turnManager.Subscribe(this);
        _curTurnsWithoutCasting = RandomNumberOfActionlessTurns();
    }

    void TryCastRandomSpell()
    {
        if (_curTurnsWithoutCasting > 0 || _attacker.target == null)
            return;

        int size = _spellManager._availableSpells.Length;
        List<int> possibleIdxs = new List<int>();
        for (int i = 0; i < size; i++)
            possibleIdxs.Add(i);

        while(possibleIdxs.Count > 0)
        {
            int randomIdx = Random.Range(possibleIdxs[0], possibleIdxs[possibleIdxs.Count - 1]);
            possibleIdxs.Remove(randomIdx);
            if (!_spellManager._availableSpells[randomIdx].onCooldown)
            {
                AnimateAndCast(randomIdx);
                break;
            }
        }
    }

    void AnimateAndCast(int spellIdx)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append( _spriteObject.transform.DOLocalMoveY(1.2f, 0.5f, false));
        sq.Join( _spriteObject.GetComponent<SpriteRenderer>().material.DOFloat(0.15f, "_Lerp", 0.25f));
        sq.AppendInterval(0.4f);
        sq.Append( _spriteObject.transform.DOLocalMoveY(0f, 0.07f, false));
        sq.Join(_spriteObject.GetComponent<SpriteRenderer>().material.DOFloat(1f, "_Lerp", 0.1f));
        sq.InsertCallback(1.25f,() => { 
            _spellManager.CastSpell(spellIdx);
            _curTurnsWithoutCasting = RandomNumberOfActionlessTurns();
        });

    }

    // Update is called once per frame
    void Update()
    {
        if (_turnManager.nextTurnProgress > 0.8f)
            TryCastRandomSpell();
    }

    public void OnTurnPassed()
    {
        _curTurnsWithoutCasting--;
        Debug.Log("Cur turns without casting: " + _curTurnsWithoutCasting);
    }
}
