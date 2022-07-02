using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();

    public void Roll()
    {
        int roll = Random.RandomRange(0, 100);
        Debug.Log("Roll");
        if (roll <= 30)
        {
            int temp = Random.RandomRange(0, objects.Count);
            objects[temp].gameObject.SetActive(true);
        }
    }

}
