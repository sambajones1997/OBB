using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool m_bDestroyOnLifeExpire = false;
    public float mProjectileSpeed = 15.0f;
    public float mProjectileMaxLifeTime = 5.0f;
    float mProjectileLifeTime = 5.0f;
    public int mProjectileDamage = 10;
    public GameObject mParentObject;

    public string mName = "";

    PoolSystem mPool;

    private void Start()
    {
        string poolName = mName + "_Pool";
        mPool = ObjectPoolingSystem.FindCreatePool(poolName);
    }

    private void FixedUpdate()
    {
        mProjectileLifeTime -= Time.fixedDeltaTime;
        if (mProjectileLifeTime <= 0.0f)
        {
            if (m_bDestroyOnLifeExpire)
                Destroy(gameObject);
            else
                ReleaseProjectileToPool();
        }
        else
        {
            MoveProjectile();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject collidingObject = collider.gameObject;
        if (collidingObject == null)
        {
            return;
        }

        if (collidingObject != mParentObject)
        {
            if (collidingObject.layer == 10)
            {
                Projectile proj = GetComponentInChildren<Projectile>(collidingObject.transform.root);
                if (proj && proj.mParentObject == mParentObject)
                    return;
            }

            DamagePackage damagePackage = new DamagePackage(mProjectileDamage, mParentObject);
            if (mParentObject == null)
            {
                Debug.Log("Oh no, my parent is null!");
            }
            Health HealthComponent = collidingObject.GetComponentInChildren<Health>();
            if (HealthComponent != null && !HealthComponent.IsDying())
            {
                collidingObject.SendMessage("TakeDamage", damagePackage, SendMessageOptions.DontRequireReceiver);
            }      
            ReleaseProjectileToPool();
        }
    }

    public void TakeDamage(DamagePackage damagePackage)
    {
        ReleaseProjectileToPool();
    }

    public void ResetProjectile()
    {
        mProjectileLifeTime = mProjectileMaxLifeTime;
    }

    public void ShootProjectile(Vector3 pos, Quaternion rot)
    {
        ResetProjectile();
        transform.position = pos;
        transform.rotation = rot;
        gameObject.SetActive(true);
    }

    public void ReleaseProjectileToPool()
    {
        ResetProjectile();
        if (mPool != null)
        {
            mPool.PoolObjects.Add(this);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void MoveProjectile()
    {
        transform.Translate(Vector3.forward * mProjectileSpeed * Time.fixedDeltaTime);
    }
}
