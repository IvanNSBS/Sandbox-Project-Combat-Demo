using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Spells/Defensive Stance")]
public class DefensiveStance : SpellObject
{
    [Header("General")]
    public float duration = 10f;

    [Header("Defense Values")]
    [Range(0, 1)] public float reducedDmgTakenInPercent = 0.5f;


    [Header("Heal Value")]
    [Range(0,1)] public float healPercentOfMitigatedDmg = 0.35f;


    private GameObject fxInstance;
    public override void Cast(GameObject caster)
    {
        HealthAndMana stats = caster.GetComponent<HealthAndMana>();
        if(!stats)
            return;

        Sequence sq = DOTween.Sequence();

        sq.AppendCallback(() => {
            stats.resistancePct = reducedDmgTakenInPercent;

            fxInstance = Instantiate(instantiatedGO, caster.transform);
            fxInstance.transform.localPosition = Vector3.zero;
        });
        sq.AppendInterval(duration);
        sq.AppendCallback(() => {
            stats.HealAndResetMitigatedDmg(healPercentOfMitigatedDmg);
            stats.resistancePct = 0f;

            Destroy(fxInstance);
        });

        GameplayUtils.SpawnSound(spellSound, caster.transform.position);
    }
}
