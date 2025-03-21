using UnityEngine;
using System;

public class Shooter : MonoBehaviour
{

    private RefactoredProjectile projectilePrefab;

    private Transform targetTransform;

    private float projectileMaxMoveSpeed;
    private float projectileMaxHeight;

    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
    [SerializeField] private AnimationCurve speedAnimationCurve;

    // Update is called once per frame
    public void Shoot(Transform targetTransform)
    {

        Debug.Log("From Shooter yayyyyyyyyy");
        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        // projectile.InitializeProjectile(target, projectileMaxMoveSpeed, projectileMaxHeight);
        // projectile.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, speedAnimationCurve);
    }

    public void InitializeShooter(RefactoredProjectile projectilePrefab, float projectileMaxMoveSpeed, float projectileMaxHeight) {
        this.projectilePrefab = projectilePrefab;
        this.projectileMaxMoveSpeed = projectileMaxMoveSpeed;
        this.projectileMaxHeight = projectileMaxHeight;
    }
}
