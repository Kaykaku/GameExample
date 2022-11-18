using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] float swordDamage = 30f;

    //set up the damage to by a attack
    public void SetDamage(float swordDamage)
    {
        this.swordDamage = swordDamage;
    }

    //Trigger when hitting an object that is a player or an enemy, calculates the damage taken for the character
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Character>().OnHit(swordDamage);
        }
    }
    
}
