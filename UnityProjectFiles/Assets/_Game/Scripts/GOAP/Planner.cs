using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

public class Planner
{
    private List<Node> openList;
    private List<Node> closedList;

    public Queue<Action> CreatePlan(List<Action> availableActions, WorldState worldState, Goal goal)
    {

        foreach (var action in availableActions)
        {
            action.ResetAction();
        }
        
        
        SortedDictionary<string, object> goalWorldState = new SortedDictionary<string, object>();
        goalWorldState.Add(goal.GoalWorldState.Key, goal.GoalWorldState.Value);
        Node startNode = new Node(goalWorldState, 0);
        Node goalNode = new Node( worldState.CurrentWorldState,0);
        Node solutionNode = null;

        List<Action> usableActions = availableActions.Where(x => x.CanPerformAction()).ToList();
        CreateGraphFromActions(usableActions, startNode, ref goalNode, ref solutionNode);
        
        return BacktraceActions(solutionNode);
    }

    
    public void CreateGraphFromActions(List<Action> usableActions, Node parentNode, ref  Node goalNode, ref Node solutionNode)
    {
        
        foreach (var  state in usableActions)
        {
            Node newNode = new Node(new SortedDictionary<string, object>(parentNode.State));

            if (CompareStates(newNode.State, state.Effects))
            {
                newNode.ApplyNewState(state.Preconditions);
                newNode.Cost = parentNode.Cost + state.ActionCost;
                newNode.CameFrom = parentNode;
                newNode.ActionToReach = state;
            }
            else
            {
                continue;
            }
            
            if (CompareNodes(newNode, goalNode))
            {
                if (solutionNode == null || newNode.Cost < solutionNode.Cost)
                {
                    solutionNode = newNode;
                }
            }
            else
            {
                List<Action> newUsableAction = new List<Action>(usableActions);
                newUsableAction.Remove(state);
                CreateGraphFromActions(newUsableAction, newNode, ref goalNode, ref solutionNode);
            }
        }
    }

    public Queue<Action> BacktraceActions(Node solution)
    {
        if (solution == null)
            return null;

        Queue<Action> returnQueue = new Queue<Action>();

        Node iterNode = solution;
        while (iterNode.CameFrom != null && iterNode.ActionToReach != null)
        {
            returnQueue.Enqueue(iterNode.ActionToReach);
            iterNode = iterNode.CameFrom;
        }

        return new Queue<Action>(returnQueue.Reverse());
    }

    
    public bool CompareNodes(Node a, Node b)
    {
        
        return CompareStates(a.State, b.State);
    }

    public bool CompareStates(SortedDictionary<string, object> a, SortedDictionary<string, object> b)
    {
        bool isSame = true;

        foreach (var pair in a)
        {
            isSame &= b.Contains(pair);

        }
        return isSame;
    }
    

}
