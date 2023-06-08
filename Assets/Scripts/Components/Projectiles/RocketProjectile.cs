using UnityEngine;

public class RocketProjectile : Projectile
{

    protected override void MoveProjectile()
    {
        base.MoveProjectile();
        transform.Rotate(new Vector3(0.0f, 0.0f, 45.0f * Time.fixedDeltaTime));
    }
}
