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

    private bool isGrounded;
    private bool isJumping;
    private bool isAttack;
    private float horizontal;
    private string currentAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isAttack);
        if (isAttack) return;

        isGrounded = IsCheckGrounded();
        horizontal = Input.GetAxis("Horizontal");


        
        if (isGrounded )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Throw();
            }

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("Run");
            }
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
        
        if(!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("Fall");
            isJumping = false;
        }
    }

    private bool IsCheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f , Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down, 1.2f, groundLayer);
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
        yield return new WaitForSeconds(1f);
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
            anim.
        }
    }
}