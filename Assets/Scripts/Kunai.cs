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

    // Set the flight speed for Kunai
    // Destroy after set time
    public void OnInit()
    {
        rb.velocity = transform.right *flySpeed;
        Invoke("OnDespawn",timeDespawn);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    // Deal damage to Enemy on impact
    // Show the effect of taking damage and disappearing after 1s
    // Destroy Kunai
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
