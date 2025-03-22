using UnityEngine;


/**UNDER CONSTRUCTION**/
public class ProjectileVisual : MonoBehaviour
{
    [SerializeField] Transform projectileVisual;
    [SerializeField] RefactoredProjectile projectile;
    
    public void InitializeVisual(RefactoredProjectile projectile) {
        this.projectile = projectile;
    }

    void Update() {
        UpdateProjectileRotation();
    }

    void UpdateProjectileRotation() {
        Vector3 projectileMoveDir = projectile.getProjectileMoveDir();

        projectileVisual.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg);
    }
}
