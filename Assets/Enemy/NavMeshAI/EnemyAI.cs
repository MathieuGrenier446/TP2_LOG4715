using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    // Patrolling
    public Transform[] destinationPoints;
    private int destinationPointIndex = 0;
    Vector3 target;
    private bool isIdle = false;
    // Attacking
    public float timeBetweenAttacks = 1;
    bool hasAlreadyAttacked;
    public GameObject projectile;
    // State
    public string playerObjectName = "MaleFree1";
    public float sightRange = 3;
    public float attackRange = 1;
    public bool isPlayerInSight = false;
    public bool isPlayerInAttackRange = false;
    
    void Awake()
    {
        player = GameObject.Find(playerObjectName).transform;
        agent = GetComponent<NavMeshAgent>();
        SetNextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerInSight = Physics.Raycast(transform.position, transform.forward, sightRange, whatIsPlayer);
        isPlayerInAttackRange = Physics.Raycast(transform.position, transform.forward, attackRange, whatIsPlayer);


        if (!isPlayerInSight) Patrol();   
        else if (!isPlayerInAttackRange) ChasePlayer();
        else AttackPlayer();
        
    }

    private void Patrol(){
        if (isIdle) {
            return;
        }
        bool isCloseToTarget = Vector3.Distance(transform.position, target) < 1;
        if (isCloseToTarget) {
            SetNextTarget();
            StartCoroutine(Idle());
        }
        agent.SetDestination(target);
    }
    private void ChasePlayer(){
        agent.SetDestination(player.position);
    }
    private void AttackPlayer(){
        agent.SetDestination(transform.position);
        if (!hasAlreadyAttacked) {
            hasAlreadyAttacked = true;
            ProjectileAttack();
            StartCoroutine(ResetAttack());
        }
    }
    private IEnumerator ResetAttack(){
        float initialTime = Time.time;
        yield return new WaitForSeconds(timeBetweenAttacks);
        hasAlreadyAttacked = false;
        float finalTime = Time.time;
        float duration = finalTime - initialTime;
        Debug.Log("Coroutine lasted" + duration);
    }
    void SetNextTarget() {
        if (destinationPoints.Length == 0) return;
        target = destinationPoints[destinationPointIndex].position;
        destinationPointIndex = (destinationPointIndex + 1) % destinationPoints.Length;
        
    }

    IEnumerator Idle(){
        isIdle = true;
        yield return new WaitForSeconds(5);
        isIdle = false;
    }

    private void ProjectileAttack() {
        Rigidbody rb = Instantiate(projectile, transform.position + new Vector3(0,0.5f,0), Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
    }
}
