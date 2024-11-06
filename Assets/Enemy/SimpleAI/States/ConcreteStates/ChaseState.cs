	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

    public class ChaseState : IState
    {
        private EnemyController enemy;
        private float originalFOV;
        private float originalMoveSpeed;

        public ChaseState(EnemyController enemy)
        {
            this.enemy = enemy;
        }


        public void Enter()
        {
            originalMoveSpeed = enemy.MoveSpeed;
            originalFOV = enemy.AngleFOV;
            enemy.MoveSpeed *=enemy.ChaseMoveSpeedMultiplier;
        }


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
        }

        private void ChasePlayer(){
            enemy.GoForward();
        }

        private void ExpressMildConfusion(){
            enemy.Emote("...?", Color.yellow);
        }
    }