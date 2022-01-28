using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New IntValue", menuName = "ScriptableObjects/IntValue")]
public class IntValue : ScriptableValue<int>, ISerializationCallbackReceiver
{
   
    public void OnAfterDeserialize()
    {
        currentValue = defaultValue;
    }

    public void OnBeforeSerialize()
    {
    }

    
}