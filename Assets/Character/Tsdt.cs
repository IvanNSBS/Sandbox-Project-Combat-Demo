using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tsdt : MonoBehaviour
{

    public Camera camera;
    public float duration = 1f;
    public float strenght = 2;
    public int vibrato = 8;
    // Start is called before the first frame update
    void Start()
    {
        camera.DOShakePosition(0.5f, 1, 8, 45, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            camera.DOShakePosition(duration, strenght, vibrato, 45, true);

    }
}
