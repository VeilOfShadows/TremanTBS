using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Material fadeMaterial;
    public Material defaultMaterial;
    public LayerMask mask;
    public float transitionSpeed;

    //public bool coroutineStarted = false;

    public List<FadingObject> fadedObjects = new List<FadingObject>();

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000, mask))
        {
            if(hit.transform.GetComponent<FadingObject>())
            {
                FadingObject obj = hit.transform.GetComponent<FadingObject>();
                if(!fadedObjects.Contains(obj))
                {
                    fadedObjects.Add(obj);
                }

                for(int i = 0; i < obj.Renderers.Count; i++)
                {
                    FadeOut(obj.Renderers[i]);
                }
            }
            //FadeOut(hit.transform.GetComponent<MeshRenderer>());
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            for(int i = 0; i < fadedObjects.Count; i++)
            {
                FadeIn(fadedObjects[i]);
            }
        }
    }
    
    public void FadeIn(FadingObject obj)
    {
        foreach(MeshRenderer item in obj.Renderers)
        {
            item.material = defaultMaterial;
        }

    }

    public void FadeOut(MeshRenderer renderer)
    {
        renderer.material = fadeMaterial;
        //renderer.material.color = new Color(191,191,191,Mathf.Lerp(255, 100, transitionSpeed)* Time.deltaTime);
        //StartCoroutine(CheckFade());
    }

    //public void FadeIn(FadingObject obj)
    //{
    //    fadedObjects.Remove(obj);
    //    for(int i = 0; i < obj.Renderers.Count; i++)
    //    {
    //        obj.Renderers[i].material = defaultMaterial;
    //    }
    //}

    //private IEnumerator CheckFade()
    //{
    //    if(coroutineStarted)
    //    {
    //        yield return null;
    //    }
    //    coroutineStarted = true;

    //    yield return new WaitForSeconds(2f);

    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    for(int i = 0; i < fadedObjects.Count; i++)
    //    {
    //        if(Physics.Raycast(ray, out hit, 1000, mask))
    //        {
    //            if(hit.transform == fadedObjects[i])
    //            {
    //                //object is still being hovered, ignore it
    //                yield return new WaitForSeconds(2f);
    //                Debug.Log("object still hit");
    //                yield return null;
    //            }
    //            else
    //            {
    //                Debug.Log("object not hit");
    //                FadeIn(hit.transform.GetComponent<FadingObject>());
    //                coroutineStarted = false;
    //                yield return null;
    //            }
    //        }
    //    }        
    //}
}
