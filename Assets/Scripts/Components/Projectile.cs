using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool m_bIsRocket = false;
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
            transform.Translate(Vector3.forward * mProjectileSpeed * Time.fixedDeltaTime);
            if (m_bIsRocket) transform.Rotate(new Vector3(0.0f, 0.0f, 45.0f * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != null && collider.gameObject != mParentObject)
        {
            if (collider.gameObject.layer == 10)
            {
                Projectile proj = GetComponentInChildren<Projectile>(collider.gameObject.transform.root);
                if (proj && proj.mParentObject == mParentObject)
                    return;
            }
            DamagePackage damagePackage = new DamagePackage(mProjectileDamage, mParentObject);
            if (collider.transform.gameObject != null)
            {
                collider.transform.gameObject.SendMessage("TakeDamage", damagePackage, SendMessageOptions.DontRequireReceiver);
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
        gameObject.SetActive(false);
    }

    public void ShootProjectile(Vector3 pos, Quaternion rot)
    {
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
}
