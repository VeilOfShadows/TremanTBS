using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is used 

public class SessionManager : MonoBehaviour
{
    //array of turns to keep things really open. Can add or remove people from the fight with ease
    public Turn[] turns;
    int turnIndex;
    public float delta;

    public GridManager gridManager;

    bool isInit;

    private void Start()
    {
        gridManager.Init();
        InitStateManagers();
        PlaceUnits();
        isInit = true;
    }

    void InitStateManagers()
    {
        foreach (Turn t in turns)
        {
            t.player.Init();
        }
    }

    void PlaceUnits()
    {
        GridCharacter[] units = GameObject.FindObjectsOfType<GridCharacter>();
        foreach (GridCharacter u in units)
        {
            Node n = gridManager.GetNode(u.transform.position);
            if (n != null)
            {
                u.transform.position = n.worldPosition;
                n.character = u;
                u.currentNode = n;
            }
        }
    }

    private void Update()
    {
        if (!isInit)
        {
            return;
        }

        delta = Time.deltaTime;

        //turn is over
        if (turns[turnIndex].Execute(this))
        {
            turnIndex++;            
            if (turnIndex > turns.Length - 1)
            {
                turnIndex = 0;
            }
        }
    }

}
