using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMouseInteractions : MonoBehaviour /*: StateActions*/
{
    GridCharacter previousCharacter;
    public Session sm;

    public void Update()
    {
        if(previousCharacter != null)
        {
            previousCharacter.OnDeHighlight();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            Node n = sm.gridManager.GetNode(hit.point);
            IDetectable detectable = hit.transform.GetComponent<IDetectable>();
            if(detectable != null)
            {
                n = detectable.OnDetect();
            }

            if(n != null)
            {
                if(n.character != null)
                {
                    //highlighted your own unit
                    if(n.character.isPlayer == true)
                    {
                        n.character.OnHighlight();
                        previousCharacter = n.character;
                        sm.ClearPath();
                        if(Input.GetMouseButtonDown(0))
                        {
                            sm.currentCharacter = n.character;
                            n.character.OnSelect();
                        }
                    }
                    //else //highlighted enemy unit
                    //{

                    //}
                }

                //if the session has a character and the node does not contain a character
                if(sm.currentCharacter != null && n.character == null)
                {
                    //click to select a node to start walking on the path
                    if(Input.GetMouseButtonDown(0))
                    {
                        if(sm.currentCharacter.currentPath != null || sm.currentCharacter.currentPath.Count > 0)
                        {
                            //sm.StartCoroutine(sm.MoveCharacterAlongPath());
                            Debug.Log("ATTEMPTING MOVE");
                        }
                    }
                    else
                    {
                        PathDetection(n);
                    }
                }
                //else // no character selected
                //{
                //    if(Input.GetMouseButtonDown(0))
                //    {
                //        if(n.character != null)
                //        {
                //            if(n.character == sm.currentCharacter)
                //            {
                //                n.character.OnSelect();
                //                //states.previousNode = null;
                //                //sm.ClearPath(states);
                //            }
                //        }
                //    }
                //}
            }
        }
    }

    //public void Update()
    //{
    //    //if(Input.GetMouseButtonDown(0))
    //    //{
    //        //if you have a previous character, unhighlight him
    //        if(previousCharacter != null)
    //        {
    //            previousCharacter.OnDeHighlight(previousCharacter);
    //        }

    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if(Physics.Raycast(ray, out hit, 1000))
    //        {
    //            Node n = sm.gridManager.GetNode(hit.point);
    //            IDetectable detectable = hit.transform.GetComponent<IDetectable>();

    //            if(detectable != null)
    //            {
    //                n = detectable.OnDetect();
    //            }

    //            if(n != null)
    //            {
    //                if(n.character != null)
    //                {
    //                    //highlighted your own unit
    //                    //if(n.character.owner == states.playerHolder)
    //                    //{
    //                    //    n.character.OnHighlight(states.playerHolder);
    //                    //    previousCharacter = n.character;
    //                    //    sm.ClearPath(states);
    //                    //}
    //                    //else //highlighted enemy unit
    //                    //{

    //                    //}
    //                }

    //                if(sm.currentCharacter != null && n.character == null)
    //                {
    //                    if(Input.GetMouseButtonDown(0))
    //                    {
    //                        if(states.currentCharacter.currentPath != null || states.currentCharacter.currentPath.Count > 0)
    //                        {
    //                            //states.SetState("moveOnPath");
    //                        }
    //                    }
    //                    else
    //                    {
    //                        PathDetection(states, sm, n);
    //                    }
    //                }
    //                else // no character selected
    //                {
    //                    if(Input.GetMouseButtonDown(0))
    //                    {
    //                        if(n.character != null)
    //                        {
    //                            if(n.character.owner == states.playerHolder)
    //                            {
    //                                n.character.OnSelect(states.playerHolder);
    //                                //states.previousNode = null;
    //                                //sm.ClearPath(states);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        //}
    //    }
    //}

    void PathDetection(Node n)
    {
        sm.currentNode = n;

        if(sm.currentNode != null)
        {
            if(sm.currentNode != sm.previousNode || sm.previousNode == null)
            {
                sm.previousNode = sm.currentNode;
                sm.PathfinderCall(sm.currentCharacter, sm.currentNode);
            }
        }
    }
}
