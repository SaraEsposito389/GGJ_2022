using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSlipper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.isTrigger)
        {
            // Generate event to increase score
            Destroy(gameObject);
        }
            
    }
}
