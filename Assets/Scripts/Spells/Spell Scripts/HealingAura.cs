using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing Aura")]
public class HealingAura : SpellObject
{
    public float duration = 10f;
    public float totalHeal = 100f;

    public override void Cast(GameObject caster)
    {
        float deltaHeal = totalHeal / duration;

        var instance = Instantiate(instantiatedGO, caster.transform);
        instance.transform.localPosition = Vector3.zero;

        //Set Heal Data
        var healData = instance.GetComponent<HealOverTime>();
        healData.health = caster.GetComponent<HealthAndMana>();
        healData.healPerSecond = deltaHeal;

        //Set Timer Data
        var timer = instance.GetComponent<DestroyAfterTime>();
        timer.duration = duration;
        timer.remainingTime = duration;

        //Set Sound Object
        GameObject obj = new GameObject();
        obj.transform.position = caster.transform.position;
        var src = obj.AddComponent<AudioSource>();
        src.clip = spellSound;
        src.Play();
        var destroy = obj.AddComponent<DestroyAfterTime>();
        destroy.duration = src.clip.length;
        destroy.remainingTime = destroy.duration;
    }
}
