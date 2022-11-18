using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private GameObject model;
    [SerializeField] private float fireRate =1f;

    Vector3 savePos;
    private bool isGrounded;
    private bool isJumping;
    private bool isAttack;
    private float horizontal;
    private int coinCount;
    private int attackCombo;
    private float lastThrow;

    // Based on the Character interface will initialize:
    // HP and MP of the receiver and displayed on the bar (if any)
    //Convert the Character to the Idle state
    //set character velocity to 0
    //Adjust the character's position to the save point and save the current position
    //Display the number of coins available
    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;

        rb.velocity = Vector2.zero;
        ChangeAnim("Idle");
        transform.position = savePos;
        SavePoint();
        UIManager.instance.SetCoin(coinCount);
    }

    // Based on the Character interface will :
    // Destroy itself
    // Re-initialize character upon death
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    // Based on the Character interface will :
    // Convert current state to null
    // Run dead animation
    // Destroy the character after 2 seconds
    public override void OnDeath()
    {
        base.OnDeath(); 
    }

    // Update is called once per frame
    void Update()
    {
        //When the character is in a dead state, stop the Update method
        if (base.IsDeath) return;
        isGrounded = IsCheckGrounded(); //Check ground status
        horizontal = Input.GetAxis("Horizontal"); //Check navigation button event
        
        //If in attack state, the character stops moving and ends the function
        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        //If you are on the ground, allow the following action
            // If in jump state, end Update
            // If Space is pressed, do Jump
            // If LMouse is pressed, perform Attack
            // If RMouse is pressed, do Throw
            // If there is a navigation value, execute the animation Run
        if (isGrounded )
        {
            if (isJumping) return;
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            else if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
            else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                Throw();
            }

            else if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("Run");
            }
        }

        //If the Character doesn't touch the ground and has a downward velocity according to Y , run Fall animation and turn off jumping
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("Fall");
            isJumping = false;
        }

        // If there is a navigation value, set the speed for the Character to move
        // Change the direction of the character's MODEL according to the movement direction
        // If there is NO navigation value, then set the Character to stand still, run animation Idle
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            model.transform.rotation= Quaternion.Euler (new Vector3(0,horizontal < 0 ? 180:0,0));
        }else if (isGrounded)
        {
            rb.velocity = Vector2.zero;
            ChangeAnim("Idle");
        }
        

    }

    // Check if the character is on the ground by creating a Raycast from the character's position to the character's feet
    // Returns true if it detects Collider whose Layer is the ground
    private bool IsCheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.05f , Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down, 1.05f, groundLayer);
        return hit.collider != null;

    }

    // Switch to attack state
    // Run animation corresponding to the current combo attack (attack0,attack1,attack2)
    // Assign damage that deals combo attacks to 30, 35, 40 . respectively
    // Reset comboAttack = 0 when 3 combo attacks have been completed or when the time is over for the next combo attack
    // Return to Idle state after attack
    // Attack time 0.6s , Time to wait for the next combo attack is 0.75s , that means you have 0.25s to perform combo attack
    private void Attack()
    {
        isAttack = true;
        ChangeAnim("Attack"+attackCombo);
        attackArea.GetComponent<AttackArea>().SetDamage(30f + attackCombo * 5f);
        attackCombo++;
        if (attackCombo > 2) attackCombo = 0;
        else Invoke(nameof(ResetAttackCombo),0.75f);
        Invoke(nameof(ResetAttack),0.6f);
    }

    // Switch to jump state
    // Add upward jumping force for the character
    // Run jump animation
    private void Jump()
    {
        isJumping = true;
        rb.AddForce(jumpForce * Vector2.up);
        ChangeAnim("Jump");
    }

    // If cooldown has expired perform skill Throw
    // If on cooldown, now Flytext is on cooldown
    // SKILL THROW
        // Switch to attack state
        // Run animation Throw
        // Save throw time
        // Reset Idle state after throwing 0.7s
        // Check if the current mana is enough
            // If YES, throw Kunai , subtract mana consumption , set skill cooldown and display UI
            // Otherwise, Flytext will run out of mana
    private void Throw()
    {
        if (Time.time > fireRate + lastThrow)
        {
            lastThrow = Time.time;
            isAttack = true;
            ChangeAnim("Throw");
            Invoke(nameof(ResetAttack), .7f);
            if (base.OnSkill(20f))
            {
                UIManager.instance.SetColdDown1(1f);
                Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
            }
        }
        else if (!base.IsOutMana)
        {
            base.SetFlyText("ColdDown!", Color.black);
        }
        
    }

    // Turn off attack status
    // Run animation Idle
    void ResetAttack()
    {
        if (base.IsDeath) return;
        isAttack = false;
        ChangeAnim("Idle");
    }


    //Save respawn points
    internal void SavePoint()
    {
        savePos = transform.position;
    }

    //Reset combo if not in attack state
    void ResetAttackCombo()
    {
        if(!isAttack) attackCombo = 0;
    }

    //When colliding with a COIN will increase the amount of coins picked up, display the UI, and destroy the collided COIN object.
    //If it collides with DEATZONE , the character will die
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coinCount++;
            UIManager.instance.SetCoin(coinCount);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("DeathZone"))
        {
            OnDeath();
        }
    }
}
