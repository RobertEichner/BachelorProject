using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Create Action")]
public class SimpleAction : Action
{
    [SerializeField] private List<WorldBoolContainer> pre;
    [SerializeField] private List<WorldBoolContainer> eff;


    public override void InitAction()
    {
        Preconditions.Clear();
        Effects.Clear();

        foreach (var container in pre)
        {
            Preconditions.Add(container.variable.VariableName, container.desiredValue);
        }
        
        foreach (var container in eff)
        {
            Effects.Add(container.variable.VariableName, container.desiredValue);
        }
   
    }

    public override bool CanPerformAction()
    {
        return true;
    }

    public override void ResetAction()
    {
        
    }
}
