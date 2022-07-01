using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacter : MonoBehaviour
{
    public Node currentNode;
    public PlayerHolder owner;

    public void OnInit()
    {
        owner.RegisterCharacter(this);
    }
}
