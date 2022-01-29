using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onSwitchPressed += SwitchPressed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchPressed()
    {
        // Start switch animation
    }
}
