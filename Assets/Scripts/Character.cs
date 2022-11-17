using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected HealthBar manaBar;
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

    public virtual void OnDeath()
    {
        ChangeAnim("Dead");
        Invoke(nameof(OnDespawn),2f);
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }
    public void OnHit(float damage)
    {
        if (!IsDeath)
        {
            hp -= damage;
            if (hp <= 0)
            {
                hp = 0;
                OnDeath();
            }
            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefab,transform.position+Vector3.up,Quaternion.identity).OnInit(damage.ToString(),Color.red);
        }
    }

    public bool OnSkill(float manaUsed)
    {
        if (!IsDeath && mp >= manaUsed)
        {
            mp -= manaUsed;
            manaBar.SetNewHp(mp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(manaUsed.ToString(), Color.gray);
            return true;
        }
        Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit("Out of mana", Color.cyan);
        return false;
    }

    public void SetFlyText(String mess , Color color)
    {
        Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(mess, color);
    }
}
