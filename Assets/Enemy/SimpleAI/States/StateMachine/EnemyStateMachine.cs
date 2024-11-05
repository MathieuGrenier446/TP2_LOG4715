	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	public class EnemyStateMachine
	{
		public IState CurrentState { get; private set; }


		// reference to the state objects
		public PatrolState patrolState;
		public ChaseState chaseState;
		public AttackState attackState;


		// event to notify other objects of the state change
		public event Action<IState> stateChanged;


		// pass in necessary parameters into constructor 
		public EnemyStateMachine(EnemyController enemy)
		{
			// create an instance for each state and pass in PlayerController
			this.patrolState = new PatrolState(enemy);
			this.chaseState = new ChaseState(enemy);
			this.attackState = new AttackState(enemy);
		}


		// set the starting state
		public void Initialize(IState state)
		{
			CurrentState = state;
			state.Enter();

			// notify other objects that state has changed
			stateChanged?.Invoke(state);
		}


		// exit this state and enter another
		public void TransitionTo(IState nextState)
		{
			CurrentState.Exit();
			CurrentState = nextState;
			nextState.Enter();


			// notify other objects that state has changed
			stateChanged?.Invoke(nextState);
		}


		// allow the StateMachine to update this state
		public void Update()
		{
			if (CurrentState != null)
			{
				CurrentState.Update();
			}
		}
	}
