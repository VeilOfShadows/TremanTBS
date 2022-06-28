using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool walkable;
    //public bool floorSpace;
    public Vector3 worldPosition;

    public Node(bool _walkable, /*bool _floorSpace,*/ Vector3 _worldPos)
    {
        walkable = _walkable;
        //floorSpace = _floorSpace;
        worldPosition = _worldPos;
    }
}
