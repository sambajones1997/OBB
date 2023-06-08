using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GamePlay/Health")]
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
        SendMessageUpwards("TakenDamage", damagePackage.mDamageDealer, SendMessageOptions.DontRequireReceiver);
        if (mHealth > 0 && (mHealth -= damagePackage.mDamage) <= 0)
        {
            mHealth = 0;
            SendMessageUpwards("Die", SendMessageOptions.RequireReceiver);
        }
    }

    public void AddHealth(int health)
    {
        mHealth += health;
        if (mHealth > mMaxHealth) mHealth = mMaxHealth;
    }

    public bool IsDying()
    {
        return mHealth <= 0;
    }
}
