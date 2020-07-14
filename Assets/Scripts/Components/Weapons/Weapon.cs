using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponController mWeaponController; // Serialized so in most cases we can drag in the component in prefabs
    [SerializeField] protected GameObject mWeaponProjectile;
    [SerializeField] protected AudioSource mAudioSource;
    [SerializeField] protected AudioClip mWeaponSound;
    protected float mFireRateTimer = 0.0f;
    protected float mReloadTimer = 0.0f;
    int mAmmo = 0;
    [SerializeField] protected bool mUseAmmo = false;
    [SerializeField] protected int mMaxAmmo = 10;
    [SerializeField] protected float mFireRate = 10;
    [SerializeField] protected int mWeaponDamage = 10;
    [SerializeField] protected float mMaxReloadTimer = 1.0f;

    protected void Start()
    {
        mAmmo = mMaxAmmo;
    }

    public void SetupWeapon()
    {
        if (!mWeaponController)
            mWeaponController = transform.root.GetComponentInChildren<WeaponController>();

        if (!mAudioSource)
            mAudioSource = transform.root.GetComponentInChildren<AudioSource>();

        if (!mWeaponController || !mAudioSource || !mWeaponSound || !mWeaponProjectile)
            Destroy(this);
    }

    protected void FixedUpdate()
    {
        if (mFireRateTimer > 0.0f && mAmmo > 0)
        {
            mFireRateTimer -= Time.fixedDeltaTime;
        }
        else if (mAmmo <= 0)
        {
            mReloadTimer -= Time.fixedDeltaTime;
            if (mReloadTimer <= 0.0f)
            {
                mAmmo = mMaxAmmo;
            }
        }
    }

    public void Fire(List<GameObject> projectileLocations)
    {
        if (mFireRateTimer <= 0.0f)
        {
            mAudioSource.Play();
            mFireRateTimer = 1.0f / mFireRate;
            FireWeapon(projectileLocations);
            if (mUseAmmo)
            {
                --mAmmo;
                if (mAmmo <= 0)
                    mReloadTimer = mMaxReloadTimer;
            }
        }
    }

    protected virtual void FireWeapon(List<GameObject> projectileLocations) 
    {
        foreach (GameObject go in projectileLocations)
            FireProjectile(go.transform.position, go.transform.rotation);
    }

    protected void FireProjectile(Vector3 spawnPos, Quaternion rotation)
    {
        Projectile proj = Instantiate(mWeaponProjectile, spawnPos, rotation).GetComponent<Projectile>();
        proj.mParentObject = mWeaponController.GetShipController().gameObject;
        proj.mProjectileDamage = mWeaponDamage;
    }
}
