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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        savedMovement = Vector3.up;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ManageHealth();
        if (currentHealth != 0)
        {
            ManageMovement();
        }
    }

    private void Update()
    {
        InputHandle();
    }

    private void ManageHealth()
    {

    }

    private void InputHandle()
    {
        if (currentHealth != 0)
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
        if (gender == Gender.Female)
        {
            if (Input.GetButtonDown("SlipperFemale")){
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

    private void SpawnSlippetBullet()
    {
        GameObject go = Instantiate(slipperBulletPrefab, slipperFireGameObject.transform.position, Quaternion.identity);
        SlipperBullet sb = go.GetComponent<SlipperBullet>();
        sb.SetSlipperOwner(gender);
        sb.SetDirection(savedMovement);
    }

    public Gender GetGender()
    {
        return gender;
    }
}
