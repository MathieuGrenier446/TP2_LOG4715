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
            enemy.StopMoving();
            // code that runs when we first enter the state
            Debug.Log("Entering Attack State");
        }


        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (!enemy.isPlayerInSight)
            {
                ExpressAnger();
                enemy.StateMachine.TransitionTo(enemy.StateMachine.patrolState);
                return;
            }
            if(!enemy.isPlayerInAttackRange){
                ExpressConfusion();
                enemy.StateMachine.TransitionTo(enemy.StateMachine.chaseState);
                return;
            }
            AttackPlayer();
        }


        public void Exit()
        {
            // code that runs when we exit the state
            Debug.Log("Exiting Attack State");
        }

        private void AttackPlayer(){
            
            enemy.AttackPlayer();
        }

        
        private void ExpressAnger(){
            enemy.Emote("#@?*!!!", Color.red);
        }

        private void ExpressConfusion(){
            Color orange = new Color(249, 180, 45, 1);
            enemy.Emote("!?", orange);
        }

    }
