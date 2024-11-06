	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

    public class ChaseState : IState
    {
        private EnemyController enemy;
        private float originalFOV;
        private float originalMoveSpeed;

        // pass in any parameters you need in the constructors
        public ChaseState(EnemyController enemy)
        {
            this.enemy = enemy;
        }


        public void Enter()
        {
            originalMoveSpeed = enemy.MoveSpeed;
            originalFOV = enemy.AngleFOV;
            // code that runs when we first enter the state
            Debug.Log("Entering Chase State");
            // TODO: Add exclamation mark above the enemy
        }


        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (!enemy.isPlayerInSight)
            {
                ExpressMildConfusion();
                enemy.StateMachine.TransitionTo(enemy.StateMachine.patrolState);
                return;
            }
            if(enemy.isPlayerInAttackRange){
                enemy.StateMachine.TransitionTo(enemy.StateMachine.attackState);
            } else {
                ChasePlayer();
            }
        }


        public void Exit()
        {
            enemy.MoveSpeed = originalMoveSpeed;
            enemy.AngleFOV = originalFOV;
            // code that runs when we exit the state
            Debug.Log("Exiting Chase State");
        }

        private void ChasePlayer(){
            enemy.MoveSpeed +=0.25f;
            enemy.GoForward();
        }

        private void ExpressMildConfusion(){
            enemy.Emote("...?", Color.yellow);
        }
    }
