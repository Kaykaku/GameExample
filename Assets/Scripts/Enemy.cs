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
    void Update()
    {
        if (base.IsDeath) return;
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }    
    }

    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);
    }

    public override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }


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
    public void Moving()
    {
        ChangeAnim("Run");
        rb.velocity = transform.right * moveSpeed * (isRight ? 1 : -1);
    }

    public void StopMoving()
    {
        ChangeAnim("Idle");
        rb.velocity = Vector2.zero;
    }

    public void Attack()
    {
        ChangeAnim("Attack"); 
        Invoke(nameof(ActiveAttack), 0.3f);
        Invoke(nameof(DeActiveAttack), 0.7f);
    }

    public bool IsTargetInRange()
    {
        return Vector2.Distance(transform.position, target.transform.position) <= attackRange;
    }

    public void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    public void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyWall"))
        {
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;

        model.transform.rotation = isRight ? Quaternion.Euler(Vector3.zero): Quaternion.Euler(Vector3.up *180);
    }
}
