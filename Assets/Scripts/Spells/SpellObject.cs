using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpellObject : ScriptableObject
{
    public string spellName = "No Name";
    public float cooldown = 0f;
    public float manaCost = 25f;
    [Range(1, 15)] public int charges = 1;
    public Sprite spellIcon;
    public Sprite spellIconBG;
    public GameObject instantiatedGO;
    public AudioClip spellSound;

    public abstract bool Cast(GameObject caster);
}
