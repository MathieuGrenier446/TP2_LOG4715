public interface IDamageable {
    public float Health {get; set;}
    public void Die();
    public void TakeDamage(float damage);
}