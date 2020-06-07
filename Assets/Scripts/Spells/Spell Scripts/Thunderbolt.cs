using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Spells/Thunderbolt")]
public class Thunderbolt : SpellObject
{
    public float duration = 2.5f;
    public float damage = 20f;

    public override bool PrepareCast(GameObject caster, SpellSlot slot = null){
        var mouse = caster.GetComponent<SpellCasterWithMouse>();
        mouse.areaRadius = 0.9f;
        mouse.spellToCast = slot;
        mouse.spellToCastIsRdy = true;

        return true;
    }

    public override bool Cast(GameObject caster)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 spawnPos = Vector3.zero;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            spawnPos = hit.point;
        }
        else
            return false;

        var instance = Instantiate(instantiatedGO);
        instance.transform.position = spawnPos;

        var timer = instance.GetComponent<DestroyAfterTime>();
        timer.duration = duration;
        timer.remainingTime = duration;

        var dmgData = instance.GetComponent<DamageData>();
        dmgData.damage = damage;
        dmgData.caster = caster;

        var dmgCol = instance.GetComponent<DamageOnCollide>();
        dmgCol.dmgData = dmgData;

        GameObject obj = new GameObject();
        obj.transform.position = caster.transform.position;
        var src = obj.AddComponent<AudioSource>();
        src.clip = spellSound;
        src.Play();
        var destroy = obj.AddComponent<DestroyAfterTime>();
        destroy.duration = src.clip.length;
        destroy.remainingTime = destroy.duration;

        float shakeDuration = 0.2f;
        float strenght = 0.65f;
        int vibrato = 20;
        Camera.main.DOShakePosition(shakeDuration, strenght, vibrato, 45, true);
        
        return true;
    }
}
