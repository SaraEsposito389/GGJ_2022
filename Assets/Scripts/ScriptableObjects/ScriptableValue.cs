using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableValue<T> : ScriptableObject
{
    public T currentValue;
    public T defaultValue;

    public void SetValue(T newValue)
    {
        currentValue = newValue;
    }

    public T GetValue()
    {
        return currentValue;
    }

    public void ResetValue()
    {
        currentValue = defaultValue;
    }

    public System.Type GetTypeT()
    {
        return typeof(T);
    }
}
