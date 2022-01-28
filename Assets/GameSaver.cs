using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private static GameSaver _instance;

    [SerializeField]
    private List<IntValue> intScriptableObjects = new List<IntValue>();


    public static GameSaver Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            if (_instance.gameObject.name.CompareTo(this.gameObject.name) == 0)
            {

                Destroy(this.gameObject);
            }
            else
            {
                ResetValues();
            }
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void ResetValues()
    {
        foreach (IntValue intValue in intScriptableObjects)
        {
            intValue.ResetValue();
        }
    }
}
