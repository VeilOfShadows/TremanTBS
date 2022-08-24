using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float duration;
    public MeshRenderer renderer;
    public bool fade;

    public void OnMouseEnter()
    {
        fade = true;
    }

    private void Update()
    {
        if(fade)
        {
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            renderer.material.color = Color.Lerp(new Color(0.5f, 0.5f, 0.5f, 1f), new Color(0.5f, 0.5f, 0.5f, 0.01f), lerp);
        }
    }

}
