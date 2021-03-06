﻿using UnityEngine;

[System.Serializable]
public class SpellSlot
{
    public void UpdateCooldown()
    {
        if (currentCharges < spell.charges)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                currentCharges++;
                onCooldown = false;
                if(currentCharges < spell.charges)
                    currentCooldown = spell.cooldown;
            }
        }

        if (spellIconAtUI)
        {
            spellIconAtUI.UpdateRadialFill(cooldownPercent);
            spellIconAtUI.UpdateCooldownText(currentCooldown);
            spellIconAtUI.UpdateChargesText(currentCharges, spell.charges > 1);
        }
    }

    public void UpdateGCD(bool onGCD, float val)
    {
        if(spellIconAtUI)
            spellIconAtUI.UpdateGCD(onGCD, val);
    }

    public void ResetCharges(){
        currentCharges = spell.charges;
    }

    public bool PrepareCast(GameObject caster)
    {
        if(currentCharges == 0)
            return false;

        if(currentCharges == spell.charges)
            currentCooldown = spell.cooldown;

        currentCharges--;
        if(currentCharges == 0)
            onCooldown = true;

        return spell.PrepareCast(caster, this);
    }

    public bool CastSpell(GameObject caster, bool updateStuff = false){

        if(updateStuff){
            if(currentCharges == 0)
                return false;

            if(currentCharges == spell.charges)
                currentCooldown = spell.cooldown;

            currentCharges--;
            if(currentCharges == 0)
                onCooldown = true;
        }

        return spell.Cast(caster);
    }

    public SpellSlot(){}
    public SpellSlot(SpellObject sp){
        spell = sp;
        ResetCharges();
    }

    [HideInInspector] public SpellUI spellIconAtUI;
    [HideInInspector] public Sprite spellIcon { get => spell.spellIcon; }
    [HideInInspector] public Sprite spellIconBG { get => spell.spellIconBG; }
    [HideInInspector] public bool onCooldown = false;
    [HideInInspector] public float cooldownPercent { get => currentCooldown / spell.cooldown; }
    [HideInInspector] public float manaCost { get => spell.manaCost; }
    [HideInInspector] public bool isNull { get => spell == null; }
    [HideInInspector] public int currentCharges = 1;
    [SerializeField] private SpellObject spell;
    private float currentCooldown = 0f;
}

public class SpellManager : MonoBehaviour, IParticipant
{
    public SpellSlot[] _availableSpells = new SpellSlot[6];
    private HealthAndMana status;

    [SerializeField] GlobalTurnManager _turnManager;
    private bool onGCD;

    public bool CastSpell(int idx)
    {
        if (onGCD)
            return false;

        if(idx >= 6 || idx < 0)
        {
            Debug.LogError("Trying to cast a spell at a invalid Index");
            return false;
        }
        if (_availableSpells[idx].isNull)
        {
            Debug.LogError("No spell avaiable at index " + idx);
            return false;
        }

        if (!_availableSpells[idx].onCooldown && _availableSpells[idx].manaCost <= status.currentMana)
        {
            
            if(_availableSpells[idx].PrepareCast(gameObject)){
                status.currentMana -= _availableSpells[idx].manaCost;
                onGCD = true;
                return true;
            }
        }

        return false;
    }

    public void OnTurnPassed()
    {
        onGCD = false;
    }

    private void Start()
    {
        status = GetComponent<HealthAndMana>();
        _turnManager.Subscribe(this);

        foreach(SpellSlot slot in _availableSpells){
            slot.ResetCharges();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(SpellSlot slot in _availableSpells)
        {
            slot.UpdateCooldown();
            slot.UpdateGCD(onGCD, 1f - _turnManager.nextTurnProgress);

            if (slot.spellIconAtUI)
            {
                if (slot.manaCost > status.currentMana)
                    slot.spellIconAtUI.SetIconColor(new Color(0.4f, 0.4f, 0.4f));
                else
                    slot.spellIconAtUI.SetIconColor(Color.white);
            }
        }
    }
}
