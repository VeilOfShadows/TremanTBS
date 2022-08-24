using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingObject : MonoBehaviour
{
    public List<MeshRenderer> Renderers = new List<MeshRenderer>();
    public List<Material> Materials = new List<Material>();


    private void Awake()
    {
        if(Renderers.Count == 0)
        {
            Renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        }
    }
}