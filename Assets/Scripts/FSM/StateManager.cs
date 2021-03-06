using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager : MonoBehaviour
{
    public State currentState;
    public State startingState;
    public bool forceExit;

    public Node previousNode;
    public Node currentNode;
    public float delta;

    public PlayerHolder playerHolder;
    public GridCharacter currentCharacter
    {
        get { return _currentCharacter; }
        set
        {
            if(_currentCharacter != null)
            {
                _currentCharacter.OnDeselect(playerHolder);
            }

            _currentCharacter = value;

        }
    }

    GridCharacter _currentCharacter;

    protected Dictionary<string, State> allStates = new Dictionary<string, State>();

    private void Start()
    {
        Init();
    }

    public abstract void Init();

    public void Tick(SessionManager sm, Turn turn)
    {
        delta = sm.delta;

        if (currentState != null)
        {
            currentState.Tick(this, sm, turn);
        }

        forceExit = false;
    }

    public void SetState(string id)
    {
        State targetState = GetState(id);
        if (targetState == null)
        {
            Debug.Log("State with id : " + id + " cannot be found! Check your states and ids!");
        }

        currentState = targetState;
    }

    public void SetStartingState()
    {
        currentState = startingState;
    }

    State GetState(string id)
    {
        State result = null;
        allStates.TryGetValue(id, out result);
        return result;
    }
}
