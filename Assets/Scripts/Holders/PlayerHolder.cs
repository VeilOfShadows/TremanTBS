using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game/Player Holder")]
public class PlayerHolder : ScriptableObject
{
    [System.NonSerialized]
    public StateManager stateManager;
    [System.NonSerialized]
    GameObject stateManagerObject;

    public GameObject stateManagerPrefab;
    public List<GridCharacter> characters = new List<GridCharacter>();

    public void Init()
    {
        stateManagerObject = Instantiate(stateManagerPrefab);
        stateManager = stateManagerObject.GetComponent<StateManager>();
    }

    public void RegisterCharacter(GridCharacter c)
    {
        if (!characters.Contains(c))
        {
            characters.Add(c);
        }
    }

    public void UnRegisterCharacter(GridCharacter c)
    {
        if (characters.Contains(c))
        {
            characters.Remove(c);
        }
    }
}
