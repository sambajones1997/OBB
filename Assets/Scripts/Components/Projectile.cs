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
        if (collider.gameObject.GetComponentInParent<Projectile>())
        {
            Projectile proj = collider.gameObject.GetComponentInParent<Projectile>();
            if (proj.bIsRocket)
            {
                if (proj.mParentObject == mParentObject) return;
            }
            else
            {
                if (proj.mParentObject == mParentObject) return;
            }

        }
        if (collider.gameObject != mParentObject)
        {
            if (collider.gameObject.GetComponent<Health>()) collider.gameObject.GetComponent<Health>().Damage(mProjectileDamage, mParentObject);
            Destroy(gameObject);
        }
    }
}
