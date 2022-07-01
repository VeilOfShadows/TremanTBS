using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraTransform : StateActions
{
    TransformVariable cameraTransform;

    FloatVariable horizontal;
    FloatVariable vertical;

    VariablesHolder varHolder;

    public MoveCameraTransform(VariablesHolder holder)
    {
        varHolder = holder;

        cameraTransform = varHolder.cameraTransform;
        horizontal = varHolder.horizontalInput;
        vertical = varHolder.verticalInput;
    }

    public override void Execute(StateManager states, SessionManager sm, Turn t)
    {
        Vector3 tp = cameraTransform.value.forward * (vertical.value * varHolder.cameraMoveSpeed * states.delta);
        tp += cameraTransform.value.right * (horizontal.value * varHolder.cameraMoveSpeed * states.delta);

        cameraTransform.value.position += tp;
    }
}
