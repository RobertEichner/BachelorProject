using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Action : ScriptableObject
{
    [SerializeField] private string actionName = "";
    private SortedDictionary<string, object> preconditions = new SortedDictionary<string, object>();
    private SortedDictionary<string, object> effects = new SortedDictionary<string, object>();
    [SerializeField] private int actionCost = 0;

    public string ActionName => actionName;

    public SortedDictionary<string, object> Preconditions
    {
        get => preconditions;
        protected set => preconditions = value;
    }

    public SortedDictionary<string, object> Effects
    {
        get => effects;
        protected set => effects = value;
    }

    public int ActionCost
    {
        get => actionCost;
        set => actionCost = value;
    }

    public abstract bool CanPerformAction();
  
    public abstract void ResetAction();

    public abstract void InitAction();


}
