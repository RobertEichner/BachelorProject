using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Goal : ScriptableObject
{
    private KeyValuePair<string, object> goalWorldState = new KeyValuePair<string, object>();

    public KeyValuePair<string, object> GoalWorldState
    {
        get => goalWorldState;
        protected set => goalWorldState = value;
    }

    public abstract void InitGoal();
}
