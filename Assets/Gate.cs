using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private float timeBeforeClose = 2f;

    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = false;
        GameEvents.Instance.onSwitchPressed += Open;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onSwitchPressed -= Open;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            anim.SetBool("isOpen", true);
            StartCoroutine(StartTimerGateCo());
        }
    }

    private IEnumerator StartTimerGateCo()
    {
        yield return new WaitForSeconds(timeBeforeClose);
        anim.SetBool("isOpen", false);

        yield return null;
        isOpen = false;
    }
}
