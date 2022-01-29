using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Female,
    Male
}

public class PlayerController2D : MonoBehaviour
{
    [SerializeField]
    private Gender gender = Gender.Female;

    [SerializeField]
    private float speed = 8;
    [SerializeField]
    private bool canAttack = false;

    private Vector3 changeMovement;
    private Vector3 savedMovement;

    [SerializeField]
    private int maxHealth = 5;
    
    private int currentHealth;
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField]
    private float immortalityWindow = 0.75f;

    [SerializeField]
    private GameObject slipperBulletPrefab;
    [SerializeField]
    private GameObject slipperFireGameObject;
    [SerializeField]
    private IntValue numThrownSlippers = null;
    [SerializeField]
    private SpriteRenderer sr;

    private bool isSlipperReady;
    private bool isImmortal;
    private bool isDead;
    private bool isFlipped;
    private bool isBucketReady;
    private bool isClipReady;
    private GameObject bucketZone;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        savedMovement = Vector3.up;
        anim.SetFloat("verticalMovement", savedMovement.y);
        anim.SetFloat("horizontalMovement", savedMovement.x);
        anim.SetBool("isMoving", false);
        anim.ResetTrigger("damageTrigger");

        isSlipperReady = true;
        isDead = false;
        isFlipped = false;

        isBucketReady = false;
        isClipReady = false;

        if (gender == Gender.Male)
        {
            GameEvents.Instance.onDestroyMaleSlipperBullet += SlipperBulletDestroyed;
        } else if (gender == Gender.Female)
        {
            GameEvents.Instance.onDestroyFemaleSlipperBullet += SlipperBulletDestroyed;
        }

        ChangeHealth(maxHealth);
        GameEvents.Instance.onCollectObjectByTag += CollectObejct;
    }

    private void OnDestroy()
    {
        if (gender == Gender.Male)
        {
            GameEvents.Instance.onDestroyMaleSlipperBullet -= SlipperBulletDestroyed;
        }
        else if (gender == Gender.Female)
        {
            GameEvents.Instance.onDestroyFemaleSlipperBullet -= SlipperBulletDestroyed;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            ManageMovement();
        }
    }

    private void Update()
    {
        ManageHealth();
        InputHandle();
    }

    private void ManageHealth()
    {
        if (currentHealth == 0 && !isDead)
        {
            isDead = true;
            anim.SetBool("isDead", true);
        }
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("damageTrigger");
        StartCoroutine(ImmortalityWindowCo());
        int newHealth = currentHealth - damage;
        ChangeHealth(newHealth);
    }

    public void Heal(int healAmount)
    {
        int newHealth = currentHealth + healAmount;
        ChangeHealth(newHealth);
    }

    private IEnumerator ImmortalityWindowCo()
    {
        isImmortal = true;
        yield return new WaitForSeconds(immortalityWindow);
        isImmortal = false;
    }

    public bool GetImmortalStatus()
    {
        return isImmortal;
    }

    private void ChangeHealth(int newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        currentHealth = newHealth;

        if (gender == Gender.Male)
        {
            GameEvents.Instance.ChangeMaleHealth(currentHealth);
        }
        else if (gender == Gender.Female)
        {
            GameEvents.Instance.ChangeFemaleHealth(currentHealth);
        }

    }

    private void InputHandle()
    {
        if (!isDead)
        {
            ManageAttack();
            ManageInteraction();
        }
    }

    private void ManageMovement()
    {
        GetMovement();
        MovePlayer();
    }

    private void GetMovement()
    {
        changeMovement = Vector3.zero;

        if (gender == Gender.Female)
        {
            changeMovement.x = Input.GetAxisRaw("HorizontalFemale");
            changeMovement.y = Input.GetAxisRaw("VerticalFemale");
        }
        else if (gender == Gender.Male)
        {
            changeMovement.x = Input.GetAxisRaw("HorizontalMale");
            changeMovement.y = Input.GetAxisRaw("VerticalMale");
        }

        if (changeMovement != Vector3.zero)
        {
            savedMovement = changeMovement.normalized;
            anim.SetFloat("verticalMovement", savedMovement.y);
            anim.SetFloat("horizontalMovement", savedMovement.x);
            anim.SetBool("isMoving", true);

            if (changeMovement.x > 0f && !isFlipped)
            {
                Flip();
            } else if (changeMovement.x < 0f && isFlipped)
            {
                Flip();
            }
        } else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void Flip()
    {
        isFlipped = !isFlipped;
        sr.flipX = isFlipped;
    }

    public void MovePlayer()
    {
        changeMovement.Normalize();
        rb.MovePosition(transform.position + changeMovement * speed * Time.deltaTime);
    }

    private void ManageAttack()
    {
        if (isSlipperReady && canAttack)
        {
            if (gender == Gender.Female)
            {
                if (Input.GetButtonDown("SlipperFemale"))
                {
                    SpawnSlippetBullet();
                    Debug.Log("Gertrude attacks with direction " + savedMovement);

                    
                }
            }
            else
            {
                if (Input.GetButtonDown("SlipperMale"))
                {
                    SpawnSlippetBullet();
                    Debug.Log("Ignazio attacks with direction " + savedMovement);
                }
            }
        }
    }

    private void ManageInteraction()
    {
        if (isBucketReady && !isClipReady && bucketZone)
        {
            if (Input.GetButtonDown("InteractionFemale") || Input.GetButtonDown("InteractionMale"))
            {
                // Player leaves bucket
                GameEvents.Instance.ChangeVisibilityBucket(true);
                isBucketReady = false;
            }
        }

        if (isClipReady)
        {
            //if nel collider del panno

            if (Input.GetButtonDown("InteractionFemale"))
            {
                Debug.Log("Gertrude mette la clip sui panni");
                // TODO
                isClipReady = false;
            }

            if (Input.GetButtonDown("InteractionMale"))
            {
                Debug.Log("Ignazio mette la clip sui panni");
                // TODO
                isClipReady = false;
            }
        }
    }

    private void SpawnSlippetBullet()
    {
        GameObject go = Instantiate(slipperBulletPrefab, slipperFireGameObject.transform.position, Quaternion.identity);
        SlipperBullet sb = go.GetComponent<SlipperBullet>();
        sb.SetSlipperOwner(gender);
        sb.SetDirection(savedMovement);

        if (numThrownSlippers != null)
        {
            numThrownSlippers.SetValue(numThrownSlippers.GetValue() + 1);
        }

        isSlipperReady = false;
    }

    private void SlipperBulletDestroyed()
    {
        isSlipperReady = true;
    }

    public Gender GetGender()
    {
        return gender;
    }

    private void CollectObejct(string tag, GameObject keeper)
    {
        if (tag == "Bucket" && !isBucketReady && GameObject.ReferenceEquals(gameObject, keeper))
        {
            Debug.Log("Ho preso un bucket");
            isBucketReady = true;
            // Spawn bucket in capa
        }

        if (tag == "Clip")
        {
            Debug.Log("Ho preso una clip");
            isClipReady = true;
            // Spawn clip in capa
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BucketZone"))
        {
            bucketZone = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BucketZone"))
        {
            bucketZone = null;
        }
    }
}
