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
    bool bSetup = false;

    PoolSystem ProjectilePool;
    Projectile projectileComp;

    public void SetupWeapon(List<GameObject> projectileLocations)
    {
        if (!mWeaponController)
            mWeaponController = transform.root.GetComponentInChildren<WeaponController>();

        if (!mAudioSource)
            mAudioSource = transform.root.GetComponentInChildren<AudioSource>();

        if (!mWeaponController || !mAudioSource || !mWeaponSound || !mWeaponProjectile)
            Destroy(this);

        projectileComp = mWeaponProjectile.GetComponent<Projectile>();
        if (projectileComp != null)
        {
            ProjectilePool = ObjectPoolingSystem.FindCreatePool(ObjectPoolingSystem.GetPoolName(projectileComp.mName));
        }

        bSetup = true;
    }

    protected void FixedUpdate()
    {
        if (!bSetup)
        {
            return;
        }

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
        if (mFireRateTimer <= 0.0f && bSetup)
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
        {
            if (go != null && go.transform != null)
            {
                FireProjectile(go.transform.position, go.transform.rotation);
            }
        }
    }

    protected void FireProjectile(Vector3 spawnPos, Quaternion rotation)
    {
        Projectile projectile = RequestProjectile();
        if (projectile != null)
        {
            projectile.ShootProjectile(spawnPos, rotation);
            ++mWeaponController.m_ammoIndex;
            if (mWeaponController.m_ammoIndex >= mWeaponController.mProjectiles.Count)
                mWeaponController.m_ammoIndex = 0;
        }
    }

    protected Projectile RequestProjectile()
    {
        Projectile projectile = null;
        if (mWeaponController && ProjectilePool != null)
        {
            projectile = ObjectPoolingSystem.GetObjectFromPool<Projectile>(ProjectilePool);
            if (projectile != null)
            {
                Debug.Log("Grabbed Projectile From Pool: " + ProjectilePool.Name);
                return projectile;
            }
        }

        string projectileName = "";
        if (projectileComp != null)
        {
            projectileName = projectileComp.mName;
        }
        Debug.Log("Created Projectile: " + ObjectPoolingSystem.GetPoolName(projectileName));
        projectile = Instantiate(mWeaponProjectile, Vector3.zero, new Quaternion()).GetComponent<Projectile>();
        projectile.mParentObject = mWeaponController.GetShipController().gameObject;
        projectile.mProjectileDamage = mWeaponDamage;
        projectile.ResetProjectile();
        ++mAmmo;

        return projectile;
    }

    IEnumerator SpawnProjectiles(List<GameObject> projectileLocations)
    {
        List<Projectile> projectiles = new List<Projectile>();
        for (int i = 0; i < m_MaxAmmo * 2 * (projectileLocations.Count); ++i)
        {
            Projectile proj = Instantiate(mWeaponProjectile, Vector3.zero, new Quaternion()).GetComponent<Projectile>();
            proj.mParentObject = mWeaponController.GetShipController().gameObject;
            proj.mProjectileDamage = mWeaponDamage;
            proj.ResetProjectile();
            projectiles.Add(proj);
            ++mAmmo; // we can increase the amount of ammo we have available with each spawn so no invalid firing will be done without and extra logic check
        }
        mWeaponController.mProjectiles = projectiles;
        yield return null;
    }
}
