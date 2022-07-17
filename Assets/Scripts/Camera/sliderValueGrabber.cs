using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderValueGrabber : MonoBehaviour
{

    public Slider slider;
    public float sliderValue;
    public float finalValue;
    public GameObject cameraUpdater;
    public void UpdateSliderValue()
    {
        sliderValue = slider.value;
        finalValue = sliderValue + 0.5f;
        cameraUpdater.GetComponent<cameraSwapper>().transitionTimeUpdate(finalValue);
        
    }

}
