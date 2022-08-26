using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour
{
    public List<GridCharacter> characters = new List<GridCharacter>();
    public GridManager gridManager;
    public GridCharacter currentCharacter;
    public Node currentNode;
    public Node previousNode;
    public FiniteStateMachine fsm;
    public float delta;
    


    public LineRenderer pathViz;

    bool isInit;

    bool isPathfinding;

    public int frameRate;

    private void Start()
    {
        //RoomCreator roomCreator = FindObjectOfType<RoomCreator>();
        //roomCreator.CreateRoom();
        gridManager.Init();
        PlaceUnits();
        //InitStateManagers();
        isInit = true;
        Application.targetFrameRate = frameRate;
    }

    void PlaceUnits()
    {
        GridCharacter[] units = GameObject.FindObjectsOfType<GridCharacter>();
        foreach(GridCharacter u in units)
        {
            u.OnInit();
            u.AddToCombat();
            Node n = gridManager.GetNode(u.transform.position);
            if(n != null)
            {
                u.transform.position = n.worldPosition;
                n.character = u;
                u.currentNode = n;
            }

        }
        fsm.NewTurn();
    }


    #region Pathfinding Calls
    public void PathfinderCall(GridCharacter character, Node targetNode)
    {
        if(!isPathfinding)
        {
            isPathfinding = true;

            Node start = character.currentNode;
            Node target = targetNode;

            if(start != null && target != null)
            {
                PathfinderMaster.singleton.RequestPathFind(character,
                    start, target, PathfinderCallback, gridManager);
            }
            else
            {
                isPathfinding = false;
            }
        }
    }

    public void ClearPath()
    {
        pathViz.positionCount = 0;
        if(currentCharacter != null)
        {
            currentCharacter.currentPath = null;
        }
    }

    void PathfinderCallback(List<Node> p, GridCharacter c)
    {
        isPathfinding = false;
        if(p == null)
        {
            //Debug.LogWarning("Path not valid");
            return;
        }

        pathViz.positionCount = p.Count + 1;
        List<Vector3> allPositions = new List<Vector3>();
        Vector3 offset = Vector3.up * .1f;

        allPositions.Add(c.currentNode.worldPosition + offset);
        for(int i = 0; i < p.Count; i++)
        {
            allPositions.Add(p[i].worldPosition + Vector3.up * .1f);
        }

        c.LoadPath(p);

        pathViz.SetPositions(allPositions.ToArray());
    }
    #endregion

    public IEnumerator MoveCharacterAlongPath()
    {
        currentCharacter.currentNode.character = null;
        currentCharacter.transform.position = currentCharacter.currentPath[currentCharacter.currentPath.Count-1].worldPosition;
        currentCharacter.currentNode = currentCharacter.currentPath[currentCharacter.currentPath.Count-1];
        currentCharacter.currentNode.character = currentCharacter;
        ClearPath();

        if(currentCharacter.isPlayer)
        {
            fsm.gameState = States.PlayerDecision;
            Debug.Log("Game state is: " + fsm.gameState);
        }
        else if(!currentCharacter.isPlayer)
        {
            fsm.gameState = States.EnemyDecision;
            Debug.Log("Game state is: " + fsm.gameState);
        }

        yield return null;
    }
    //    int index = 0;
    //    bool init;
    //    float t = 1;
    //    Node startNode;
    //    Node targetNode;
    //    Node nextNode;
    //    float speed;

    //    GridCharacter c = currentCharacter;
    //    bool isMoving = false;
    //    //set spee value
    //    //t = Time.deltaTime * currentCharacter.moveSpeed;
    //    targetNode = c.currentPath[c.currentPath.Count - 1];

    //    //while(targetNode.character != c)
    //    //{
    //    for(int i = 0; i < c.currentPath.Count; i++)
    //    {
    //        if(!isMoving)
    //        {
    //            isMoving = true;
    //            Debug.Log("MOVING");
    //            if(targetNode.character == c)
    //            {
    //                yield break;
    //            }

    //            //no character - break
    //            if(c == null || index > c.currentPath.Count - 1)
    //            {
    //                yield break;
    //            }

    //            //set the start and target nodes
    //            startNode = c.currentNode;
    //            nextNode = c.currentPath[index];

    //            float _t = t - 1;
    //            _t = Mathf.Clamp01(_t);
    //            t = _t;
    //            float distance = Vector3.Distance(startNode.worldPosition, targetNode.worldPosition);
    //            speed = c.moveSpeed / distance;

    //            t += delta * speed;

    //            //set the character to be on the next node
    //            c.currentNode.character = null;
    //            c.currentNode = nextNode;
    //            c.currentNode.character = c;

    //            index++;

    //            if(index > c.currentPath.Count - 1)
    //            {
    //                //we have reached the path
    //                //t = 1;
    //                index = 0;
    //            }


    //            //Vector3 tp = Vector3.Lerp(startNode.worldPosition, nextNode.worldPosition, currentCharacter.moveSpeed);
    //            c.transform.position = Vector3.Lerp(c.currentNode.worldPosition, nextNode.worldPosition, 0.1f);
    //            yield return new WaitForSeconds(1f);
    //            isMoving = false;
    //            yield return null;
    //        }
    //    }    
    //}
}
