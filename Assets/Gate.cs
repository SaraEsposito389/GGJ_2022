using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onSwitchPressed += Open;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Open()
    {
        // Start gate animation
    }
}
