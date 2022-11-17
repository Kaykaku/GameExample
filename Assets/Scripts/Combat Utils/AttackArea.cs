using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] float swordDamage = 30f;

    public void SetDamage(float swordDamage)
    {
        this.swordDamage = swordDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Character>().OnHit(swordDamage);
        }
    }
    
}
