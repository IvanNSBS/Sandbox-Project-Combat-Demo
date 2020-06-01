using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HealOverTime : MonoBehaviour
{

    [HideInInspector] public float healPerSecond = 10f;
    [HideInInspector] public HealthAndMana health;

    // Update is called once per frame
    void FixedUpdate()
    {
        health.currentHealth += healPerSecond * Time.fixedDeltaTime;
    }
}
