using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    protected enum EAIState {Idle, Shooting, Turning}; // shooting also involves turning
    protected EAIState m_currentAIState; 
    public GameManager.EEnemyType mEnemyType;
    public float mShipRotationSpeed = 90.0f;
    public float mShipShootAngle = 15.0f;
    protected float m_aiUpdateTimer = 0.3f; // avg human reaction is 0.25 - 0.35 seconds (0.3 on avg)
    public float m_maxUpdateTimer = 0.3f;
    public PlayerController mPlayerShip;
    public GameObject mLastCauseOfDamage;
    protected Health m_healthComponent;
    bool bIsAlive = true;
    protected Weapon m_weapon;

    private void Start()
    {
       if (FindObjectOfType<PlayerController>()) mPlayerShip = FindObjectOfType<PlayerController>();
        m_weapon = GetComponentInChildren<Weapon>();
        if (!m_weapon) DestroyShip(false);
        m_healthComponent = GetComponent<Health>();
    }

    private void FixedUpdate()
    {
        m_aiUpdateTimer -= Time.fixedDeltaTime;
        if (bIsAlive && m_aiUpdateTimer <= 0.0f)
        {
            if (mPlayerShip)
            {
                CheckAngleOfPlayer();
            }

            if (m_healthComponent.GetHealth() <= 0) DestroyShip();
            m_aiUpdateTimer = m_maxUpdateTimer;
        }
        ProcessState();
    }

    private void ProcessState()
    {
        switch (m_currentAIState)
        {
            case EAIState.Turning:
                RotateShip();
                break;
            case EAIState.Shooting:
                FireWeapon();
                break;
        }
    }

    public void RotateShip()
    {
        Vector3 relativePos = mPlayerShip.gameObject.transform.position - transform.position;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, relativePos.normalized, Mathf.Deg2Rad * (mShipRotationSpeed * Time.fixedDeltaTime), 0.0f);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void CheckAngleOfPlayer()
    {
        Vector3 relativePos = mPlayerShip.gameObject.transform.position - transform.position;
        float angle = Vector3.Angle(relativePos, transform.forward);
        if (angle <= mShipShootAngle && angle >= -mShipShootAngle)
        {
            m_currentAIState = EAIState.Shooting;
        }
        else
            m_currentAIState = EAIState.Turning;
    }

    void FireWeapon()
    {
        m_weapon.Fire();
    }

    public void DestroyShip(bool addScore = true, bool GiveWeaponToPlayer = true)
    {
        if (addScore) GetComponent<GiveScore>().AddPlayerScore();
        if (mLastCauseOfDamage && mLastCauseOfDamage.tag == "Player" && GiveWeaponToPlayer && m_weapon.mWeaponType != Weapon.EWeaponType.Blaster)
        {
            mPlayerShip.GetComponent<Weapon>().ChangeWeaponType(m_weapon.mWeaponType);
        }
        foreach (var v in GetComponentsInChildren<Renderer>())
        {
            Destroy(v);
        }
        GetComponent<ExplosionComponent>().Explode();
        GetComponent<BoxCollider>().enabled = false;
        bIsAlive = false;
        FindObjectOfType<GameManager>().DestroyedShip(mEnemyType);
        if (GetComponent<SpawnLoot>() && Random.Range(0, 10) >= 9) GetComponent<SpawnLoot>().DropLoot();
        Destroy(gameObject, 1.0f);
    }

}
