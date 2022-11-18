using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    float timer = 0;
    float hitRate = 1.5f;

    // When entering the attack state, if there is a target, it will
    // Aim at the target, stop moving and attack
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

    // Switch back to patrol mode when the attack is done
    // Equivalent to waiting time for each attack
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
