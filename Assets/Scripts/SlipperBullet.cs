using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperBullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float damage;

    private Gender slipperOwner;

    private Vector3 direction;
    private float rotationCorrection = 90;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        rb.velocity = direction.normalized * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationCorrection;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {

            PlayerController2D otherPc = other.GetComponent<PlayerController2D>();
            if (otherPc && otherPc.GetGender() != slipperOwner)
            {
                DestroyBullet();
                Debug.Log(other.name);
            }
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        if (slipperOwner == Gender.Male)
        {
            GameEvents.Instance.DestroyMaleSlipperBullet();
        } else if (slipperOwner == Gender.Female)
        {
            GameEvents.Instance.DestroyFemaleSlipperBullet();
        }

        Destroy(this.gameObject);
    }

    public void SetSlipperOwner(Gender newSlipperOwner)
    {
        slipperOwner = newSlipperOwner;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
}
