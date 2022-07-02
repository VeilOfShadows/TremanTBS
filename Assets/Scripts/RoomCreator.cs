using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    public int x, z;
    public GameObject template;
    public Vector3 startPos;
    public GridManager gridManager;
    // Start is called before the first frame update
    void Start()
    {
        //CreateRoom();
    }

    public void CreateRoom()
    {
        Vector3 position = new Vector3();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                GameObject go = Instantiate(template,new Vector3(startPos.x + (i * 2), 0, startPos.z + (j * 2)), Quaternion.identity);
                //go.GetComponent<ObjectSpawner>().Roll();
            }
        }
        gridManager.Init();
    }
}
