	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
    public class AttackState : IState
    {
        private EnemyController enemy;

        // pass in any parameters you need in the constructors
        public AttackState(EnemyController enemy)
        {
            this.enemy = enemy;
        }


        public void Enter()
        {
            // code that runs when we first enter the state
            Debug.Log("Entering Attack State");
        }


        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (enemy.isPlayerInSight)
            {
                // keep attacking
            }
        }


        public void Exit()
        {
            // code that runs when we exit the state
            Debug.Log("Exiting Attack State");
        }
    }
