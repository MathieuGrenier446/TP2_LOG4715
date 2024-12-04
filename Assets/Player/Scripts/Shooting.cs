using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : SoundEmitter
{
    [SerializeField] private Transform player;
    [SerializeField] private float orbitDistance = 1f;
    [SerializeField] private Vector3 orbitOffset = new Vector3(0, 0.5f, 0);

    [SerializeField] private Camera mainCamera;
    private Vector3 mousePos;

    public bool canFire;
    private float timer;
	public float ammoCd = 2f;
	public int maxAmmo = 10;
	public bool hasRangedWeapon = true;
	private float ammoTimer = 0;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject iceBall;
    [SerializeField] private Transform ballTransform;
    private MainMenu mainMenu;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainMenu = MainMenu.Instance;
    }

    void Update()
    {
        if (mainMenu && !mainMenu.getIsGameStart() || Time.timeScale == 0f)
        {
            return;
        }
        if (player == null) return;

        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCamera.transform.position.x - player.position.x);

        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(mousePos);

        Vector3 playerCenter = player.position + orbitOffset;
        Vector3 direction = worldMousePos - playerCenter;
        direction.x = 0;

        Vector3 orbitPosition = playerCenter + direction.normalized * orbitDistance;
        transform.position = orbitPosition;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        if(!canFire) {
            timer += Time.deltaTime;
            if(timer > fireRate) {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire && PlayerStats.Instance.ammo > 0) {
            canFire = false;
            Instantiate(iceBall, ballTransform.position, Quaternion.identity);
            PlaySound(shootSound);
            PlayerStats.Instance.ammo -= 1;
            PlayerStats.Instance.NotifyUI();
        }

        handleAmmo();
    }

    public void handleAmmo() {
	    if(hasRangedWeapon) {
        	ammoTimer += Time.deltaTime;
            if(ammoTimer > ammoCd && PlayerStats.Instance.ammo < maxAmmo) {
				PlayerStats.Instance.ammo += 1;
                PlayerStats.Instance.NotifyUI();
                ammoTimer = 0;
            }
		}
	}
}

