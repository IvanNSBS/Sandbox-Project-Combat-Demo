﻿using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Weapon")]
public class Weapon : SpellObject
{
    public float atkDamage;
    public float atkRange;

    public override void Cast(GameObject caster){
        HealthAndMana target = caster.GetComponent<CombatAttacker>().target;
        Debug.Log("Target is valid? " + target == null ? "No" : "Yes");
        if(target){
            target.TakeDamage(atkDamage);
        }
    }
}
