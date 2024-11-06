using UnityEngine;

public class Currency : MonoBehaviour
{
    public float rotationSpeed = 180.0f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}