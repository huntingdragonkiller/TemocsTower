using UnityEngine;
using System;

public class Shooter : MonoBehaviour
{

    private GameObject projectilePrefab;

    private Transform targetTransform;

    public ProjectileVisual projectileVisual;

    private float projectileMaxMoveSpeed;
    private float projectileMaxHeight;

    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
    [SerializeField] private AnimationCurve speedAnimationCurve;

    // Update is called once per frame
    public void Shoot(Transform targetTransform)
    {

        Debug.Log("Shooters shoot");

        Vector3 projectileSpawnPoint = transform.position;
        if (targetTransform.position.x - transform.position.x < 0) {
            projectileSpawnPoint.x -= 1f;
        } else {
            projectileSpawnPoint.x += 1f;
        }

        

        RefactoredProjectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint, Quaternion.identity, transform).GetComponent<RefactoredProjectile>();
        projectile.InitializeProjectile(targetTransform, projectileMaxMoveSpeed, projectileMaxHeight);
        projectile.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, speedAnimationCurve);
        
        try {
            projectileVisual = projectile.GetComponent<ProjectileVisual>();
            projectileVisual.InitializeVisual(projectile);
        } catch (Exception e) {
            Debug.Log("No Rotation");
        }
    }

    public void InitializeShooter(GameObject projectilePrefab, float projectileMaxMoveSpeed, float projectileMaxHeight) {
        this.projectilePrefab = projectilePrefab;
        this.projectileMaxMoveSpeed = projectileMaxMoveSpeed;
        this.projectileMaxHeight = projectileMaxHeight;
    }
}
