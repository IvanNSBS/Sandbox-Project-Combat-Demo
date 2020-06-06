using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AudioListenerVolumeController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private SliderData _data;

    private void OnEnable()
    {
        _slider.value = _data.volume;
        AudioListener.volume = _data.volume;

        _slider.onValueChanged.AddListener((val) => {
            _data.volume = val;
            AudioListener.volume = _data.volume;
        });
    }

    private void OnDisable() {
        _slider.onValueChanged.RemoveAllListeners();    
    }
}
