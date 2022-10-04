using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Goal")]
public class SimpleGoal : Goal
{
    [SerializeField] private WorldBool goalVar;

    [SerializeField] private bool goalVal;


    public override void InitGoal()
    {
        if(goalVar == null)
            return;
        GoalWorldState = new KeyValuePair<string, object>(goalVar.VariableName, goalVal);   
    }
}
