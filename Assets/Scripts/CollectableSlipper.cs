using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSlipper : MonoBehaviour
{
    private bool collected;
    // Start is called before the first frame update
    void Start()
    {
        collected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collected)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                collected = true;
                GameEvents.Instance.CollectObject();
                Destroy(gameObject);
            }
        }
    }
}
