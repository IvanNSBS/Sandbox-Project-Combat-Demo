using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeOnCollide : MonoBehaviour
{

    public float strenght = 0.375f;
    public float shakeDuration = 0.12f;
    public int vibrato = 14;
    public DamageData dmgData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == dmgData.caster)
            return;

        Camera.main.DOShakePosition(shakeDuration, strenght, vibrato, 45, true);
    }
}
