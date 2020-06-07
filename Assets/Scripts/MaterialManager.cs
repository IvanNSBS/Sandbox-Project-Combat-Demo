using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;


    private Rigidbody _rb;
    public Material material {get => _spriteRenderer.material; set => _spriteRenderer.material = value; }

    /// <summary>
    /// W = North, but flipped
    /// A = North, not flipped
    /// S = South, not flipped
    /// D = South, but flipped
    /// </summary>
    /// <param name="vertical"></param>
    /// <param name="horizontal"></param>
    public void UpdateSpriteAndMaterial(int verticalDir, bool flipX)
    {
        _animator.SetInteger("verticalDirection", -verticalDir);
        //if(verticalDir < 0)
        //    _cutoutMaterial.SetTexture("_BaseMap", _northSpriteSheet.texture);
        //else
        //    _cutoutMaterial.SetTexture("_BaseMap", _southSpriteSheet.texture);

        _spriteRenderer.flipX = flipX;
    }

    public void SetMaterialFloat(string propertyName, float value){
        _spriteRenderer.material.SetFloat(propertyName, value);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //_animator = GetComponent<Animator>();
        //_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("walking", _rb.velocity.magnitude > 0);
    }
}
