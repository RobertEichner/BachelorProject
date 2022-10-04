using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldVariables : ScriptableObject
{
   [SerializeField] private string variableName = "";

   public string VariableName => variableName;

   public abstract void SetValue(object newValue);
   public abstract object GetValue();

}
