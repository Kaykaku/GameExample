using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private GameObject model;


    private Character target;
    public Character Target => target;
    private IState currentState;

    private bool isRight=true;

    // Update is called once per frame
    //If the character dies, the update function will stop
    //The update function will continuously execute the character's current state
    void Update()
    {
        if (base.IsDeath) return;

        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    // Based on the Character interface will initialize:
    // HP and MP of the receiver and displayed on the bar (if any)
    //Convert the object to the Idle state
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
    }

    // Based on the Character interface will :
    // Destroy itself
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);
    }

    // Based on the Character interface will :
    // Convert current state to null
    //Run dead animation
    // Destroy the character after 2 seconds
    public override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }

    //change minus current state to new state
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if(currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    //set attack object for enemy
    // if there is no target, it will stay at rest
    //If the target is within attack range, it will go through the attack state
    //If the target is out of range, it will go through patrol mode and move back to the target
    internal void SetTarget(Character character)
    {
        this.target = character;
        if (Target == null)
        {
            ChangeState(new IdleState());
        }
        else if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        } else
        {
            ChangeState(new PatrolState());
        }
    }

    //Run 'run' animation
    //Set the movement speed for the character 
    public void Moving()
    {
        ChangeAnim("Run");
        rb.velocity = transform.right * moveSpeed * (isRight ? 1 : -1);
    }


    //Run 'Idle' animation
    //Set the movement speed for the character to zero
    public void StopMoving()
    {
        if (base.IsDeath) return;
        ChangeAnim("Idle");
        rb.velocity = Vector2.zero;
    }

    //Run 'Attack' animation
    //Ưhen running the animation it will toggle the collider's trigger that receives the object in the frame corresponding to the animation
    //Open animation EAtack to see details
    public void Attack()
    {
        ChangeAnim("Attack"); 
    }

    //Return true if character in attack range
    public bool IsTargetInRange()
    {
        return Vector2.Distance(transform.position, target.transform.position) <= attackRange;
    }

    // Change the direction of the character when touching an object with the tag EnemyWall
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyWall"))
        {
            ChangeDirection(!isRight);
        }
    }

    //Set direction value corresponding to input parameter
    //Change orientation of MODEL inside CHARACTER
    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;

        model.transform.rotation = isRight ? Quaternion.Euler(Vector3.zero): Quaternion.Euler(Vector3.up *180);
    }
}
