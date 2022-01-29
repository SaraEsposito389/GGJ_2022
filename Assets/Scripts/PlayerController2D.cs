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
    [SerializeField]
    private AudioClip laughClip;
    [SerializeField]
    private AudioClip damageClip;

    private Vector3 changeMovement;
    private Vector3 savedMovement;

    [SerializeField]
    private int maxHealth = 5;
    
    private int currentHealth;
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;

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
    private bool isClipAvailable;
    private GameObject bucketZone;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

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
        isClipAvailable = false;

        if (gender == Gender.Male)
        {
            GameEvents.Instance.onDestroyMaleSlipperBullet += SlipperBulletDestroyed;
        } else if (gender == Gender.Female)
        {
            GameEvents.Instance.onDestroyFemaleSlipperBullet += SlipperBulletDestroyed;
        }

        ChangeHealth(maxHealth);
        GameEvents.Instance.onCollectObjectByTag += CollectObejct;
        GameEvents.Instance.onCanTakeClips += ClipAvailable;
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
        audioSource.PlayOneShot(damageClip);
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

    public void Laugh()
    {
        audioSource.PlayOneShot(laughClip);
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
        if (gender == Gender.Female && Input.GetButtonDown("InteractionFemale"))
        {
            takeOrLeaveBucketAndClips();
        }
        else if (gender == Gender.Male && Input.GetButtonDown("InteractionMale"))
        {
            takeOrLeaveBucketAndClips();
        }
    }

    private void takeOrLeaveBucketAndClips()
    {
        // Leave bucket on bucketZone
        if (isBucketReady && !isClipReady && bucketZone)
        {
            // Player leaves bucket
            GameEvents.Instance.ChangeVisibilityBucket(true);
            isBucketReady = false;
            //isClipAvailable = true;
            GameEvents.Instance.CanTakeClips();
        }
        else if (!isBucketReady && isClipAvailable && bucketZone)
        { // Take clip from bucket
            isClipReady = true;
            isClipAvailable = false;
            GameEvents.Instance.CollectObjectByTag("Clip", gameObject);
        }
        else if (!isClipAvailable && isClipReady && !bucketZone)
        {
            //if nel collider del panno
            Debug.Log(gameObject.name + " mette la clip sui panni");
            // TODO
            isClipReady = false;
            isClipAvailable = true;
        }
    }

    private void SpawnSlippetBullet()
    {
        GameObject go = Instantiate(slipperBulletPrefab, slipperFireGameObject.transform.position, Quaternion.identity);
        SlipperBullet sb = go.GetComponent<SlipperBullet>();
        sb.SetSlipperOwner(this);
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

        if (tag == "Clip" && GameObject.ReferenceEquals(gameObject, keeper))
        {
            Debug.Log(gameObject.name + " ha preso una clip");
            
            //TODO
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

    private void ClipAvailable()
    {
        isClipAvailable = true;
    }
}
