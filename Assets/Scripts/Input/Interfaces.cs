using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    void OnSelect();
}

public interface IDeselect
{
    void OnDeselect();
}

public interface IHighlight
{
    void OnHighlight();
}

public interface IDeHighlight
{
    void OnDeHighlight();
}

public interface IDetectable
{
    Node OnDetect();
}

