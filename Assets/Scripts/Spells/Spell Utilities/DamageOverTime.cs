using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime: MonoBehaviour
{
    [HideInInspector] public float dmg = 0f;
    [HideInInspector] public float tickDelay = 0f;
    [HideInInspector] public HealthAndMana target;
    private float currentTickTime = 0f;
    void Update()
    {
        if(dmg == 0f || target == null)
            return;

        currentTickTime += Time.deltaTime;
        if(currentTickTime >= tickDelay){
            target.TakeDamage(dmg);
            currentTickTime = 0f;
        }
    }
}
