using UnityEngine;

public class SpellCasterWithMouse : MonoBehaviour
{
    [HideInInspector] public SpellSlot spellToCast = null;
    [HideInInspector] public float areaRadius;
    [HideInInspector] public bool spellToCastIsRdy = false;

    [SerializeField] Color radiusColor = Color.yellow;
    private Vector3? mousePos = null;
    private BlockShineSelect _selected = null;

    Vector3? FindBlockToCast(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 9))
        {
            var tempSelected = hit.collider.gameObject.GetComponent<BlockShineSelect>();
            if(!tempSelected && _selected){ //didn't find but selected is not null
                _selected.ToggleShining(false);
                _selected = null;
                return null;
            }
            if(!tempSelected)
                return null;

            if(tempSelected && !_selected){ // find and nothing's selected
                _selected = tempSelected;
                _selected.ToggleShining(true);
                return hit.point;
            }
            else{ // find and something's selected
                _selected.ToggleShining(false);
                _selected = tempSelected;
                _selected.ToggleShining(true);
                return hit.point;
            }
        }

        return null;
    }

    public bool Cast(){
        if(!spellToCastIsRdy)
            return false;

        if(mousePos == null)
            return false;

        bool retrn = spellToCast.CastSpell(gameObject);
        if(retrn)
            spellToCastIsRdy = false;
        return retrn;
    }

    private void Update() {
        if(spellToCastIsRdy){
            mousePos = FindBlockToCast();

            if(Input.GetKeyDown(KeyCode.Mouse0)){
                spellToCast.CastSpell(gameObject);
                spellToCast = null;
            }
        }
    }
}
