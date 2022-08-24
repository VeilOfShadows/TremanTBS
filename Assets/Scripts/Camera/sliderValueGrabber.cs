using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sliderValueGrabber : MonoBehaviour
{

    public Slider slider;
    public float sliderValue;
    public float finalValue;
    public GameObject cameraUpdater;
    public TextMeshProUGUI text;

    public void UpdateSliderValue()
    {
        sliderValue = slider.value;
        finalValue = sliderValue + 0.5f;
        cameraUpdater.GetComponent<CameraSwapper>().transitionTimeUpdate(finalValue);
        
    }

    public void UpdateUI()
    {
        sliderValue = slider.value;
        text.text = sliderValue.ToString();
    }

}
