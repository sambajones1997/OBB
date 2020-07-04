using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class ChargeDrone_Movement : MonoBehaviour
{
    public PlayerController mPlayerShip;
    public GameObject mLastCauseOfDamage;
    protected Health m_healthComponent;
    bool bIsAlive = true;
    bool m_isCharging = false;
    protected float m_playerReaction = 0.2f;
    [SerializeField] protected float m_maxPlayerReaction = 0.2f; // time it takes to spot player and shoot at them
    [SerializeField] protected float m_maxShipTurnTimer = 0.05f;
    [SerializeField] protected float m_chargeSpeed = 100.0f;
    protected float m_shipTurnTimer = 0.05f;
    public float mShipRotationSpeed = 360.0f;
    public float mShipShootAngle = 15.0f;

    private void Start()
    {
        mPlayerShip = FindObjectOfType<PlayerController>();
        m_healthComponent = GetComponent<Health>();
    }

    private void FixedUpdate()
    {
        if (bIsAlive)
        {
            if (mPlayerShip)
            {
                if (!m_isCharging)
                {
                    m_shipTurnTimer -= Time.fixedDeltaTime;
                    if (m_shipTurnTimer <= 0.0f)
                    {
                        RotateShip();
                        m_shipTurnTimer = m_maxShipTurnTimer;
                    }
                    CheckAngleOfPlayer();
                }
                else
                {
                    ChargeDrone();
                }
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

    void ChargeDrone()
    {
        transform.Translate(Vector3.forward * m_chargeSpeed * Time.fixedDeltaTime);
        Destroy(gameObject, 10.0f);
    }

    void CheckAngleOfPlayer()
    {
        Vector3 relativePos = mPlayerShip.gameObject.transform.position - transform.position;
        float angle = Vector3.Angle(relativePos, transform.forward);
        if (angle <= mShipShootAngle && angle >= -mShipShootAngle)
        {
            m_playerReaction -= Time.fixedDeltaTime;
            if (m_playerReaction <= 0.0f)
            {
                m_isCharging = true;
            }
        }
        else
            m_playerReaction = m_maxPlayerReaction;
    }

    public void DestroyShip(bool addScore = true, bool GiveWeaponToPlayer = true)
    {
        if (addScore) GetComponent<GiveScore>().AddPlayerScore();
        foreach (var v in GetComponentsInChildren<Renderer>())
        {
            Destroy(v);
        }
        GetComponent<ExplosionComponent>().Explode();
        GetComponent<BoxCollider>().enabled = false;
        bIsAlive = false;
        FindObjectOfType<GameManager>();
        if (GetComponent<SpawnLoot>() && Random.Range(0, 10) >= 9) GetComponent<SpawnLoot>().DropLoot();
        Destroy(gameObject, 1.0f);
    }
}
