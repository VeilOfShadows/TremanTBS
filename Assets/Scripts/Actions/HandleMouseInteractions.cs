using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMouseInteractions : StateActions
{
    GridCharacter previousCharacter;

    public override void Execute(StateManager states, SessionManager sm, Turn t)
    {
        bool mouseClick = Input.GetMouseButtonDown(0);

        if(previousCharacter != null)
        {
            previousCharacter.OnDeHighlight(states.playerHolder);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            Node n = sm.gridManager.GetNode(hit.point);
            IDetectable detectable = hit.transform.GetComponent<IDetectable>();

            if(detectable != null)
            {
                n = detectable.OnDetect();
            }

            if (n != null)
            {                
                if(n.character != null)
                {
                    //highlighted your own unit
                    if(n.character.owner == states.playerHolder)
                    {
                        n.character.OnHighlight(states.playerHolder);
                        previousCharacter = n.character;
                        sm.ClearPath(states);
                    }
                    else //highlighted enemy unit
                    {

                    }
                }                

                if(states.currentCharacter != null && n.character == null)
                {
                    if(mouseClick)
                    {
                        if(states.currentCharacter.currentPath != null || states.currentCharacter.currentPath.Count > 0)
                        {
                            states.SetState("moveOnPath");
                        }
                    }
                    else
                    {
                        PathDetection(states, sm, n);
                    }
                }
                else // no character selected
                {
                    if(mouseClick)
                    {
                        if(n.character != null)
                        {
                            if(n.character.owner == states.playerHolder)
                            {
                                n.character.OnSelect(states.playerHolder);
                                states.previousNode = null;
                                sm.ClearPath(states);
                            }
                        }             
                    }
                }
            }
        }
    }

    void PathDetection(StateManager states, SessionManager sm, Node n)
    {
        states.currentNode = n;

        if(states.currentNode != null)
        {
            if(states.currentNode != states.previousNode || states.previousNode == null)
            {
                states.previousNode = states.currentNode;
                sm.PathfinderCall(states.currentCharacter, states.currentNode);
            }
        }
    }
}
