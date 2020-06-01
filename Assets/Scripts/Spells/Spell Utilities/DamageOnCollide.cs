using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollide: MonoBehaviour
{

    public DamageData dmgData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == dmgData.caster)
            return;

        var health = other.gameObject.GetComponent<HealthAndMana>();
        if(health)
            health.TakeDamage(dmgData.damage);
    }
}
