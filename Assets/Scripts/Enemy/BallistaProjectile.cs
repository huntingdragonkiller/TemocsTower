using UnityEngine;

public class BallistaProjectile : RefactoredProjectile
{

    //Changed so it doesnt delete the projectile if it collides with something not a tower
    protected override void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("Collision");
        if (collision.collider.gameObject.tag == "Ground") {
            KillRefactored();
        }else if(collision.collider.gameObject.tag != "Hitbox" && collision.collider.gameObject.tag != "Projectile"){
            Debug.Log("Collision: " + collision);
            Debug.Log("Collider: " + collision.collider);
            collision.collider.SendMessage("TakeDamage", damage);
            //Collateral Damage!
            if(collision.collider.gameObject.tag == "Tower")
                KillRefactored();
        }
        Debug.Log("Collision Done");
    }
}
