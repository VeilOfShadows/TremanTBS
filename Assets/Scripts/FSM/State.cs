using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public List<StateActions> actions = new List<StateActions>();

    public void Tick(StateManager states, SessionManager sm, Turn t)
    {
        if (states.forceExit)
        {
            return;
        }

        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].Execute(states, sm, t);
        }
    }
}
