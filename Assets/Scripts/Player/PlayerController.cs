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

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    public override void OnDeath()
    {
        base.OnDeath(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (base.IsDeath) return;
        isGrounded = IsCheckGrounded();
        horizontal = Input.GetAxis("Horizontal");
        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (isGrounded )
        {
            if (isJumping) return;
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            else if(Input.GetKeyDown(KeyCode.C))
            {
                Attack();
            }

            else if(Input.GetKeyDown(KeyCode.V))
            {
                Throw();
            }

            else if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("Run");
            }
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("Fall");
            isJumping = false;
        }

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

    private bool IsCheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.05f , Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down, 1.05f, groundLayer);
        return hit.collider != null;

    }

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
    private void Jump()
    {
        isJumping = true;
        rb.AddForce(jumpForce * Vector2.up);
        ChangeAnim("Jump");
    }


    private void Throw()
    {
        if (Time.time > fireRate + lastThrow)
        {
            lastThrow = Time.time;
            isAttack = true;
            ChangeAnim("Throw");
            Invoke(nameof(ResetAttack), 1f);
            if (base.OnSkill(20f))
            {
                UIManager.instance.SetColdDown1(1f);
                Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
            }
        }
        else if(base.IsOutMana)
        {
            Debug.Log(1);

            base.SetFlyText("Out of mana", Color.cyan);
        }
        else
        {
            Debug.Log(2);
            base.SetFlyText("InColdDown", Color.black);
        }
        
    }

    void ResetAttack()
    {
        isAttack = false;
        ChangeAnim("Idle");
    }

    internal void SavePoint()
    {
        savePos = transform.position;
    }

    void ResetAttackCombo()
    {
        if(!isAttack) attackCombo = 0;
    }
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
