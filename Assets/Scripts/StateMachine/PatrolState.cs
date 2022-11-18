using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float timer;
    float randomTime;

    // When entering the Patrol state, randomly initialize the time in the Patrol state and reset the timer
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f);
    }

    // Character will move in a fixed direction during execution time
    // Receive object will go to Idle state when execution time is up
    // * If there is a target, the character will head towards the target
    // **If target is within attack range, switch to attack state
    // **If target is out of range, move towards target
    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;

        if (enemy.Target != null)
        {
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
            if (enemy.IsTargetInRange())
            {
                enemy.ChangeState(new AttackState());
            }
            else
            {
                enemy.Moving();
            }
            return;
        }
        if (timer < randomTime)
        {
            enemy.Moving();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
