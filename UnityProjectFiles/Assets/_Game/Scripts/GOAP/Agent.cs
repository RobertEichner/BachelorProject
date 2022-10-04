using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private List<Action> availableActions = new List<Action>();
    [SerializeField] private List<Goal> goals;
    
    

    public List<Action> AvailableActions => availableActions;

    public void ResetAgent()
    {
        foreach (var goal in goals)
        {

            goal.InitGoal();
        }

        foreach (var action in availableActions)
        {
            action.InitAction();
        }
    }




    public List<List<Action>> CreatePlan(WorldState worldState)
    {
        List<List<Action>> toReturn = new  List<List<Action>>();
        var planner = new Planner();
        foreach (var goal in goals)
        {
            var tmp = new List<Action>(availableActions);
            var res = planner.CreatePlan(tmp, worldState, goal);
            if(res != null)
                toReturn.Add(res.ToList());
        }

        return toReturn;
    }

}
