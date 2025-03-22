using UnityEngine;


/**UNDER CONSTRUCTION**/
public class ProjectileVisual : MonoBehaviour
{
    private Transform projectileVisual;
    private RefactoredProjectile projectile;

    void Update() {
        UpdateProjectileRotation();
    }

    void UpdateProjectileRotation() {
        Vector3 projectileMoveDir = projectile.getProjectileMoveDir();

        projectileVisual.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg);
    }
}
