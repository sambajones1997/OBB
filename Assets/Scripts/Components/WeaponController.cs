using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Weapon controller will have a child class - player weapon controller which will handle the swapping of weapons
// this one will simply handle the firing and usage of equipped weapon.
public class WeaponController : MonoBehaviour
{
    [SerializeField] protected Weapon mEquippedWeapon;
    [SerializeField] protected ShipController mShipController;
    [SerializeField] protected List<GameObject> mProjectileLocations;
    public void FireWeapon()
    {
        if (mEquippedWeapon && mProjectileLocations.Count > 0)
            mEquippedWeapon.Fire(mProjectileLocations);
    }

    public void Update()
    {
        OnWeaponControllerUpdate();
    }

    protected virtual void OnWeaponControllerUpdate()
    {

    }

    protected void Start()
    {
        mEquippedWeapon = GetComponentInChildren<Weapon>();
        mEquippedWeapon.SetupWeapon();
        OnWeaponControllerStart();
    }
    protected virtual void OnWeaponControllerStart()
    {

    }

    public ShipController GetShipController()
    {
        return mShipController;
    }
}
