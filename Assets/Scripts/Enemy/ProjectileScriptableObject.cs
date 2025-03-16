using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileScriptableObject", menuName = "ScriptableObjects/Projectile")]
public class ProjectileScriptableObject : ScriptableObject
{
    //Base stats for enemies
    [SerializeField]
    float speed;
    public float Speed { get => speed; set => speed = value; }
    
    [SerializeField]
    float launchAngle;
    public float LaunchAngle { get => launchAngle; set => launchAngle = value; }
    
    [SerializeField]
    float damage;
    public float Damage { get => damage; set => damage = value; }

}
