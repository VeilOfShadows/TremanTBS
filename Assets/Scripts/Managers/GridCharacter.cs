using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacter : MonoBehaviour, ISelectable, IDeselect, IHighlight, IDeHighlight, IDetectable
{
    public PlayerHolder owner;

    public float moveSpeed;

    public bool isCrouched;

    [HideInInspector]
    public Node currentNode;
    [HideInInspector]
    public GameObject highlighter;
    public bool isSelected;

    public List<Node> currentPath;

    public void LoadPath(List<Node> path)
    {
        currentPath = path;
    }

    public void OnInit()
    {
        owner.RegisterCharacter(this);
        highlighter.SetActive(false);
    }

    public void OnSelect(PlayerHolder player)
    {
        highlighter.SetActive(true);
        isSelected = true;
        player.stateManager.currentCharacter = this;
    }

    public void OnDeselect(PlayerHolder player)
    {
        isSelected = false;
        highlighter.SetActive(false);
    }

    public void OnHighlight(PlayerHolder player)
    {
        
        highlighter.SetActive(true);

    }

    public void OnDeHighlight(PlayerHolder player)
    {
        if(!isSelected)
        {
            highlighter.SetActive(false);
        }
    }

    public Node OnDetect()
    {
        return currentNode;
    }
}
