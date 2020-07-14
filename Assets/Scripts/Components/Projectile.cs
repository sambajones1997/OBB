using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool bIsRocket = false;
    public float mProjectileSpeed = 15.0f;
    public float mProjectileLifeTime = 5.0f;
    public int mProjectileDamage = 10;
    public GameObject mParentObject;

    private void FixedUpdate()
    {
        mProjectileLifeTime -= Time.fixedDeltaTime;
        if (mProjectileLifeTime <= 0.0f) Destroy(gameObject);
        else
        {
            transform.Translate(Vector3.forward * mProjectileSpeed * Time.fixedDeltaTime);
            if (bIsRocket) transform.Rotate(new Vector3(0.0f, 0.0f, 45.0f * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != mParentObject)
        {
            if (collider.gameObject.layer == 10)
            {
                Projectile proj = GetComponentInChildren<Projectile>(collider.gameObject.transform.root);
                if (proj && proj.mParentObject == mParentObject)
                    return;
            }
            DamagePackage damagePackage = new DamagePackage(mProjectileDamage, mParentObject);
            collider.transform.root.SendMessage("TakeDamage", damagePackage);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(DamagePackage damagePackage)
    {
        Destroy(gameObject);
    }
}
