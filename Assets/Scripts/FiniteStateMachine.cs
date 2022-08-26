using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{ 
    Start,
    NewTurn,
    DetermineNextTurn,
    PlayerDecision,
    PlayerMovement,
    PlayerAction,
    EnemyDecision,
    EnemyMovement,
    EnemyAction,
    Death
}

public class FiniteStateMachine : MonoBehaviour
{
    #region Variables
    public States gameState;
    public Session sm;
    public List<GridCharacter> unitsInCombat = new List<GridCharacter>();
    public List<GridCharacter> unitsToTakeTurn = new List<GridCharacter>();
    #endregion

    #region Unity Methods
    public void Start()
    {
        gameState = States.Start;
        Debug.Log("Game state is: " + gameState);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            switch(gameState)
            {
                case States.Start:
                    break;

                case States.NewTurn:
                    break;

                case States.PlayerDecision:
                    DetermineNextTurn();

                    break;
                case States.PlayerMovement:
                    DetermineNextTurn();
                    break;

                case States.PlayerAction:
                    break;

                case States.EnemyDecision:
                    DetermineNextTurn();

                    break;
                case States.EnemyMovement:
                    DetermineNextTurn();
                    break;

                case States.EnemyAction:
                    break;

                case States.Death:
                    break;

                default:
                    break;
            }
        }
    }
    #endregion

    #region State Management
    //For resetting the game to a new turn. Everyone in combat will be upt back into the turn order
    public void NewTurn()
    {
        gameState = States.NewTurn;
        Debug.Log("Game state is: " + gameState);

        //clear the list of units and repopulate it
        unitsToTakeTurn.Clear();
        foreach(GridCharacter character in unitsInCombat)
        {
            unitsToTakeTurn.Add(character);
        }

        //sort the units and tell the next unit to take their turn
        unitsToTakeTurn.Sort(SortTurnOrder);
        unitsToTakeTurn.Reverse();
        unitsToTakeTurn[0].OnSelect();

        sm.currentCharacter = unitsToTakeTurn[0];
        sm.ClearPath();

        FindNextUnit();
    }

    //removes the unit that just finished their turn and finds out who takes the next turn
    public void DetermineNextTurn()
    {
        gameState = States.DetermineNextTurn;
        Debug.Log("Game state is: " + gameState);

        //deselect and remove the unit from the turn order
        unitsToTakeTurn[0].OnDeselect();
        unitsToTakeTurn.RemoveAt(0);

        //check if the turn is finished or not
        if(unitsToTakeTurn.Count <= 0)
        {
            NewTurn();
            return;
        }

        //sort the units and tell the next unit to take their turn
        unitsToTakeTurn.Sort(SortTurnOrder);
        unitsToTakeTurn.Reverse();
        unitsToTakeTurn[0].OnSelect();

        sm.currentCharacter = unitsToTakeTurn[0];
        sm.ClearPath();

        FindNextUnit();
    }

    //finds the next unit that is taking a turn and changes the game state accordingly
    public void FindNextUnit()
    {
        if(unitsToTakeTurn[0].isPlayer)
        {
            gameState = States.PlayerDecision;
            Debug.Log("Game state is: " + gameState);
        }
        else if(!unitsToTakeTurn[0].isPlayer)
        {
            gameState = States.EnemyDecision;
            Debug.Log("Game state is: " + gameState);
        }
    }

    //sorts the turn order by initiative stat
    private int SortTurnOrder(GridCharacter c1, GridCharacter c2)
    {
        return c1.initiative.CompareTo(c2.initiative);
    }
    #endregion
}
