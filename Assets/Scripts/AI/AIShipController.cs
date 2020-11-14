using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Controllers/AIShipController")]
[RequireComponent(typeof(WeaponController))]
public class AIShipController : ShipController
{
    [Header("AI Controller")]
    public float mShipShootAngle = 15.0f;
    public PlayerController m_playerController;
    protected float m_playerReaction = 0.2f;
    [SerializeField] protected float m_maxPlayerReaction = 0.2f; // time it takes to spot player and shoot at them
    [SerializeField] protected float m_maxShipTurnTimer = 0.05f;
    protected float m_shipTurnTimer = 0.05f;
    MomentumMovement m_momentumMovement;
    protected float m_spawnActivationDelay = 0.5f;

    // Start is called before the first frame update
    protected override void OnShipStart()
    {
        base.OnShipStart();
        m_playerController = m_gameManager.m_playerController;
        m_momentumMovement = GetComponentInChildren<MomentumMovement>();
    }

    protected override void OnShipFixedUpdate()
    {
        base.OnShipFixedUpdate();
        if (m_isAlive)
        {
            if (m_spawnActivationDelay > 0.0f)
            {
                m_spawnActivationDelay -= Time.fixedDeltaTime;
                return;
            }
            m_momentumMovement.AddInput(transform.forward);
            if (m_playerController)
            {
                m_shipTurnTimer -= Time.fixedDeltaTime;
                if (m_shipTurnTimer <= 0.05f)
                {
                    RotateShip();
                    m_shipTurnTimer = m_maxShipTurnTimer;
                }
                CheckAngleOfPlayer();
            }
        }
    }

    protected override void RotateShip()
    {
        Vector3 relativePos = m_playerController.gameObject.transform.position - transform.position;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, relativePos.normalized, Mathf.Deg2Rad * (m_shipRotationSpeed * Time.fixedDeltaTime), 0.0f);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    protected void CheckAngleOfPlayer()
    {
        Vector3 relativePos = m_playerController.gameObject.transform.position - transform.position;
        float angle = Vector3.Angle(relativePos, transform.forward);
        if (angle <= mShipShootAngle && angle >= -mShipShootAngle)
        {
            m_playerReaction -= Time.fixedDeltaTime;
            if (m_playerReaction <= 0.0f)
                m_weaponController.FireWeapon();
        }
        else
            m_playerReaction = m_maxPlayerReaction;
    }

    protected override void OnShipDie()
    {
        base.OnShipDie();
        if (m_lastCauseOfDamage.transform.root == m_playerController.transform.root)
        {
            BroadcastMessage("SetupScoreController", m_gameManager, SendMessageOptions.DontRequireReceiver);
        }
        Destroy(gameObject, 1.0f);
    }
}
