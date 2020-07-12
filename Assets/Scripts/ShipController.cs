using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject mLastCauseOfDamage;
    protected Health mHealthComponent;
    protected WeaponController mWeaponController;

    protected bool bIsAlive = true;
    protected float mShipRotationSpeed = 90.0f;
    void Start()
    {
        mWeaponController = GetComponentInChildren<WeaponController>();
        mHealthComponent = GetComponent<Health>();
        OnShipStart();
        if (!mWeaponController)
            Die();
    }

    private void Update()
    {
        OnShipUpdate();
    }

    private void FixedUpdate()
    {
        OnShipFixedUpdate();
    }

    protected void Die()
    {
        OnShipDie();
    }

    protected virtual void OnShipStart() { }
    protected virtual void OnShipFixedUpdate() { }
    protected virtual void OnShipUpdate() { }
    protected virtual void RotateShip() { }
    protected virtual void OnShipDie() { }
}
