using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
   private Node cameFrom;
   private int cost;
   private SortedDictionary<string, object> state;
   private Action actionToReach;

   public Node CameFrom
   {
      get => cameFrom;
      set => cameFrom = value;
   }


   public SortedDictionary<string, object> State
   {
      get => state;
      set => state = value;
   }

   public int Cost
   {
      get => cost;
      set => cost = value;
   }

   public Action ActionToReach
   {
      get => actionToReach;
      set => actionToReach = value;
   }

   public Node(SortedDictionary<string, object> state)
   {
      this.state = state;
   }

   public Node(SortedDictionary<string, object> state, int cost)
   {
      this.state = state;
      this.cost = cost;
   }

   public void ApplyNewState(SortedDictionary<string, object>  actionToApply)
   {
      /*foreach (var stateChange in actionToApply)
      {
         state[stateChange.Key] = stateChange.Value;
      }*/
      state = new SortedDictionary<string, object>(actionToApply);
   }
}
