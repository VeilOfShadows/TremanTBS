using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : StateActions
{
    VariablesHolder varHolder;

    public InputManager(VariablesHolder holder)
    {
        varHolder = holder;
    }

    public override void Execute(StateManager states, SessionManager sm, Turn t)
    {
        varHolder.horizontalInput.value = Input.GetAxis("Horizontal");
        varHolder.verticalInput.value = Input.GetAxis("Vertical");
    }
}
