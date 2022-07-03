using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();

    public void Roll()
    {
        int roll = Random.Range(0, 100);
        Debug.Log("Roll");
        if (roll <= 30)
        {
            int temp = Random.Range(0, objects.Count);
            objects[temp].gameObject.SetActive(true);
        }
    }

}
