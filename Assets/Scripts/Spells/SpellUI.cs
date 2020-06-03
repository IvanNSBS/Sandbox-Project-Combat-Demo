using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellUI : MonoBehaviour
{
    [SerializeField] private Image _border;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _radialFill;
    [SerializeField] private Image _radialFillGCD;
    [SerializeField] TMPro.TextMeshProUGUI _text;
    [SerializeField] TMPro.TextMeshProUGUI _chargesText;
    public void SetIconAndBorder(Sprite border, Sprite icon)
    {
        if(_border && _icon)
        {
            _border.sprite = border;
            _icon.sprite = icon;
        }
    }

    public void UpdateRadialFill(float val)
    {
        if(_radialFill){
            _radialFill.fillAmount = val;
            _radialFill.enabled = val > 0f;
        }
    }

    public void UpdateGCD(bool onGCD, float val)
    {
        if (_radialFillGCD)
        {
            _radialFillGCD.enabled = onGCD;
            _radialFillGCD.fillAmount = val;
        }
    }

    public void UpdateChargesText(int val, bool display = false){
        if(!display)
            _chargesText.text = string.Empty;
        else
            _chargesText.text = val.ToString();
    }

    public void UpdateCooldownText(float val)
    {
        if (!_text)
            return;

        if (val <= 0f)
            _text.enabled = false;
        else
            _text.enabled = true;
        _text.text = val.ToString("F1");
    }

    public void SetIconColor(Color color)
    {
        if(_icon)
            _icon.color = color;
    }
}
