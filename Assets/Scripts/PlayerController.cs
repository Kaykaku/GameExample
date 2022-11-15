using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;



    [SerializeField] private float jumpForce;

    Vector3 savePos;
    private bool isGrounded;
    private bool isJumping;
    private bool isAttack;
    private bool isDeath;
    private float horizontal;
    private int coinCount;
    private string currentAnim;
    // Start is called before the first frame update
    void Start()
    {
        savePos = transform.position;
        OnInit();
    }

    void OnInit()
    {
        isDeath = false;
        isAttack = false;

        rb.velocity = Vector2.zero;
        ChangeAnim("Idle");
        transform.position = savePos;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath) return;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                Throw();
            }

            if (Mathf.Abs(horizontal) > 0.1f)
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
            rb.velocity = new Vector2(horizontal * Time.deltaTime * speed, rb.velocity.y);
            transform.rotation= Quaternion.Euler (new Vector3(0,horizontal < 0 ? 180:0,0));
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
        ChangeAnim("Attack");
        StartCoroutine(ResetAttack());
    }
    private void Jump()
    {
        isJumping = true;
        rb.AddForce(jumpForce * Vector2.up);
        ChangeAnim("Jump");
    }

    private void Throw()
    {
        ChangeAnim("Throw");
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        rb.velocity = Vector2.zero;
        isAttack = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        isAttack = false;
        ChangeAnim("Idle");
    }

    private void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    internal void SavePoint()
    {
        savePos = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("DeathZone"))
        {
            isDeath = true;
            ChangeAnim("Dead");
            Invoke("OnInit",1f);
        }
    }
}
