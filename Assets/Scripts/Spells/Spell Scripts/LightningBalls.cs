using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Lightning Balls")]
public class LightningBalls : SpellObject
{
    public float duration = 10f;
    public float damage = 20f;
    public AudioClip onCastSound;
    public AudioClip loopSound;

    public override bool Cast(GameObject caster)
    {
        var instance = Instantiate(instantiatedGO, caster.transform);
        instance.transform.localPosition = Vector3.zero;
        var dmgData = instance.GetComponent<DamageData>();
        dmgData.damage = damage;
        dmgData.caster = caster;
        dmgData.dmgSound = spellSound;
        var timer = instance.GetComponent<DestroyAfterTime>();
        timer.duration = duration;
        timer.remainingTime = duration;


        GameObject obj = new GameObject();
        obj.transform.position = caster.transform.position;
        var src = obj.AddComponent<AudioSource>();
        src.clip = onCastSound;
        src.Play();
        var destroy = obj.AddComponent<DestroyAfterTime>();
        destroy.duration = src.clip.length;
        destroy.remainingTime = destroy.duration;

        var soundData = instance.GetComponent<SoundData>();
        soundData.sound = loopSound;
        soundData.loop = true;

        return true;
    }
}
