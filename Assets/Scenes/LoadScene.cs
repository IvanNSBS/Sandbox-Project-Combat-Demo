using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadMelee()
    {
        SceneManager.LoadScene("MeleeScene", LoadSceneMode.Single);
    }

    public void LoadCaster()
    {
        SceneManager.LoadScene("CasterScene", LoadSceneMode.Single);
    }
}
