using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Create WorldVariable/Int")]
public class WorldInt : WorldVariables
{
    [SerializeField] private int amount = 0;
    
    public override void SetValue(object newValue)
    {
        amount = (int)newValue;
    }

    public override object GetValue()
    {
        return amount;
    }
}
