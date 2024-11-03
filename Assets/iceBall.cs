using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private Vector3 mousePos;
    private Rigidbody rigidbody;

    [SerializeField] private float force = 5f;
    [SerializeField] private float lifeTime = 5f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        
        // Get the main camera
        Camera mainCam = Camera.main;

        // Get the mouse position in screen space, then convert it to world space on the YZ-plane
        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCam.transform.position.x - transform.position.x); // Set Z-depth to match distance from camera to object on X-axis
        Vector3 worldMousePos = mainCam.ScreenToWorldPoint(mousePos);

        // Calculate direction from the iceBall to the mouse position
        Vector3 direction = worldMousePos - transform.position;
        direction.x = 0; // Lock the X-axis for movement on the YZ-plane

        // Apply the initial velocity in the YZ direction
        rigidbody.linearVelocity = direction.normalized * force;
        StartCoroutine(DestroyAfterTime(lifeTime));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time); // Wait for the specified time
        Destroy(gameObject); // Destroy this game object
    }
}

