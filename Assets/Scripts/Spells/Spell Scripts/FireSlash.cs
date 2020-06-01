using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.InteropServices;

[CreateAssetMenu(menuName = "Spells/Fire Slash")]
public class FireSlash : SpellObject
{
    [Header("Slash")]
    public AudioClip slashSound;
    public int numOfSlashes = 3;
    public float delayBetweenSlashes = 0.3f;
    public float slashLifetime = 10f;
    public float damage = 25f;
    public float slashSpeed = 50f;
    [Header("SpawnEffect")]
    public float spawnFXDuration = 0.5f;
    public GameObject spawnFXPrefab;

    public override void Cast(GameObject caster)
    {
        GameObject target = caster.GetComponent<CombatAttacker>().target.gameObject;
        if (!target)
            return;

        Vector3 pointB = target.transform.position;
        pointB = new Vector3(pointB.x, caster.transform.position.y, pointB.z);
        Vector3 direction = (pointB - caster.transform.position).normalized;

        Sequence sq = DOTween.Sequence();

        for(int i = 0; i < numOfSlashes; i++)
        {
            sq.AppendCallback(() => { 
                var instance = Instantiate(instantiatedGO);
                instance.transform.rotation = Quaternion.FromToRotation(instance.transform.up, -direction) * instance.transform.rotation;
                instance.transform.position = caster.transform.position;
                //slash movement
                var movement = instance.GetComponent<MoveOverTime>();
                movement.moveDir = direction;
                movement.moveSpeed = slashSpeed;
                // Damaga and sound setup
                var timer = instance.GetComponent<DestroyAfterTime>();
                timer.duration = slashLifetime;
                timer.remainingTime = slashLifetime;
                var dmgData = instance.GetComponent<DamageData>();
                dmgData.damage = damage;
                dmgData.caster = caster;
                var dmgCol = instance.GetComponent<DamageOnCollide>();
                dmgCol.dmgData = dmgData;

                var fxInstance = Instantiate(spawnFXPrefab);
                fxInstance.transform.position = caster.transform.position;
                fxInstance.transform.rotation = Quaternion.FromToRotation(instance.transform.up, -direction) * instance.transform.rotation;
                var fxTimer = fxInstance.GetComponent<DestroyAfterTime>();
                fxTimer.duration = spawnFXDuration;
                fxTimer.remainingTime = spawnFXDuration;

                GameplayUtils.SpawnSound(spellSound, caster.transform.position);
                GameplayUtils.SpawnSound(slashSound, caster.transform.position);
            });
            sq.AppendInterval(delayBetweenSlashes);

        }
    }
}
