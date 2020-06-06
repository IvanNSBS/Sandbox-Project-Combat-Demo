using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Volume/Master Volue Data")]
public class SliderData : ScriptableObject
{
    [Range(0f, 1f)]public float volume = 0.75f; 
}
