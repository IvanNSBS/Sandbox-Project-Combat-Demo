using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/DummySpell")]
public class DummySpell : SpellObject
{
    public override bool Cast(GameObject caster)
    {
        Debug.Log("Casted Dummy Spell");
        return true;
    }
}
