﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

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

    // Start is called before the first frame update
    void Start()
    {
        //_animator = GetComponent<Animator>();
        //_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}