using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int mHealth = 100;
    public int mMaxHealth = 100;
    public int GetHealth() { return mHealth; }

    private void Start()
    {
        mHealth = mMaxHealth;
    }

    public void TakeDamage(DamagePackage damagePackage)
    {
        if (mHealth > 0 && (mHealth -= damagePackage.mDamage) <= 0)
        {
            mHealth = 0;
            if (GetComponentInParent<ShipController>()) GetComponentInParent<ShipController>().mLastCauseOfDamage = damagePackage.mDamageDealer;
            SendMessageUpwards("Die", SendMessageOptions.RequireReceiver);
        }
    }

    public void AddHealth(int health)
    {
        mHealth += health;
        if (mHealth > mMaxHealth) mHealth = mMaxHealth;
    }
}
