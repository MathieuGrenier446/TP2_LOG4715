using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float orbitDistance = 1.5f;
    [SerializeField] private Vector3 orbitOffset = new Vector3(0, 0.5f, 0);

    private Camera mainCamera;
    private Vector3 mousePos;

    public bool canFire;
    private float timer;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject iceBall;
    [SerializeField] private Transform ballTransform;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
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

        if (Input.GetMouseButton(0) && canFire) {
            canFire = false;
            Instantiate(iceBall, ballTransform.position, Quaternion.identity);
        }
    }
}

