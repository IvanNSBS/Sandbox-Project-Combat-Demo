using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Ball Lightning")]
public class BallLightning : SpellObject
{
    public float duration = 25f;
    public float damage = 80f;
    public float ballSpeed = 110f;
    public AudioClip castSound;

    public override bool Cast(GameObject caster)
    {
        var instance = Instantiate(instantiatedGO);
        instance.transform.position = caster.transform.position;

        var ballMov = instance.GetComponent<MoveOverTime>();
        ballMov.moveSpeed = ballSpeed;

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
        ballMov.moveDir = pointB - caster.transform.position;

        var dmgData = instance.GetComponent<DamageData>();
        dmgData.damage = damage;
        dmgData.caster = caster;
        dmgData.dmgSound = spellSound;

        var dmgOnCol = instance.GetComponent<DamageAndDestroy>();
        dmgOnCol.dmgData = dmgData;
        dmgOnCol.destroyOnHit = instance;

        var timer = instance.GetComponent<DestroyAfterTime>();
        timer.duration = duration;
        timer.remainingTime = duration;


        GameObject obj = new GameObject();
        obj.transform.position = caster.transform.position;
        var src = obj.AddComponent<AudioSource>();
        src.clip = castSound;
        src.Play();
        var destroy = obj.AddComponent<DestroyAfterTime>();
        destroy.duration = src.clip.length;
        destroy.remainingTime = destroy.duration;

        return true;
    }
}
