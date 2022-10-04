using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Create WorldVariable/Bool")]
public class WorldBool : WorldVariables
{
    [SerializeField] private bool hasItem = false;
    
    public override void SetValue(object newValue)
    {
        hasItem = (bool)newValue;
    }

    public override object GetValue()
    {
        return hasItem;
    }
}
