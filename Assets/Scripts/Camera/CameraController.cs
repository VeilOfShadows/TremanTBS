using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Information")]
    [SerializeField]
    private GameObject activeCam;
    [SerializeField]
    private int index = 0;
    [SerializeField]
    private List<GameObject> cameras = new List<GameObject>();

    [Header("Coroutine Information")]
    [SerializeField]
    private bool isCoroutine = false;
    [SerializeField]
    private float swapCooldown;

    [Header("Camera Movement")]
    [SerializeField]
    private GameObject camHolder;
    [SerializeField]
    private float movementSpeed;
    
    CinemachineBrain cinemachineBrain;
    float verticalInput;
    float horizontalInput;

    public void Start()
    {
        activeCam = cameras[index];
        cinemachineBrain = this.gameObject.GetComponent<CinemachineBrain>();
        //transitionTimeUpdate(cinemachineBrain.m_DefaultBlend.m_Time);
    }

    public void transitionTimeUpdate(float tTime)
    {
        //transitionTime = this.gameObject.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time;
        cinemachineBrain.m_DefaultBlend.m_Time = tTime;
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

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        
        camHolder.transform.position += new Vector3(verticalInput * activeCam.transform.forward.x, 0, verticalInput * activeCam.transform.forward.z) * Time.deltaTime * movementSpeed;
        camHolder.transform.position += new Vector3(horizontalInput * activeCam.transform.right.x, 0, horizontalInput * activeCam.transform.right.z) * Time.deltaTime * movementSpeed;
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
            activeCam = cameras[index];
            cameras[index + 3].SetActive(false);
        }
        else if (index != 3)
        {
            index++;
            cameras[index].SetActive(true);
            activeCam = cameras[index];
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
            activeCam = cameras[index];
            cameras[index - 3].SetActive(false);
        }
        else if (index != 0)
        {
            index--;
            cameras[index].SetActive(true);
            activeCam = cameras[index];
            cameras[index + 1].SetActive(false);
        }
        yield return new WaitForSeconds(swapCooldown);
        isCoroutine = false;
        yield return null;
    }
}
