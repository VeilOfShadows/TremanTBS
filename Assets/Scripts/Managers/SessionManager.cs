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

    public LineRenderer pathViz;


    public GridManager gridManager;

    bool isInit;

    bool isPathfinding;

    public void PathfinderCall(Node targetNode)
    {
        if (!isPathfinding)
        {
            isPathfinding = true;

            Node start = turns[0].player.characters[0].currentNode;
            Node target = targetNode;

            if (start != null && target != null)
            {
                PathfinderMaster.singleton.RequestPathFind(turns[0].player.characters[0],
                    start, target, PathfinderCallback, gridManager);
            }
            else
            {
                isPathfinding = false;
            }
        }
    }

    void PathfinderCallback(List<Node> p, GridCharacter c)
    {
        isPathfinding = false;
        if (p == null)
        {
            Debug.LogWarning("Path not valid");
            return;
        }

        pathViz.positionCount = p.Count;
        List<Vector3> allPositions = new List<Vector3>();
        for (int i = 0; i < p.Count; i++)
        {
            allPositions.Add(p[i].worldPosition + Vector3.up * .1f);
        }
        pathViz.SetPositions(allPositions.ToArray());
    }

    private void Start()
    {
        //RoomCreator roomCreator = FindObjectOfType<RoomCreator>();
        //roomCreator.CreateRoom();
        gridManager.Init();
        PlaceUnits();
        InitStateManagers();
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
            u.OnInit();
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
