using UnityEngine;

[System.Serializable]
public class SpellSlot
{
    public void UpdateCooldown()
    {
        if (onCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
                onCooldown = false;
        }

        if (spellIconAtUI)
        {
            spellIconAtUI.UpdateRadialFill(cooldownPercent);
            spellIconAtUI.UpdateCooldownText(currentCooldown);
        }
    }

    public void UpdateGCD(bool onGCD, float val)
    {
        if(spellIconAtUI)
            spellIconAtUI.UpdateGCD(onGCD, val);
    }

    public void CastSpell(GameObject caster)
    {
        spell.Cast(caster);
        currentCooldown = spell.cooldown;
        onCooldown = true;
    }

    [HideInInspector] public SpellUI spellIconAtUI;
    [HideInInspector] public Sprite spellIcon { get => spell.spellIcon; }
    [HideInInspector] public Sprite spellIconBG { get => spell.spellIconBG; }
    [HideInInspector] public bool onCooldown = false;
    [HideInInspector] public float cooldownPercent { get => currentCooldown / spell.cooldown; }
    [HideInInspector] public float manaCost { get => spell.manaCost; }
    [HideInInspector] public bool isNull { get => spell == null; }
    [SerializeField] private SpellObject spell;
    private float currentCooldown;
}

public class SpellManager : MonoBehaviour, IParticipant
{
    public SpellSlot[] _availableSpells = new SpellSlot[6];
    private HealthAndMana status;

    [SerializeField] GlobalTurnManager _turnManager;
    private bool onGCD;

    public void CastSpell(int idx)
    {
        if (onGCD)
            return;

        if(idx >= 6 || idx < 0)
        {
            Debug.LogError("Trying to cast a spell at a invalid Index");
            return;
        }
        if (_availableSpells[idx].isNull)
        {
            Debug.LogError("No spell avaiable at index " + idx);
            return;
        }

        if (!_availableSpells[idx].onCooldown && _availableSpells[idx].manaCost <= status.currentMana)
        {
            status.currentMana -= _availableSpells[idx].manaCost;
            _availableSpells[idx].CastSpell(gameObject);

            onGCD = true;
        }
    }

    public void OnTurnPassed()
    {
        onGCD = false;
    }

    private void Start()
    {
        status = GetComponent<HealthAndMana>();
        _turnManager.Subscribe(this);
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
