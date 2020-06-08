using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDescriptions : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject meleeDescription;
    [SerializeField] GameObject casterDescription;
    [SerializeField] bool meleeIsDefaultDescription = true;
    

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if(meleeIsDefaultDescription){
            casterDescription.SetActive(false);
            meleeDescription.SetActive(true);
        }
        else{
            casterDescription.SetActive(true);
            meleeDescription.SetActive(false);
        }
    }

    public void UseMeleeDescription(){
        casterDescription.SetActive(false);
        meleeDescription.SetActive(true);
    }

    public void UseCasterDescription(){
        casterDescription.SetActive(true);
        meleeDescription.SetActive(false);
    }

}
