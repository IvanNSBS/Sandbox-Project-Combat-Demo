using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Weapon")]
public class Weapon : SpellObject
{
    public float atkDamage;
    public float atkRange;

    public override bool Cast(GameObject caster){
        HealthAndMana target = caster.GetComponent<CombatAttacker>().target;
            if(target){
            target.TakeDamage(atkDamage);
            return true;
        }
        return false;
    }
}
