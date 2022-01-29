using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField]
    private int numClips = 5;

    private int currNumOfClips = 0;

    [SerializeField]
    private int secBeforeDestroy = 0;

    [SerializeField]
    private bool isBucketToCollect = true;

    // Start is called before the first frame update
    void Start()
    {
        currNumOfClips = numClips;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateCurrNumOfClips()
    {
        if (currNumOfClips != 0)
        {
            currNumOfClips--;
        }
        else
        {
            Destroy(gameObject, secBeforeDestroy);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isBucketToCollect && other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.name + " takes " + this.tag);
            GameEvents.Instance.CollectObjectByTag(this.tag, other.gameObject);
            Destroy(gameObject, secBeforeDestroy);
        }
        else if (!isBucketToCollect && other.gameObject.CompareTag("Player"))
        {
            // Get clip
            Debug.Log("Bucket dà clip");
            UpdateCurrNumOfClips();
        }
    }
}
