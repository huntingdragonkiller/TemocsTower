using UnityEngine;
using System;

public class Shooter : MonoBehaviour
{

    private GameObject projectilePrefab;

    private Transform targetTransform;

    private float projectileMaxMoveSpeed;
    private float projectileMaxHeight;

    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
    [SerializeField] private AnimationCurve speedAnimationCurve;

    // Update is called once per frame
    public void Shoot(Transform targetTransform)
    {

        Debug.Log("Shooters shoot");
        RefactoredProjectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<RefactoredProjectile>();
        projectile.InitializeProjectile(targetTransform, projectileMaxMoveSpeed, projectileMaxHeight);
        projectile.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, speedAnimationCurve);
    }

    public void InitializeShooter(GameObject projectilePrefab, float projectileMaxMoveSpeed, float projectileMaxHeight) {
        this.projectilePrefab = projectilePrefab;
        this.projectileMaxMoveSpeed = projectileMaxMoveSpeed;
        this.projectileMaxHeight = projectileMaxHeight;
    }
}
