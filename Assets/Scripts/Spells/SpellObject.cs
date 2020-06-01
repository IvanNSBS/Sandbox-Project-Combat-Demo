using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpellObject : ScriptableObject
{
    public string spellName = "No Name";
    public float cooldown = 0f;
    public float manaCost = 25f;
    public Sprite spellIcon;
    public Sprite spellIconBG;
    public GameObject instantiatedGO;
    public AudioClip spellSound;

    public abstract void Cast(GameObject caster);
}
