using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    float timer;
    float randomTime;

    // When entering the Idle state, randomly initialize the time in the Idle state and reset the timer
    public void OnEnter(Enemy enemy)
    {
        enemy.StopMoving();
        timer = 0;
        randomTime = Random.Range(2f,4f);
    }

    // When executed, the character will be in the Idle state at the end time
    // When finished, it will switch to the patrol state
    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;

        if (timer > randomTime)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }

}
