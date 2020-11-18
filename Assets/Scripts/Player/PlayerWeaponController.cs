using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : WeaponController
{
    [SerializeField] protected Weapon mDefaultWeapon;

    float mWeaponTimeRemaining = 0.0f;
    float mMaxWeaponTime = 10.0f;
    bool mUsingDefaultWeapon = true;

    public void PickUpNewWeapon(GameObject newWeapon)
    {
        newWeapon.transform.parent = transform.root;
        if (!mUsingDefaultWeapon)
        {
            UnequipAndDestroyWeapon();
        }
        SwapWeapon(newWeapon.GetComponent<Weapon>(), false);
    }

    protected void UnequipAndDestroyWeapon()
    {
        mEquippedWeapon = null;
        Destroy(mEquippedWeapon.gameObject);
        SwapWeapon(mDefaultWeapon, true);
    }

    public void SwapWeapon(Weapon newWeapon, bool usingDefaultWeapon = false)
    {
        mEquippedWeapon = newWeapon;
        if (!usingDefaultWeapon)
            mEquippedWeapon.SetupWeapon(mProjectileLocations);
        mWeaponTimeRemaining = mMaxWeaponTime;
        mUsingDefaultWeapon = usingDefaultWeapon;
    }

    public void AddWeaponTime(float time)
    {
        mWeaponTimeRemaining += Mathf.Clamp(mWeaponTimeRemaining + time, 0.0f, mMaxWeaponTime);
    }

    public void MaximiseTime()
    {
        mWeaponTimeRemaining = mMaxWeaponTime;
    }

    protected override void OnWeaponControllerUpdate()
    {
        base.OnWeaponControllerUpdate();
        if (!mUsingDefaultWeapon)
        {
            mWeaponTimeRemaining -= Time.deltaTime;
            if (mWeaponTimeRemaining <= 0.0f)
            {
                SwapWeapon(mDefaultWeapon, true);
            }
        }
    }

    protected override void OnWeaponControllerStart()
    {
        if (!mEquippedWeapon && mDefaultWeapon)
            mEquippedWeapon = mDefaultWeapon;
        else if (mEquippedWeapon && !mDefaultWeapon)
            mDefaultWeapon = mEquippedWeapon;
        else
            mEquippedWeapon = GetComponentInChildren<Weapon>();
    }
}
