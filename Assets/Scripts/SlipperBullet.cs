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

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        rb.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.gameObject.CompareTag("Player") && other.isTrigger)
        {
            Destroy(this.gameObject);
            if (!other.GetComponent<Player>().IsStaggered)
            {
                other.GetComponent<Player>().TakeDamage(damage.currentValue);

            }
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }*/
    }
}
