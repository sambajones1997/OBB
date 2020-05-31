using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum EWeaponType { Blaster, Shotgun, Rockets};
    public EWeaponType mWeaponType;
    AudioSource mAudioSource;
    public AudioClip mEnergySound;
    public AudioClip mRocketSound;
    public float mFireRateTimer = 0.0f;
    public List<GameObject> mWeaponSlots;
    public GameObject mEnergyObject;
    public GameObject mRockets;
    public int mFireRate = 10;
    public int mWeaponAmmo = 10;
    public int mMaxWeaponAmmo = 10;
    public bool bUsesAmmo = false;
    public float mReloadTime = 2.0f;
    bool bIsReloading = false;
    public bool bWeaponCanChange = false;
    float mAmmoRegenTimer = 0.0f;
    public int mWeaponDamage = 10;

    private void Start()
    {
        mWeaponAmmo = mMaxWeaponAmmo;
        mAudioSource = GetComponentInParent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (mFireRateTimer > 0.0f) mFireRateTimer -= Time.fixedDeltaTime;
        if (bIsReloading)
        {
            mAmmoRegenTimer -= Time.fixedDeltaTime;
            if (mAmmoRegenTimer <= 0.0f)
            {
                Reload();
                bIsReloading = false;
            }
        }
    }

    public void Fire()
    {
        if (mFireRateTimer <= 0.0f && mWeaponAmmo > 0)
        {
            mAudioSource.Play();
            switch (mWeaponType)
            {
                case EWeaponType.Blaster:
                    FireBlaster();
                    mFireRate = 10;
                    break;
                case EWeaponType.Shotgun:
                    FireShotgun();
                    mFireRate = 5;
                    break;
                case EWeaponType.Rockets:
                    FireRockets();
                    mFireRate = 1;
                    break;
            }
            mFireRateTimer = 1.0f / mFireRate;
            UseAmmo();
        }
    }

    void FireShotgun()
    {
        foreach (GameObject go in mWeaponSlots)
        {
            Projectile proj = Instantiate(mEnergyObject, go.transform.position, go.transform.rotation).GetComponent<Projectile>();
            proj.mParentObject = gameObject;
            proj.mProjectileDamage = mWeaponDamage;
            proj = Instantiate(mEnergyObject, go.transform.position, Quaternion.Euler(go.transform.rotation.eulerAngles + new Vector3 (0.0f,-5.0f,0.0f))).GetComponent<Projectile>();
            proj.mParentObject = gameObject;
            proj.mProjectileDamage = mWeaponDamage;
            proj = Instantiate(mEnergyObject, go.transform.position, Quaternion.Euler(go.transform.rotation.eulerAngles + new Vector3(0.0f, 5.0f, 0.0f))).GetComponent<Projectile>();
            proj.mParentObject = gameObject;
            proj.mProjectileDamage = mWeaponDamage;
        }
    }

    void FireBlaster()
    {
        foreach (GameObject go in mWeaponSlots)
        {
            Projectile proj = Instantiate(mEnergyObject, go.transform.position, go.transform.rotation).GetComponent<Projectile>();
            proj.mParentObject = gameObject;
            proj.mProjectileDamage = mWeaponDamage;
        }
    }

    void FireRockets()
    {
        foreach (GameObject go in mWeaponSlots)
        {
            Projectile proj = Instantiate(mRockets, go.transform.position, go.transform.rotation).GetComponent<Projectile>();
            proj.mParentObject = gameObject;
            proj.mProjectileDamage = mWeaponDamage;
        }
    }

    void UseAmmo()
    {
        if (bUsesAmmo)
        {
            mWeaponAmmo--;
            if (mWeaponAmmo <= 0)
            {
                if (!bWeaponCanChange)
                {
                    mAmmoRegenTimer = mReloadTime;
                    bIsReloading = true;
                }
                else
                {
                    ChangeWeaponType(EWeaponType.Blaster);
                }
            }
        }
    }

    public void Reload()
    {
        mWeaponAmmo = mMaxWeaponAmmo;
    }

    public int GetWeaponAmmo()
    {
        return mWeaponAmmo;
    }

    public void ChangeWeaponType(EWeaponType newWeaponType)
    {
        if (bWeaponCanChange)
        {
            mWeaponType = newWeaponType;
            switch (mWeaponType)
            {
                case EWeaponType.Shotgun:
                    mWeaponAmmo = 10;
                    mFireRate = 5;
                    mWeaponDamage = 5;
                    bUsesAmmo = true;
                    mAudioSource.clip = mEnergySound;
                    break;
                case EWeaponType.Rockets:
                    mWeaponAmmo = 5;
                    mFireRate = 1;
                    mWeaponDamage = 25;
                    bUsesAmmo = true;
                    mAudioSource.clip = mRocketSound;
                    break;
                case EWeaponType.Blaster:
                    mWeaponAmmo = 10;
                    mFireRate = 10;
                    mWeaponDamage = 10;
                    bUsesAmmo = false;
                    mAudioSource.clip = mEnergySound;
                    break;
            }
        }
    }
}
