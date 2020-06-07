using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Spells/Rupture")]
public class Rupture : SpellObject
{
    [Header("Damage")]
    public float totalDamage = 5f;
    public float tickDelay = 0.5f;

    [Header("Duration")]
    public float duration = 10f;

    private GameObject instantiatedFX;
    public override bool Cast(GameObject caster){
        CombatAttacker _casterAtk = caster.GetComponent<CombatAttacker>();
        if(!_casterAtk)
            return false;

        HealthAndMana target = _casterAtk.target;

        if(!target)
            return false;

        float dmgPerTick = totalDamage / (duration/tickDelay);
        instantiatedFX = Instantiate(instantiatedGO, target.gameObject.transform);
        instantiatedFX.transform.localPosition = Vector3.zero;

        Sequence sq = DOTween.Sequence();

        sq.AppendCallback(() => {
            DamageOverTime dot = instantiatedFX.AddComponent<DamageOverTime>();
            dot.dmg = dmgPerTick;
            dot.tickDelay = tickDelay;
            dot.target = target;

            GameplayUtils.SpawnSound(spellSound, target.transform.position);

        });

        sq.AppendInterval(duration);

        sq.AppendCallback( () => {
            Destroy(instantiatedFX);
        } );

        return true;
    }
}
