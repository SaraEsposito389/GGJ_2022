using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSlipper : MonoBehaviour
{
    private bool collected;
    [SerializeField]
    private AudioClip collectSFX;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        collected = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collected)
        {
            if (other.gameObject.CompareTag("Player") && !other.isTrigger)
            {
                collected = true;
                audioSource.PlayOneShot(collectSFX);
                GameEvents.Instance.CollectObject();

                gameObject.GetComponent<SpriteRenderer>().enabled = false;

                Destroy(gameObject, 1.5f);
            }
        }
    }
}
