using UnityEngine;

public class RangedWeapon: MonoBehaviour
{
    private float timeSinceLastShot = 0;
    private bool canFire = true;
    public float ShootingCooldown;
    [SerializeField]  Projectile projectile;
    [SerializeField] private Transform projectileExitPoint;

    void Update()
    {
        HandleWeaponCooldown();
    }

    private void HandleWeaponCooldown(){
        if(!canFire) {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > ShootingCooldown) {
                canFire = true;
                timeSinceLastShot = 0;
            }
        }
    }

    public void ShootAt(Vector3 targetPosition) {
        if (!canFire){
            return;
        }
        canFire = false;
        Vector3 directionToTarget = (targetPosition - projectileExitPoint.position).normalized;
        Projectile newProjectile = Instantiate(projectile, projectileExitPoint.position, Quaternion.LookRotation(directionToTarget));
        newProjectile.SetDirection(directionToTarget);
    }

}