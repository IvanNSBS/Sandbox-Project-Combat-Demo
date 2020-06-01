using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageData : MonoBehaviour
{
    public float damage = 10f;
    public GameObject caster; // ignore caster on collision
    public AudioClip dmgSound;
}
