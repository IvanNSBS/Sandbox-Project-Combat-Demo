﻿using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Spells/Weapon Swing")]
public class WeaponSwing : SpellObject
{
    [Header("Damage")]
    [Range(0, 5)] public float weaponDmgIncreaseInPct = 1.7f;

    [Header("FX Properties")]
    public float duration = 1f;
    public Vector3 spawnOffset = Vector3.zero;

    private GameObject instantiatedFX;
    public override bool Cast(GameObject caster){

        CombatAttacker _casterAtk = caster.GetComponent<CombatAttacker>();
        if(!_casterAtk)
            return false; 

        var fx = Instantiate(instantiatedGO);
        fx.transform.position = caster.transform.position + spawnOffset;

        var dmgData = fx.GetComponent<DamageData>();
        var dmgOnCol = fx.GetComponent<DamageOnCollide>();
        var destroy = fx.GetComponent<DestroyAfterTime>();
        
        destroy.duration = duration;
        destroy.remainingTime = duration;

        dmgData.damage = _casterAtk.weapon.atkDamage * weaponDmgIncreaseInPct;
        dmgData.caster = caster;

        dmgOnCol.dmgData = dmgData;

        if(spellSound)
            GameplayUtils.SpawnSound(spellSound, caster.transform.position);

        float shakeDuration = 0.2f;
        float strenght = 0.65f;
        int vibrato = 20;
        Camera.main.DOShakePosition(shakeDuration, strenght, vibrato, 45, true);

        return true;
    }
}
