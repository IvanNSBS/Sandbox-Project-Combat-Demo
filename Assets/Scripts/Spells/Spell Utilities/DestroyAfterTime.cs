using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    public float duration = 5f;
    [HideInInspector] public float remainingTime;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = duration;
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
            Destroy(gameObject);
    }
}
