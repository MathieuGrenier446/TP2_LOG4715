using UnityEngine;

public class Projectile: MonoBehaviour
{   
    public float Speed = 10f;
    public float Damage = 10f;
    public float LifeTime = 5f;

    private Vector3 direction;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    void Update()
    {
        transform.position += direction * Speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats.Instance.CurentHealthMod(-Damage);
        } else if (other.CompareTag("Enemy")){
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.TakeDamage(Damage);
        }
        Destroy(gameObject);
    }
}
