using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public Enemy enemy;

    //Set a target for the character when the Player is in sight
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.SetTarget(collision.GetComponent<Character>());
        }
    }

    //Set a target for the character when the Player is out of sight
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.SetTarget(null);
        }
    }
}
