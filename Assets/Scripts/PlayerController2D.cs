using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{

    enum Gender
    {
        Female,
        Male
    }

    [SerializeField]
    private Gender gender = Gender.Female;

    [SerializeField]
    private float speed = 0;

    private Vector3 changeMovement;

    [SerializeField]
    private int maxHealth = 5;

    private int currentHealth;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        ManageHealth();
        InputHandle();
    }

    private void ManageHealth()
    {

    }

    private void InputHandle()
    {
        if (currentHealth != 0)
        {
            ManageMovement();
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

        }
        else
        {

        }
    }
}
