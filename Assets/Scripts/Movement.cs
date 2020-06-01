using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public Vector3 moveDir = Vector3.zero;
    [SerializeField] public float _speed = 10f;

    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = _speed * moveDir * Time.deltaTime;
    }
}
