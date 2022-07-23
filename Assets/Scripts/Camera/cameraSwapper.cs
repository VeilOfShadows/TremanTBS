using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwapper : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> cameras = new List<GameObject>();
    [SerializeField]
    private int index = 0;
    public bool isCoroutine = false;
    public float swapCooldown;

    public void Start()
    {
        transitionTimeUpdate(this.gameObject.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);
    }

    public void transitionTimeUpdate(float tTime)
    {
        //transitionTime = this.gameObject.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time;
        this.gameObject.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = tTime;
        swapCooldown = tTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isCoroutine)
            {
                StartCoroutine(rotateCamClockwise());
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isCoroutine)
            {
                StartCoroutine(rotateCamCounterClockwise());
            }
        }
    }

    //returns current cam index number;
    public int IndexCount()
    {
        return index;
    }


    //////////////Coroutines/////////////////////////////
    public IEnumerator rotateCamClockwise()
    {
        if (isCoroutine)
        {
            yield return null;
        }
        isCoroutine = true;
        if (index == 3)
        {
            index = 0;
            cameras[index].SetActive(true);
            cameras[index + 3].SetActive(false);
        }
        else if (index != 3)
        {
            index++;
            cameras[index].SetActive(true);
            cameras[index - 1].SetActive(false);
        }
        yield return new WaitForSeconds(swapCooldown);
        isCoroutine = false;
        yield return null;
    }

    public IEnumerator rotateCamCounterClockwise()
    {
        if (isCoroutine)
        {
            yield return null;
        }
        isCoroutine = true;
        if (index == 0)
        {
            index = 3;
            cameras[index].SetActive(true);
            cameras[index - 3].SetActive(false);
        }
        else if (index != 0)
        {
            index--;
            cameras[index].SetActive(true);
            cameras[index + 1].SetActive(false);
        }
        yield return new WaitForSeconds(swapCooldown);
        isCoroutine = false;
        yield return null;
    }
}
