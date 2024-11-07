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
            if(!enemy.isTouchingGround){
                return;
                
            }
            else if (enemy.isPlayerInAttackRange){
                ExpressBigSuprise();
                enemy.StateMachine.TransitionTo(enemy.StateMachine.attackState);
                
            } else if (enemy.isPlayerInSight){
                ExpressSuprise();
                enemy.StateMachine.TransitionTo(enemy.StateMachine.chaseState);
            } else {
                Patrol();
            }
        }

        public void Exit()
        {
            enemy.animator.SetBool("isWalking", false);
            // code that runs when we exit the state
            Debug.Log("Exiting Patrol State");
        }

        private void Patrol(){
            if (!enemy.isPathClear) {
                enemy.animator.SetBool("isWalking", false);
                enemy.animator.SetTrigger("isReacting");
                enemy.StopMoving();
                enemy.Reverse();
            } else {
                enemy.animator.SetBool("isWalking", true);
                enemy.GoForward();
            }
        }

        
        private void ExpressSuprise(){
            enemy.animator.SetTrigger("isReacting");
            enemy.Emote("!", Color.red);
        }

        private void ExpressBigSuprise(){
            enemy.animator.SetTrigger("isReacting");
            enemy.Emote("!!!", Color.red);
        }
    }
