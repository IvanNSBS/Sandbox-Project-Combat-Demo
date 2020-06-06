using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Spells/Bull Rush")]
public class BullRush : SpellObject
{
    [Header("Rush Properties")]
    public float damage = 1.7f;
    public float stunDuration = 3f;
    public float dashDuration = 0.5f;
    public float distanceForMaxDuration = 2f;
    public float punchDuration = 0.12f;
    [Range(0f, 1f)]public float punchAtkRangeMult = 0.3f;

    [Header("FX Properties")]
    public float fxDuration = 1f;
    public Vector3 effectOffset = Vector3.zero;


    public override void Cast(GameObject caster){

        CombatAttacker _casterAtk = caster.GetComponent<CombatAttacker>();
        if(!_casterAtk)
            return;

        if(!_casterAtk.target)
            return;

        HealthAndMana target = _casterAtk.target;

        float atkRange = _casterAtk.weapon.atkRange;
        Vector3 dir = (target.transform.position - caster.transform.position);
        float actualMaxDistance = distanceForMaxDuration - dir.normalized.magnitude*atkRange;

        float distance = Vector3.Distance(target.transform.position, caster.transform.position);
        float lerp = Mathf.Clamp01(distance/actualMaxDistance);


        GameplayUtils.SpawnSound(spellSound, caster.transform.position);

        Sequence callbacks = DOTween.Sequence();
        var fx = Instantiate(instantiatedGO, target.transform);
        callbacks.AppendCallback( () => {
            target.GetComponent<AIFollow>().ToggleActive(false);
            target.GetComponent<AISpellCaster>().enabled = false;

            fx.transform.localPosition = effectOffset;

        } );
        callbacks.AppendInterval(stunDuration);
        callbacks.AppendCallback( () => {
            target.GetComponent<AIFollow>().ToggleActive(true);
            target.GetComponent<AISpellCaster>().enabled = true;

            Destroy(fx);
        } );

        Sequence sq = DOTween.Sequence();
        sq.Append( caster.transform.DOMove(target.transform.position - dir.normalized, dashDuration*lerp, false) );
        sq.Append( caster.transform.DOPunchPosition(dir.normalized*atkRange*punchAtkRangeMult, punchDuration, 11, 0f, false) );
        sq.Insert(0f, callbacks);

    }
}
