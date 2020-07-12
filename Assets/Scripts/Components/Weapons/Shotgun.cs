using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shotgun : Weapon
{
    protected override void FireWeapon(List<GameObject> projectileLocations)
    {
        base.FireWeapon(projectileLocations);
        foreach (GameObject go in projectileLocations)
        {
            FireProjectile(go.transform.position, go.transform.rotation);
            FireProjectile(go.transform.position, Quaternion.Euler(go.transform.rotation.eulerAngles + new Vector3(0.0f, -5.0f, 0.0f)));
            FireProjectile(go.transform.position, Quaternion.Euler(go.transform.rotation.eulerAngles + new Vector3(0.0f, 5.0f, 0.0f)));
        }
    }
}
