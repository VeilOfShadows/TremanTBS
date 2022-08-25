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
            Node n = gridManager.GetNode(u.transform.position);
            if(n != null)
            {
                u.transform.position = n.worldPosition;
                n.character = u;
                u.currentNode = n;
            }

        }
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

    //public IEnumerator MoveCharacterAlongPath()
    //{
    //    //bool isInit;
    //    float t = 2;
    //    //float rotationT;
    //    float speed;
    //    Node startNode;
    //    Node targetNode;
    //    int index = 0;
    //    /*Walking along a path
    //     * 
    //    */
    //    startNode = currentCharacter.currentNode;
    //    targetNode = currentCharacter.currentPath[currentCharacter.currentPath.Count];
    //    float _t = t - 1;
    //    _t = Mathf.Clamp01(_t);
    //    t = _t;
    //    float distance = Vector3.Distance(startNode.worldPosition, targetNode.worldPosition);
    //    speed = currentCharacter.moveSpeed / distance;

    //    //    Vector3 direction = new Vector3(targetNode.worldPosition.x - startNode.worldPosition.x, 0, targetNode.worldPosition.z - startNode.worldPosition.z);
    //    //    targetRotation = Quaternion.LookRotation(direction);
    //    //    startRotation = c.transform.rotation;

    //    t += delta * speed;

    //    if(t > 1)
    //    {
    //        //    isInit = false;

    //        currentCharacter.currentNode.character = null;
    //        currentCharacter.currentNode = targetNode;
    //        currentCharacter.currentNode.character = currentCharacter;

    //        index++;

    //        if(index > currentCharacter.currentPath.Count - 1)
    //        {
    //            //we moved onto our path
    //            t = 1;
    //            index = 0;

    //            //states.SetStartingState();
    //        }
    //    }

    //    Vector3 tp = Vector3.Lerp(startNode.worldPosition, targetNode.worldPosition, t);
    //    currentCharacter.transform.position = tp;

    //    //Quaternion targetRotation;
    //    //Quaternion startRotation;

    //    //GridCharacter c = currentCharacter;
    //    ////if(!isInit)
    //    ////{
    //    //    if(c == null || index > c.currentPath.Count - 1)
    //    //    {
    //    //        states.SetStartingState();
    //    //        return;
    //    //    }

    //    //    isInit = true;
    //    //    startNode = c.currentNode;
    //    //    targetNode = c.currentPath[index];
    //    //    float _t = t - 1;
    //    //    _t = Mathf.Clamp01(_t);
    //    //    t = _t;
    //    //    float distance = Vector3.Distance(startNode.worldPosition, targetNode.worldPosition);
    //    //    speed = c.moveSpeed / distance;

    //    //    Vector3 direction = new Vector3(targetNode.worldPosition.x - startNode.worldPosition.x, 0, targetNode.worldPosition.z - startNode.worldPosition.z);
    //    //    targetRotation = Quaternion.LookRotation(direction);
    //    //    startRotation = c.transform.rotation;
    //    ////}

    //    //t += delta * speed;
    //    //rotationT += states.delta * c.moveSpeed * 2;

    //    //if(rotationT > 1)
    //    //{
    //    //    rotationT = 1;
    //    //}

    //    //c.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationT);

    //    //if(t > 1)
    //    //{
    //    //    isInit = false;

    //    //    c.currentNode.character = null;
    //    //    c.currentNode = targetNode;
    //    //    c.currentNode.character = c;

    //    //    index++;

    //    //    if(index > c.currentPath.Count - 1)
    //    //    {
    //    //        //we moved onto our path
    //    //        t = 1;
    //    //        index = 0;

    //    //        states.SetStartingState();
    //    //    }
    //    //}

    //    //Vector3 tp = Vector3.Lerp(startNode.worldPosition, targetNode.worldPosition, t);
    //    //c.transform.position = tp;
    //    yield return null;
    //}

    
}
