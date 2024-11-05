	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
    public class PatrolState : IState
    {
        private EnemyController enemy;

        // pass in any parameters you need in the constructors
        public PatrolState(EnemyController enemy)
        {
            this.enemy = enemy;
        }


        public void Enter()
        {
            // code that runs when we first enter the state
            Debug.Log("Entering Patrol State");
        }


        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (!enemy.isPathClear)
            {
                enemy.Flip();
            }
            if (enemy.isPlayerInSight){
                enemy.StateMachine.TransitionTo(enemy.StateMachine.chaseState);
            }
        }


        public void Exit()
        {
            // code that runs when we exit the state
            Debug.Log("Exiting Patrol State");
        }
    }
