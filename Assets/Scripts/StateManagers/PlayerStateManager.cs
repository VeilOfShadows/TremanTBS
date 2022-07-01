using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : StateManager
{
    public override void Init()
    {
        VariablesHolder gameVars = Resources.Load("GameVariables") as VariablesHolder;

        State interactions = new State();
        
        interactions.actions.Add(new InputManager(gameVars));
        interactions.actions.Add(new DetectMousePosition());
        interactions.actions.Add(new MoveCameraTransform(gameVars));

        State wait = new State();

        currentState = interactions;

        allStates.Add("interactions", interactions);
        allStates.Add("wait", wait);
    }
}
