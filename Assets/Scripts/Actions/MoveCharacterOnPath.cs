using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterOnPath : StateActions
{
    bool isInit;
    float t;
    float rotationT;
    float speed;
    Node startNode;
    Node targetNode;
    Quaternion targetRotation;
    Quaternion startRotation;
    int index;

    public override void Execute(StateManager states, SessionManager sm, Turn turn)
    {
        GridCharacter c = states.currentCharacter;
        if(!isInit)
        {
            if(c == null || index > c.currentPath.Count - 1)
            {
                states.SetStartingState();
                return;
            }

            isInit = true;
            startNode = c.currentNode;
            targetNode = c.currentPath[index];
            float _t = t-1;
            _t = Mathf.Clamp01(_t);
            t = _t;
            float distance = Vector3.Distance(startNode.worldPosition, targetNode.worldPosition);
            speed = c.moveSpeed / distance;

            Vector3 direction = new Vector3(targetNode.worldPosition.x - startNode.worldPosition.x, 0, targetNode.worldPosition.z - startNode.worldPosition.z);
            targetRotation = Quaternion.LookRotation(direction);
            startRotation = c.transform.rotation;
        }

        t += states.delta * speed;
        rotationT += states.delta * c.moveSpeed * 2;

        if(rotationT > 1)
        {
            rotationT = 1;
        }

        c.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationT);

        if(t > 1)
        {
            isInit = false;

            c.currentNode.character = null;
            c.currentNode = targetNode;
            c.currentNode.character = c;

            index++;

            if(index > c.currentPath.Count - 1)
            {
                //we moved onto our path
                t = 1;
                index = 0;

                states.SetStartingState();
            }
        }

        Vector3 tp = Vector3.Lerp(startNode.worldPosition, targetNode.worldPosition, t);
        c.transform.position = tp;
    }
}
