using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponController mWeaponController; // Serialized so in most cases we can drag in the component in prefabs
    [SerializeField] protected GameObject mWeaponProjectile;
    [SerializeField] protected AudioSource mAudioSource;
    [SerializeField] protected AudioClip mWeaponSound;
    [SerializeField] protected float mMaxReloadTimer = 1.0f;
    [SerializeField] protected float mFireRate = 10;
    protected float mFireRateTimer = 0.0f;
    protected float mReloadTimer = 0.0f;
    [SerializeField] protected bool mUseAmmo = false;
    [SerializeField] protected int mWeaponDamage = 10;
    [SerializeField] protected int m_MaxAmmo = 10;
    int mAmmo = 0;

    protected void Start()
    {
        mAmmo = m_MaxAmmo;
    }

    public void SetupWeapon(List<GameObject> projectileLocations)
    {
        if (!mWeaponController)
            mWeaponController = transform.root.GetComponentInChildren<WeaponController>();

        if (!mAudioSource)
            mAudioSource = transform.root.GetComponentInChildren<AudioSource>();

        if (!mWeaponController || !mAudioSource || !mWeaponSound || !mWeaponProjectile)
            Destroy(this);

         List<Projectile> projectiles = new List<Projectile>();

        for (int i = 0; i < m_MaxAmmo * 2 * (projectileLocations.Count); ++i)
        {
            Projectile proj = Instantiate(mWeaponProjectile, Vector3.zero, new Quaternion()).GetComponent<Projectile>();
            proj.mParentObject = mWeaponController.GetShipController().gameObject;
            proj.mProjectileDamage = mWeaponDamage;
            proj.ResetProjectile();
            projectiles.Add(proj);
        }
        mWeaponController.mProjectiles = projectiles;
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
                mAmmo = m_MaxAmmo;
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
        mWeaponController.mProjectiles[mWeaponController.m_ammoIndex].ShootProjectile(spawnPos, rotation);
        ++mWeaponController.m_ammoIndex;
        if (mWeaponController.m_ammoIndex >= mWeaponController.mProjectiles.Count)
            mWeaponController.m_ammoIndex = 0;
    }
}
