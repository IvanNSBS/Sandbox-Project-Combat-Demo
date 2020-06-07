using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Spells/Volt Vari")]
public class VoltVari : SpellObject
{
    public float duration = 0.6f;
    public float damage = 120f;
    public float offset = 0.5f;

    public override bool PrepareCast(GameObject caster, SpellSlot slot = null){
        var mouse = caster.GetComponent<SpellCasterWithMouse>();
        mouse.areaRadius = 0f;
        mouse.spellToCast = slot;
        mouse.spellToCastIsRdy = true;

        return true;
    }

    public override bool Cast(GameObject caster)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 pointB = Vector3.zero;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            pointB = hit.point;
        }
        else
            return false;
        pointB = new Vector3(pointB.x, caster.transform.position.y, pointB.z);
        Vector3 direction = (pointB - caster.transform.position).normalized;
        var instance = Instantiate(instantiatedGO);
        instance.transform.position = caster.transform.position + direction*offset;
        instance.transform.rotation = Quaternion.FromToRotation(instance.transform.up, direction) * instance.transform.rotation;



        // Damaga and sound setup

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
