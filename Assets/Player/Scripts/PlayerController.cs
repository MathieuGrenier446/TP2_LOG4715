﻿using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : SoundEmitter
{
    // Déclaration des constantes
    private static readonly Vector3 FlipRotation = new Vector3(0, 180, 0);
    private static readonly Vector3 CameraPosition = new Vector3(10, 1, 0);
    private static readonly Vector3 InverseCameraPosition = new Vector3(-10, 1, 0);

    // Déclaration des variables
    bool _Grounded { get; set; }
    bool _Flipped { get; set; }
    Animator _Anim { get; set; }
    Rigidbody _Rb { get; set; }
    Camera _MainCamera { get; set; }
    public Transform Target;

    MainMenu mainMenu;

    // Valeurs exposées
    [SerializeField]
    float MoveSpeed = 5.0f;

    [SerializeField]
    float JumpForce = 10f;

    [SerializeField]
    LayerMask WhatIsGround;

    public float SlopeLimit = 45f; 
    public float SlopeGroundCheckDistance = 1.1f;

    // Dash
    
    [SerializeField]
    float DashForce = 24f;
    [SerializeField]
    float DashCooldown = 1f;
    [SerializeField]
    float DashingTime = 0.2f;
    [SerializeField]
    public bool dashUnlocked = false;
    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField] private float iFrame = 1f;

    [SerializeField] private AudioClip damageTakenSound;
    private float timer = 0;
    private bool inIframe = false;
    

    // Awake se produit avait le Start. Il peut être bien de régler les références dans cette section.
    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _MainCamera = Camera.main;
    }

    // Utile pour régler des valeurs aux objets
    void Start()
    {
        _Grounded = false;
        _Flipped = false;
        mainMenu = MainMenu.Instance;
    }

    // Vérifie les entrées de commandes du joueur
    void Update()
    {
        if (mainMenu && !mainMenu.getIsGameStart())
        {
            return;
        }
        if (isDashing){
            return;
        }
        var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
        HorizontalMove(horizontal);
        FlipCharacter(horizontal);
        CheckJump();
        CheckDash();

        if(inIframe) {
            
            timer += Time.deltaTime;
            if(timer > iFrame) {
                inIframe = false;
                timer = 0;
            }
        }
    }

    // Gère le mouvement horizontal
    void HorizontalMove(float horizontal)
    {
        if (horizontal != 0) {
            bool isHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, SlopeGroundCheckDistance, WhatIsGround);
            if (isHit){
                Vector3 groundNormal = hit.normal;
                float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
                if (slopeAngle > SlopeLimit)
                {
                    horizontal = 0;
                }
            }
        }
        _Rb.linearVelocity = new Vector3(_Rb.linearVelocity.x, _Rb.linearVelocity.y, horizontal);
        _Anim.SetFloat("MoveSpeed", Mathf.Abs(horizontal));
    }

    // Gère le saut du personnage, ainsi que son animation de saut
    void CheckJump()
    {
        if (_Grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _Rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
            }
        }
    }

    // Gère l'orientation du joueur et les ajustements de la camera
    void FlipCharacter(float horizontal)
    {
        if (horizontal < 0 && !_Flipped)
        {
            _Flipped = true;
            transform.Rotate(FlipRotation);
            _MainCamera.transform.Rotate(-FlipRotation);
            _MainCamera.transform.localPosition = InverseCameraPosition;
        }
        else if (horizontal > 0 && _Flipped)
        {
            _Flipped = false;
            transform.Rotate(-FlipRotation);
            _MainCamera.transform.Rotate(FlipRotation);
            _MainCamera.transform.localPosition = CameraPosition;
        }
    }

    // Collision avec le sol
    void OnCollisionEnter(Collision coll)
    {        
        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) == 0)
            return;

        // Évite une collision avec le plafond
        if (coll.relativeVelocity.y > 0)
        {
            _Grounded = true;
            _Anim.SetBool("Grounded", _Grounded);
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Obstacle" && !inIframe) {
            TakeDamage(20);
            inIframe = true;
        } else if (coll.CompareTag("Projectile")){
            Projectile projectile = coll.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.Damage);
            if (PlayerStats.Instance.GetHealth() == 0){
                Destroy(gameObject);
            }
        }
        if (coll.gameObject.tag == "Currency") {
            PlayerStats.Instance.CurrencyMod(1);
        }
    }

    void OnTriggerStay(Collider coll) {
        if (coll.gameObject.tag == "Obstacle" && !inIframe) {
            TakeDamage(20);
            inIframe = true;
        }
    }

    void CheckDash()
    {
        if (dashUnlocked && Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void TakeDamage(float healthLoss)
    {
        PlayerStats.Instance.CurrentHealthMod(-1*healthLoss);
        PlaySound(damageTakenSound);
    }

    private IEnumerator Dash()
    {
        _Rb.useGravity = false;
        isDashing = true;
        Vector3 forceVector = new Vector3(0, 0, DashForce * transform.forward.z);
        // _Rb.AddForce(forceVector);
        _Rb.linearVelocity = forceVector;
        canDash = false;
        yield return new WaitForSeconds(DashingTime);
        _Rb.useGravity = true;
        isDashing = false;
        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
    }
}