using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Phases/Idle Phase")]
public class IdlePhase : Phase
{
    public override bool IsComplete(SessionManager sm, Turn turn)
    {
        return false;
    }

    public override void OnStartPhase(SessionManager sm, Turn turn)
    {
        Debug.Log("IdlePhase started");
        base.OnStartPhase(sm, turn);

    }
}
