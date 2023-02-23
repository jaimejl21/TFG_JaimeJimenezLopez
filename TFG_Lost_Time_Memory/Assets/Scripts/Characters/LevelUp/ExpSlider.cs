using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExpSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderTxt;
    public LevelUpMananager lum;

    public int max, value;

    public void UpdateValue(int exp)
    {
        value = exp;
        slider.value = value;
        UpdateExpTxt();
    }

    public void UpdateMaxValue()
    {
        slider.maxValue = max;
        UpdateExpTxt();
    }

    public void UpdateValues(int exp, int maxValue)
    {
        value = exp;
        max = maxValue;
        slider.maxValue = max;
        slider.value = value;
        UpdateExpTxt();
    }

    void UpdateExpTxt()
    {
        sliderTxt.text = "" + value + " / " + max;
    }
}
