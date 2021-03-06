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
    private bool isFacingUp = true;

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

    [SerializeField]
    private GameObject heldObjectBaloon;
    [SerializeField]
    private SpriteRenderer heldObject;

    [SerializeField]
    private GameObject clipPrefab;

    private bool isSlipperReady;
    private bool isImmortal;
    private bool isDead;
    private bool isFlipped;
    private bool isBucketReady;
    private bool isInSwitchArea;
    private bool isSwitchPressed;

    [SerializeField]
    private bool isClipAvailable = false;

    [SerializeField]
    private bool isClipReady = false;
    
    private GameObject bucketZone = null;
    private GameObject sheetArea = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (isFacingUp)
        {
            savedMovement = Vector3.up;
        } else
        {
            savedMovement = Vector3.down;
        }

        anim.SetFloat("verticalMovement", savedMovement.y);
        anim.SetFloat("horizontalMovement", savedMovement.x);
        anim.SetBool("isMoving", false);
        anim.ResetTrigger("damageTrigger");

        isSlipperReady = true;
        isDead = false;
        isFlipped = false;

        isBucketReady = false;
        //isClipReady = false;
        //isClipAvailable = false;
        isInSwitchArea = false;

        if (gender == Gender.Male)
        {
            GameEvents.Instance.onDestroyMaleSlipperBullet += SlipperBulletDestroyed;
        } else if (gender == Gender.Female)
        {
            GameEvents.Instance.onDestroyFemaleSlipperBullet += SlipperBulletDestroyed;
        }

        if (canAttack && isSlipperReady)
        {
            heldObjectBaloon.SetActive(true);
            heldObject.sprite = slipperBulletPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
        }

        ChangeHealth(maxHealth);
        GameEvents.Instance.onCollectGameObject += CollectObject;
        GameEvents.Instance.onCanTakeClips += ClipAvailable;
        GameEvents.Instance.onSwitchPressedEnd += SwitchOff;
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

        GameEvents.Instance.onCollectGameObject -= CollectObject;
        GameEvents.Instance.onCanTakeClips -= ClipAvailable;
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
            GameEvents.Instance.PlayerDead(gender);
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
            TakeOrLeaveBucketAndClips();
            ManageSwitch();
        }
        else if (gender == Gender.Male && Input.GetButtonDown("InteractionMale"))
        {
            TakeOrLeaveBucketAndClips();
            ManageSwitch();
        }
    }

    private void TakeOrLeaveBucketAndClips()
    {
        // Leave bucket on bucketZone
        if (isBucketReady && !isClipReady && bucketZone && !sheetArea)
        {
            // Player leaves bucket
            GameEvents.Instance.ChangeVisibilityBucket(true);
            isBucketReady = false;
            isClipAvailable = true; // you can take clip
            GameEvents.Instance.CanTakeClips();
            heldObjectBaloon.SetActive(false);
        }
        else if (!isBucketReady && isClipAvailable && bucketZone && !sheetArea)
        { // Take clip from bucket
            isClipReady = true;
            isClipAvailable = false;
            GameEvents.Instance.CollectObjectByTag("Clip", gameObject);

            if (clipPrefab)
            {
                heldObject.sprite = clipPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
                heldObjectBaloon.SetActive(true);
            }
        }
        else if (!isClipAvailable && isClipReady && sheetArea)
        {
            // leave clip on sheet
            GameEvents.Instance.TryToClipSheet(sheetArea);
            Debug.Log(gameObject.name + " mette la clip sui panni");
            
            isClipReady = false;
            isClipAvailable = true;
            sheetArea.GetComponent<BoxCollider2D>().enabled = false;
            sheetArea = null;

            heldObjectBaloon.SetActive(false);
        }
    }

    private void ManageSwitch()
    {
        if (isInSwitchArea)
        {
            isSwitchPressed = true;
            GameEvents.Instance.SwitchPressed();
            Debug.Log("Switch pressed");
        }
    }

    private void SpawnSlippetBullet()
    {
        heldObjectBaloon.SetActive(false);

        GameObject go = Instantiate(slipperBulletPrefab, slipperFireGameObject.transform.position, Quaternion.identity);
        SlipperBullet sb = go.GetComponent<SlipperBullet>();
        sb.SetSlipperOwner(this);
        sb.SetDirection(savedMovement);

        if (numThrownSlippers != null)
        {
            numThrownSlippers.SetValue(numThrownSlippers.GetValue() + 1);
        }

        GameEvents.Instance.SlipperThrown();

        isSlipperReady = false;
    }

    private void SlipperBulletDestroyed()
    {
        isSlipperReady = true;
        heldObject.sprite = slipperBulletPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
        heldObjectBaloon.SetActive(true);
    }

    public Gender GetGender()
    {
        return gender;
    }

    private void CollectObject(GameObject collectedObj, GameObject keeper)
    {
        if (!heldObjectBaloon.activeInHierarchy)
        {
            if (collectedObj.CompareTag("Bucket") && !isBucketReady && GameObject.ReferenceEquals(gameObject, keeper))
            {
                Debug.Log("Ho preso un bucket");
                isBucketReady = true;
                heldObject.sprite = collectedObj.GetComponentInChildren<SpriteRenderer>().sprite;
                heldObjectBaloon.SetActive(true);
                Destroy(collectedObj);
            }

            if (collectedObj.CompareTag("Clip") && GameObject.ReferenceEquals(gameObject, keeper))
            {
                Debug.Log(gameObject.name + " ha preso una clip");

                //TODO
                heldObject.sprite = collectedObj.GetComponentInChildren<SpriteRenderer>().sprite;
                heldObjectBaloon.SetActive(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BucketZone"))
        {
            bucketZone = other.gameObject;
        }
        else if (other.gameObject.CompareTag("Switch"))
        {
            isInSwitchArea = true;
            //Debug.Log(gameObject.name + " is in SwitchZone");
        }
        else if (other.gameObject.CompareTag("SheetPoint"))
        {
            sheetArea = other.gameObject;
            //Debug.Log(gameObject.name + " is in SheetArea");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BucketZone"))
        {
            bucketZone = null;
        }
        else if (other.gameObject.CompareTag("Switch"))
        {
            isInSwitchArea = false;
        }
        else if (other.gameObject.CompareTag("SheetPoint"))
        {
            sheetArea = null;
        }
    }

    private void ClipAvailable()
    {
        isClipAvailable = true;
    }

    private void SwitchOff()
    {
        isSwitchPressed = false;
    }
}
