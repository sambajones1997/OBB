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
    [SerializeField] public List<Projectile> mProjectiles = new List<Projectile>();
    public int m_ammoIndex = 0;
    public void FireWeapon()
    {
        if (mEquippedWeapon && mProjectileLocations.Count > 0)
            mEquippedWeapon.Fire(mProjectileLocations);
    }

    public void Update()
    {
        if (!mEquippedWeapon)
            return;
        OnWeaponControllerUpdate();
    }

    protected virtual void OnWeaponControllerUpdate()
    {

    }

    protected void Start()
    {
        mEquippedWeapon = GetComponentInChildren<Weapon>();
        if (mEquippedWeapon)
        {
            mEquippedWeapon.SetupWeapon(mProjectileLocations);
        }
        OnWeaponControllerStart();
    }
    protected virtual void OnWeaponControllerStart()
    {

    }

    public ShipController GetShipController()
    {
        return mShipController;
    }


    // mostly to be used by player to swap weapons
    public void ResetWeaponController()
    {
        foreach (Projectile p in mProjectiles)
        {
            p.m_bDestroyOnLifeExpire = true; // all projectiles will destroy themselves on lifetime expiration
        }
        mProjectiles.Clear(); // clear this list
        Start(); // Restart this component
    }
}
