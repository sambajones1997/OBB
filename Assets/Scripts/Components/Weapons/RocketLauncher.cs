using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    protected override void FireWeapon(List<GameObject> projectileLocations)
    {
        foreach (GameObject go in projectileLocations)
        {
            Projectile proj = Instantiate(mWeaponProjectile, go.transform.position, go.transform.rotation).GetComponent<Projectile>();
            proj.mParentObject = gameObject;
            proj.mProjectileDamage = mWeaponDamage;
        }
    }
}
