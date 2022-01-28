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

    private Vector3 changeMovement;
    private Vector3 savedMovement;

    [SerializeField]
    private int maxHealth = 5;

    private int currentHealth;
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject slipperBulletPrefab;
    [SerializeField]
    private GameObject slipperFireGameObject;
    [SerializeField]
    private IntValue numThrownSlippers = null;

    private bool isSlipperReady;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ChangeHealth(maxHealth);

        savedMovement = Vector3.up;
        isSlipperReady = true;
        isDead = false;

        if (gender == Gender.Male)
        {
            GameEvents.Instance.onDestroyMaleSlipperBullet += SlipperBulletDestroyed;
        } else if (gender == Gender.Female)
        {
            GameEvents.Instance.onDestroyFemaleSlipperBullet += SlipperBulletDestroyed;
        }
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
            Debug.Log(gameObject.name + ": sono morto" );
        }
    }

    public void TakeDamage(int damage)
    {
        int newHealth = currentHealth - damage;
        ChangeHealth(newHealth);
    }

    public void Heal(int healAmount)
    {
        int newHealth = currentHealth + healAmount;
        ChangeHealth(newHealth);
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
        }
    }

    public void MovePlayer()
    {
        changeMovement.Normalize();
        rb.MovePosition(transform.position + changeMovement * speed * Time.deltaTime);

        //Play Animation
    }

    private void ManageAttack()
    {
        if (isSlipperReady)
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
}
