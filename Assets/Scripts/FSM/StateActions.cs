using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateActions
{
    public abstract void Execute(StateManager states, SessionManager sm, Turn t);
        
}
