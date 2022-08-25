using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueGrabber : MonoBehaviour
{
    [SerializeField]
    Slider slider;
    [SerializeField]
    float sliderValue;
    [SerializeField]
    TextMeshProUGUI text;

    float finalValue;

    public void UpdateSliderValue()
    {
        sliderValue = slider.value;
        finalValue = sliderValue + 0.5f;
        Camera.main.GetComponent<CameraController>().transitionTimeUpdate(finalValue);
        
    }

    public void UpdateUI()
    {
        sliderValue = slider.value;
        text.text = sliderValue.ToString();
    }

}
