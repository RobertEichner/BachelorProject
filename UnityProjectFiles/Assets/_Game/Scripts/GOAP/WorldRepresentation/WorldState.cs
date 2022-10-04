using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    [SerializeField] private List<WorldVariables> variablesToWatch = new List<WorldVariables>();
    
    private SortedDictionary<string, object> currentWorldState = new SortedDictionary<string, object>();
    

    public SortedDictionary<string, object> CurrentWorldState
    {
        get => currentWorldState;
        set => currentWorldState = value;
    }


    private void Awake()
    {
        foreach (var worldVariable in variablesToWatch)
        {
            currentWorldState.Add(worldVariable.VariableName, worldVariable.GetValue());
        }
    }
}
