using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Phase : ScriptableObject
{
    public string phaseName;
    public bool forceExit;

    public abstract bool IsComplete(SessionManager sm, Turn turn);

    [System.NonSerialized]
    protected bool isInit;

    public virtual void OnStartPhase(SessionManager sm, Turn turn)
    {
        if (isInit)
        {
            return;
        }
        isInit = true;
    }

    public virtual void OnEndPhase(SessionManager sm, Turn turn)
    {
        isInit = false;
        forceExit = false;
    }
}
