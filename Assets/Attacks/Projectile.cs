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
            // PlayerControler player = other.GetComponent<PlayerControler>();
            // if (player != null)
            // {
            //     player.TakeDamage(damage);
            // }
            PlayerStats.Instance.CurentHealthMod(-Damage);
            Destroy(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
