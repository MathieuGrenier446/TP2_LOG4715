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
    // Attacking
    public float timeBetweenAttacks = 3;
    bool hasAlreadyAttacked;
    public GameObject projectile;
    // State
    public string playerObjectName = "MaleFree1";
    public float sightRange = 3;
    public float attackRange = 1;
    public bool isPlayerInSight = false;
    public bool isPlayerInAttackRange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.Find(playerObjectName).transform;
        agent = GetComponent<NavMeshAgent>();
        SetNextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!isPlayerInSight) Patrol();   
        else if (!isPlayerInAttackRange) ChasePlayer();
        else AttackPlayer();
        
    }

    private void Patrol(){
        if (Vector3.Distance(transform.position, target) < 1) {
            SetNextTarget();
        }
        agent.SetDestination(target);
    }
    private void ChasePlayer(){
        agent.SetDestination(player.position);
    }
    private void AttackPlayer(){
        agent.SetDestination(transform.position);
        if (!hasAlreadyAttacked) {
            ProjectileAttack();
            hasAlreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack(){
        hasAlreadyAttacked = false;
    }
    void SetNextTarget() {
        if (destinationPoints.Length == 0) return;
        target = destinationPoints[destinationPointIndex].position;
        destinationPointIndex = (destinationPointIndex + 1) % destinationPoints.Length;
    }

    private void ProjectileAttack() {
        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }
}
