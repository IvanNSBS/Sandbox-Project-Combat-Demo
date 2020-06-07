using UnityEngine;

public class BlockShineSelect : MonoBehaviour
{
    private Material _mat;
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        _mat = GetComponentInChildren<MeshRenderer>().material;
    }

    public void ToggleShining(bool val){
        if(!_mat)
            return;

        _mat.SetInt("_Toggle", val ? 1 : 0);
    }
}
