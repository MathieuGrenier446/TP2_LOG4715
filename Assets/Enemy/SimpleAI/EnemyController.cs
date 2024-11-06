using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, IMovable, IUnitStats
{
    public EnemyStateMachine StateMachine;
    Rigidbody _Rb { get; set; }
    CapsuleCollider entityCollider;
    [SerializeField]
    WallCollider wallCollider;
    public LayerMask WhatIsPlayer;
    public LayerMask WhatIsGround;
    public float Health {get; set;} = 10;
    public float MoveSpeed { get; set; } = 1f;
    public float AttackDamage { get; set; }
    public float AngleFOV;
    public float SightRange;
    public float AttackRange;
    public RangedWeapon rangedWeapon;
    public float MaxFallDistance;
    [HideInInspector]
    public Vector3 playerPosition;
    [HideInInspector]
    public bool isPlayerInSight;
    [HideInInspector]
    public bool isPlayerInAttackRange;
    [HideInInspector]
    public bool isPathClear;
    private float direction;
    private float maxStuckDuration = 2;
    private float currentStuckDuration = 0;
    Vector3 futurePosition;
    Vector3 lastPosition;
    Vector3 sizeOfPathBlockedDetectionBox; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        this.StateMachine = new EnemyStateMachine(this);
        this.StateMachine.Initialize(StateMachine.patrolState);
        _Rb = GetComponent<Rigidbody>();
        entityCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIsPlayerInSight();
        UpdateIsPathClear();
        this.StateMachine.Update();
    }


    public void Die(){
        // TODO: give experience to player
        Destroy(gameObject);
    }

    public void TakeDamage(float damage){
        this.Health = this.Health - damage;
        if (this.Health <= 0) {
            Die();
        }
    }
    
    public void GoForward()
    {
        _Rb.linearVelocity = new Vector3(_Rb.linearVelocity.x, _Rb.linearVelocity.y, MoveSpeed * direction);
    }

    public void Reverse()
    {
        transform.Rotate(0,180,0);
    }

    public void StopMoving(){
        _Rb.linearVelocity = new Vector3(0,_Rb.linearVelocity.y,0);
    }

    public void OnTriggerEnter(Collider collider){
        // TODO: Add a flip when colliding with wall in the future.
    }
    
    private void UpdateIsPlayerInSight(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SightRange, WhatIsPlayer);
        if (hitColliders.Length==0){
            ResetPlayerSight();
            return;
        }
        Collider playerCollider = hitColliders[0];
        Vector3 directionToTarget = playerCollider.transform.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget.normalized);
        bool isTargetInFOV = angleToTarget < AngleFOV / 2;
        if (!isTargetInFOV){
            ResetPlayerSight();
            return;
        }
        RaycastHit hit;
        bool isRayCastHitValidObject = Physics.Raycast(transform.position, directionToTarget.normalized, out hit, SightRange, WhatIsGround | WhatIsPlayer);
        bool isSightUnobstructed = hit.collider.gameObject == playerCollider.gameObject;
        if (!isRayCastHitValidObject || !isSightUnobstructed){
            ResetPlayerSight();
            return;
        }
        float distanceToTarget = Vector3.Distance(transform.position, playerCollider.transform.position);
        this.isPlayerInAttackRange = distanceToTarget <=AttackRange;
        this.isPlayerInSight = true;
        this.playerPosition = playerCollider.transform.position;
        return;
    }
    
    private void ResetPlayerSight(){
        this.isPlayerInSight = false;
        this.isPlayerInAttackRange = false;
    }

    private void UpdateIsPathClear(){
        direction = transform.forward.z;
        float maxDistance = entityCollider.bounds.center.y - entityCollider.bounds.min.y + MaxFallDistance;
        futurePosition = transform.position + _Rb.linearVelocity;
        bool isFuturePositionOnGround = Physics.Raycast(futurePosition, Vector3.down, maxDistance, WhatIsGround);

        bool isPathBlocked = wallCollider.isColliding;
        wallCollider.isColliding = false;
        //bool isStuck = IsStuck();
        this.isPathClear = isFuturePositionOnGround && !isPathBlocked;
        lastPosition = transform.position;
    }

    public void AttackPlayer() {
        this.rangedWeapon.ShootAt(
            this.playerPosition
        );
    }

    public void Emote(string emote, Color color) {

    }

    private bool IsStuck(){
        bool isStuckSinceLastFrame = transform.position == lastPosition;
        if (!isStuckSinceLastFrame) {
            currentStuckDuration = 0;
            return false;
        }
        currentStuckDuration+=Time.deltaTime;
        bool isStuck = currentStuckDuration>maxStuckDuration;
        if (isStuck) {
            currentStuckDuration = 0;
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Vector3 colliderCenter = entityCollider.bounds.center;
        float widthZ = entityCollider.bounds.max.z;
        Vector3 pointOfOriginOfPathBlockedDetectionBox = new Vector3(colliderCenter.x, colliderCenter.y, colliderCenter.z+widthZ*direction*-1);
        Gizmos.DrawCube(pointOfOriginOfPathBlockedDetectionBox, sizeOfPathBlockedDetectionBox);
    }
}
