using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketZone : MonoBehaviour
{
    [SerializeField]
    private GameObject bucket;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onChangeVisibilityBucket += BucketVisibility;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BucketVisibility(bool visibility)
    {
        bucket.SetActive(visibility);
    }
}
