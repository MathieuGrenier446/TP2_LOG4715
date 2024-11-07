using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStateMachine StateMachine;
    Rigidbody _Rb { get; set; }
    Collider entityCollider;
    public
    Animator animator;
    public LayerMask WhatIsPlayer;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsWall;
    public LayerMask WhatIsEnemy;
    public TextMesh EmoteText;
    public float ChaseMoveSpeedMultiplier = 2.0f;
    public float Health = 100;
    public float MoveSpeed = 1f;
    public float AttackDamage = 5;
    public float AngleFOV = 120f;
    public float SightRange = 5f;
    public float AttackRange = 2f;
    public float wallDetectionRange = 0.2f;
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
    [HideInInspector]
    public bool isTouchingGround;
    private float direction;
    private float maxStuckDuration = 2;
    private float currentStuckDuration = 0;
    private Vector3 lastPosition;
    
    void Awake()
    {
        this.StateMachine = new EnemyStateMachine(this);
        this.StateMachine.Initialize(StateMachine.patrolState);
        _Rb = GetComponent<Rigidbody>();
        entityCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = transform.forward.z;
        UpdateIsPlayerInSight();
        UpdateIsPathClear();
        UpdateIsTouchingGround();
        this.StateMachine.Update();
    }


    public void Die(){
        PlayerStats.Instance.AwardEnemyKillExperience();
        Emote("Nooo!", Color.red);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage){
        this.Health = this.Health - damage;
        if (this.Health <= 0) {
            Die();
        }
        if (!isPlayerInSight){
            Reverse();
        }
        animator.SetTrigger("isHit");
        Emote("Ouch!", Color.red);
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
        PlayerController player = playerCollider.gameObject.GetComponent<PlayerController>();
        playerPosition = player.Target.position;
        return;
    }
    
    private void ResetPlayerSight(){
        this.isPlayerInSight = false;
        this.isPlayerInAttackRange = false;
    }

    private void UpdateIsPathClear(){
        this.isPathClear = IsPathAhead() && !IsPathBlocked() && !IsStuck();
        
    }

    public void AttackPlayer() {
        if (rangedWeapon.canFire) animator.SetTrigger("isAttacking");
        this.rangedWeapon.ShootAt(
            this.playerPosition
        );
    }

    public void Emote(string text, Color color)
    {
        CancelInvoke("HideEmote");
        EmoteText.text = text;
        EmoteText.color = color;
        EmoteText.gameObject.SetActive(true); 
        Invoke("HideEmote", 1);
    }

    private void HideEmote()
    {
        EmoteText.gameObject.SetActive(false);
    }

    private bool IsPathAhead(){
        float maxDistance = entityCollider.bounds.center.y - entityCollider.bounds.min.y + MaxFallDistance;
        Vector3 futurePosition = transform.position + _Rb.linearVelocity;
        return Physics.Raycast(futurePosition, Vector3.down, maxDistance, WhatIsGround);
    }

    private bool IsPathBlocked(){
        Vector3 halfDimensionsOfBox = new Vector3(0.0f, entityCollider.bounds.size.y*0.8f, wallDetectionRange/2);
        Vector3 centerOfBox = new Vector3(entityCollider.bounds.center.x, entityCollider.bounds.center.y, entityCollider.bounds.center.z+(entityCollider.bounds.extents.z+wallDetectionRange/2+0.1f)*direction);
        return Physics.CheckBox(centerOfBox, halfDimensionsOfBox, Quaternion.identity, WhatIsWall | WhatIsEnemy);
    }
    private bool IsStuck(){
        bool isStuckSinceLastFrame = transform.position == lastPosition;
        bool isStuck;
        if (!isStuckSinceLastFrame) {
            currentStuckDuration = 0;
            isStuck = false;
        } else {
            currentStuckDuration+=Time.deltaTime;
            isStuck = currentStuckDuration>maxStuckDuration;
            if (isStuck) currentStuckDuration = 0;
        }
        lastPosition = transform.position;
        return isStuck;
    }

    private void UpdateIsTouchingGround(){
        isTouchingGround = Physics.Raycast(transform.position, Vector3.down, 1f, WhatIsGround);
        if(!isTouchingGround){
            animator.SetBool("isFalling", true);
        } else {
            animator.SetBool("isFalling", false);
        }
    }
}
