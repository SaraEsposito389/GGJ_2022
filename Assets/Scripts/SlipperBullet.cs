using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperBullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int damage = 1;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip throwSFX;
    [SerializeField]
    private AudioClip impactSFX;

    private PlayerController2D slipperOwnerPc;

    private Gender slipperOwnerGender;

    private Vector3 direction;
    private float rotationCorrection = -90;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = throwSFX;
        audioSource.Play();
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
        
        if (other.gameObject.CompareTag("Player") && other.isTrigger)
        {

            PlayerController2D otherPc = other.GetComponent<PlayerController2D>();
            if (otherPc && otherPc.GetGender() != slipperOwnerGender && !otherPc.GetImmortalStatus()) 
            {
                otherPc.TakeDamage(damage);

                //slipperOwnerPc.Laugh();

                DestroyBullet();
            }
        } else if (other.gameObject.CompareTag("Object") && other.isTrigger)
        {
            DestroyBullet();
        }
        else if (other.gameObject.CompareTag("Wall") && other.isTrigger)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        if (slipperOwnerGender == Gender.Male)
        {
            GameEvents.Instance.DestroyMaleSlipperBullet();
        } else if (slipperOwnerGender == Gender.Female)
        {
            GameEvents.Instance.DestroyFemaleSlipperBullet();
        }

        Destroy(this.gameObject);
    }

    public void SetSlipperOwner(PlayerController2D newSlipperOwner)
    {
        slipperOwnerPc = newSlipperOwner;
        slipperOwnerGender = newSlipperOwner.GetGender();
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
}
