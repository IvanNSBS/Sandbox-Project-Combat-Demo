using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOverTime : MonoBehaviour
{

    public float moveSpeed = 50f;
    public Vector3 moveDir = new Vector3(1f, 0, 0);

    private Rigidbody _rb;
    // Update is called once per frame
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _rb.velocity = moveDir * moveSpeed * Time.deltaTime;
    }
}
