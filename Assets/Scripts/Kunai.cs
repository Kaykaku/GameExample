using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject hitVFXprefab;
    [SerializeField] private float flySpeed =5f;
    [SerializeField] private float timeDespawn =4f;
    [SerializeField] private float damage =30f;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        rb.velocity = transform.right *flySpeed;
        Invoke("OnDespawn",timeDespawn);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Character>().OnHit(damage);
            Destroy(Instantiate(hitVFXprefab,transform.position,transform.rotation),1f);
            OnDespawn();
        }
    }
}
