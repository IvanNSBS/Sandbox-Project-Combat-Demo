using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Movement _movement;
    [SerializeField] private Transform _rightDir;
    [SerializeField] private Transform _upDir;

    [SerializeField] private Image _UISpellsParent;
    [SerializeField] private GameObject _spellSlotPrefab;
    [SerializeField] private bool _spriteLookAtTarget = false;

    private CombatAttacker _attacker;
    private SpellManager _spellManager;
    private MaterialManager _matManager;
    private List<KeyCode> _pressedBtns = new List<KeyCode>();

    void Start()
    {
        _matManager = GetComponent<MaterialManager>();
        _movement = GetComponent<Movement>();
        _spellManager = GetComponent<SpellManager>();
        _attacker = GetComponent<CombatAttacker>();

        if (_spellManager)
        {
            for(int i = 0; i < 5; i++)
            {
                if (_spellManager._availableSpells[i].isNull)
                    continue;

                var uiSlot = Instantiate(_spellSlotPrefab, _UISpellsParent.transform);
                var spell = _spellManager._availableSpells[i];
                uiSlot.GetComponent<SpellUI>().SetIconAndBorder(spell.spellIconBG, spell.spellIcon);
                _spellManager._availableSpells[i].spellIconAtUI = uiSlot.GetComponent<SpellUI>();
            }
        }

    }

    KeyCode PeekLastBtn()
    {
        if (_pressedBtns.Count == 0)
            return KeyCode.None;
        else
            return _pressedBtns[_pressedBtns.Count - 1];
    }

    void AddPressedBtn(KeyCode k)
    {
        if (_pressedBtns.Contains(k))
            return;

        _pressedBtns.Add(k);
    }

    void RemovePressedBtn(KeyCode k)
    {
        if (_pressedBtns.Contains(k))
            _pressedBtns.Remove(k);
    }

    void UpdateSpriteFromTargetDir()
    {
        Vector3 direction = _attacker.target.gameObject.transform.position - gameObject.transform.position;
        direction = direction.normalized;

        // 
        int vertical = 1;
        bool flipX = false;

        if (direction.x > 0)
            flipX = true;
        if (direction.z > 0)
            vertical = -1;

        _matManager.UpdateSpriteAndMaterial(vertical, flipX);
    }

    void UpdateMovement()
    {
        KeyCode k = PeekLastBtn();
        if (k == KeyCode.None)
        {
            _movement.moveDir = Vector3.zero;
            return;
        }

        if (k == KeyCode.W)
        {
            _movement.moveDir = (_upDir.position - gameObject.transform.position).normalized;
            if (_attacker.target == null || !_spriteLookAtTarget)
                _matManager.UpdateSpriteAndMaterial(-1, true);
            else
                UpdateSpriteFromTargetDir();
        }
        else if (k == KeyCode.A)
        {
            _movement.moveDir = -(_rightDir.position - gameObject.transform.position).normalized;
            if (_attacker.target == null || !_spriteLookAtTarget)
                _matManager.UpdateSpriteAndMaterial(-1, false);
            else
                UpdateSpriteFromTargetDir();
        }


        else if (k == KeyCode.S)
        {
            _movement.moveDir = -(_upDir.position - gameObject.transform.position).normalized;
            if (_attacker.target == null || !_spriteLookAtTarget)
                _matManager.UpdateSpriteAndMaterial(1, false);
            else
                UpdateSpriteFromTargetDir();
        }
        else if (k == KeyCode.D)
        {
            _movement.moveDir = (_rightDir.position - gameObject.transform.position).normalized;
            if (_attacker.target == null || !_spriteLookAtTarget)
                _matManager.UpdateSpriteAndMaterial(1, true);
            else
                UpdateSpriteFromTargetDir();
        }


    }

    private BlockShineSelect _selected = null;

    void CheckForSelectedBlock(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            var tempSelected = hit.collider.gameObject.GetComponent<BlockShineSelect>();
            if(!tempSelected && _selected){ //didn't find but selected is not null
                _selected.ToggleShining(false);
                _selected = null;
                return;
            }

            if(tempSelected && !_selected){ // find and nothing's selected
                _selected = tempSelected;
                _selected.ToggleShining(true);
            }
            else{ // find and something's selected
                _selected.ToggleShining(false);
                _selected = tempSelected;
                _selected.ToggleShining(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.W))
                AddPressedBtn(KeyCode.W);

            else if (Input.GetKeyDown(KeyCode.A))
                AddPressedBtn(KeyCode.A);

            else if (Input.GetKeyDown(KeyCode.S))
                AddPressedBtn(KeyCode.S);

            else if (Input.GetKeyDown(KeyCode.D))
                AddPressedBtn(KeyCode.D);
        }

        {
            if (Input.GetKeyUp(KeyCode.W))
                RemovePressedBtn(KeyCode.W);

            else if (Input.GetKeyUp(KeyCode.A))
                RemovePressedBtn(KeyCode.A);

            else if (Input.GetKeyUp(KeyCode.S))
                RemovePressedBtn(KeyCode.S);

            else if (Input.GetKeyUp(KeyCode.D))
                RemovePressedBtn(KeyCode.D);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            _spellManager.CastSpell(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            _spellManager.CastSpell(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            _spellManager.CastSpell(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            _spellManager.CastSpell(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            _spellManager.CastSpell(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            _spellManager.CastSpell(5);

        if(Input.GetKeyDown(KeyCode.Space))
            _attacker.TryAttack();


        UpdateMovement();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction, Color.blue, 10f);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                HealthAndMana target = hit.collider.gameObject.GetComponent<HealthAndMana>();
                if (target && target != GetComponent<HealthAndMana>() )
                {
                    // no targer right now
                    if(_attacker.target == null)
                    {
                        _attacker.target = target;
                        _attacker.target.statusHighlightImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (target == _attacker.target) 
                        {
                            _attacker.target.statusHighlightImage.gameObject.SetActive(false);
                            _attacker.target = null;
                        }
                        else
                        {
                            _attacker.target = target;
                            _attacker.target.statusHighlightImage.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        CheckForSelectedBlock();
    }
}
