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
    public PlayerController mPlayerShip;
    public GameObject mLastCauseOfDamage;
    protected Health m_healthComponent;
    bool bIsAlive = true;
    protected Weapon m_weapon;

    protected float m_playerReaction = 0.2f;
    [SerializeField] protected float m_maxPlayerReaction = 0.2f; // time it takes to spot player and shoot at them
    [SerializeField] protected float m_maxShipTurnTimer = 0.05f;
    protected float m_shipTurnTimer = 0.05f;

    private void Start()
    {
       if (FindObjectOfType<PlayerController>()) mPlayerShip = FindObjectOfType<PlayerController>();
        m_weapon = GetComponentInChildren<Weapon>();
        if (!m_weapon) DestroyShip(false);
        m_healthComponent = GetComponent<Health>();
    }

    private void FixedUpdate()
    {
        if (bIsAlive)
        {
            if (mPlayerShip)
            {
                m_shipTurnTimer -= Time.fixedDeltaTime;
                if (m_shipTurnTimer <= 0.05f)
                {
                    RotateShip();
                    m_shipTurnTimer = m_maxShipTurnTimer;
                }
                CheckAngleOfPlayer();
            }

            if (m_healthComponent.GetHealth() <= 0) DestroyShip();
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
            m_playerReaction -= Time.fixedDeltaTime;
            if (m_playerReaction <= 0.0f)
                m_weapon.Fire();
        }
        else
            m_playerReaction = m_maxPlayerReaction;
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
