using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    float timer = 0;
    float hitRate = 1.5f;
    public void OnEnter(Enemy enemy)
    {
        if(enemy.Target != null)
        {
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);

            enemy.StopMoving();
            enemy.Attack();
        }
        timer = 0;
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer >= hitRate)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }

    
}
