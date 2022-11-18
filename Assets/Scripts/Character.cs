using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Bar healthBar;
    [SerializeField] protected Bar manaBar;
    [SerializeField] protected CombatTextV2 combatTextPrefab;

    [SerializeField] private float hp;
    [SerializeField] private float mp;
    [SerializeField] private float maxHp;
    [SerializeField] private float maxMp;
    private string currentAnim;

    public bool IsDeath => hp <= 0;
    public bool IsOutMana => mp <= 0;

    private void Start()
    {
        OnInit();
    }

    // Initialize the character's HP and Mp and display it on the UI
    public virtual void OnInit()
    {
        hp = maxHp;
        healthBar.OnInit(maxHp);
        mp = maxMp;
        if(manaBar != null) manaBar.OnInit(maxMp);
    }

    public virtual void OnDespawn()
    {
        
    }

    // Run Dead animation and destroy object after 2s
    public virtual void OnDeath()
    {
        ChangeAnim("Dead");
        Invoke(nameof(OnDespawn),2f);
    }

    //Change from current animation to new animation
    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    // If the character is not dead, damage will be calculated
    // Switch to dead state if HP <= 0
    // Show the UI HP value after damage calculation
    // Display Flytext with the amount of damage taken
    public virtual void OnHit(float damage)
    {
        if (!IsDeath)
        {
            hp -= damage;
            if (hp <= 0)
            {
                hp = 0;
                OnDeath();
            }
            healthBar.SetNewPoint(hp);
            Instantiate(combatTextPrefab,transform.position+Vector3.up,Quaternion.identity).OnInit(damage.ToString(),Color.red);
        }
    }

    // Calculating the permission to use skill
    // If the current Mp is lower than Mp to use the skill, Flytext does not have enough Mp, returns FALSE
    // If the character is still alive and has enough Mp used for the skill, it will calculate the remaining mana,
    // show Flytext the amount of Mp used, return TRUE
    public bool OnSkill(float manaUsed)
    {
        if (!IsDeath && mp >= manaUsed)
        {
            mp -= manaUsed;
            manaBar.SetNewPoint(mp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(manaUsed.ToString(), Color.gray);
            return true;
        }
        Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit("Mana!!", Color.cyan);
        return false;
    }

    //The function is used to display Flytext at the character's position by color and content
    public void SetFlyText(String mess , Color color)
    {
        Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(mess, color);
    }
}
