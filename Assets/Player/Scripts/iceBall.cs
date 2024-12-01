using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private Vector3 mousePos;
    private Rigidbody entityRigidbody;

    [SerializeField] private float force = 5f;
    [SerializeField] private float lifeTime = 5f;

    void Start()
    {
        entityRigidbody = GetComponent<Rigidbody>();
        Camera mainCam = Camera.main;

        mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCam.transform.position.x - transform.position.x);
        Vector3 worldMousePos = mainCam.ScreenToWorldPoint(mousePos);

        Vector3 direction = worldMousePos - transform.position;
        direction.x = 0;

        entityRigidbody.linearVelocity = direction.normalized * force;
        StartCoroutine(DestroyAfterTime(lifeTime));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.TakeDamage(force);
            Destroy(gameObject);
        }
        
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}

