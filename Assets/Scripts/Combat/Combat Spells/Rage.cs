using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Spells/Rage")]
public class Rage : SpellObject
{
    [Header("Damage")]
    public float flatBonusDamage = 40f;
    public int bonusAtkPerTurn = 1;

    [Header("Duration")]
    public float duration = 5f;


    private GameObject fxInstance;
    public override void Cast(GameObject caster)
    {
        CombatAttacker atkComponent = caster.GetComponent<CombatAttacker>();
        if(!atkComponent)
            return;

        Sequence sq = DOTween.Sequence();
        sq.AppendCallback(() => {
            atkComponent.weapon.charges += 1;
            atkComponent.weapon.atkDamage += flatBonusDamage;
            atkComponent.RefreshCharges();

            //spawn rage fx here
            fxInstance = Instantiate(instantiatedGO, caster.transform);
            fxInstance.transform.localPosition = Vector3.zero;
        });
        sq.AppendInterval(duration);
        sq.AppendCallback(() => {
            atkComponent.weapon.charges -= 1;
            atkComponent.weapon.atkDamage -= flatBonusDamage;

            Destroy(fxInstance);
            //destroy rage fx here
        });
    }
}
