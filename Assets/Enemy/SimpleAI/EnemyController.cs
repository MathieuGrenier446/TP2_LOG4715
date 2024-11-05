using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStateMachine StateMachine;
    public bool isPlayerInSight;
    public bool isPathClear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.StateMachine.Initialize(StateMachine.patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flip(){}

    // check avec un FOV si le joueur est visible et in range
    private void IsPlayerInSight(){
        this.isPlayerInSight = false;
    }

    // check si le vide ou un obstacle est devant
    private void IsPathClear(){
        this.isPathClear = false;   
    }
}
