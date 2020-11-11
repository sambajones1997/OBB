using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class AIShipController : ShipController
{
    [Header("AI Controller")]
    public GameManager.EEnemyType mEnemyType;
    public float mShipShootAngle = 15.0f;
    public PlayerController mPlayerShip;
    protected float m_playerReaction = 0.2f;
    [SerializeField] protected float m_maxPlayerReaction = 0.2f; // time it takes to spot player and shoot at them
    [SerializeField] protected float m_maxShipTurnTimer = 0.05f;
    protected float m_shipTurnTimer = 0.05f;
    MomentumMovement m_momentumMovement;

    // Start is called before the first frame update
    protected override void OnShipStart()
    {
        base.OnShipStart(); 
        if (FindObjectOfType<PlayerController>()) mPlayerShip = FindObjectOfType<PlayerController>();
        m_momentumMovement = GetComponentInChildren<MomentumMovement>();
    }

    protected override void OnShipFixedUpdate()
    {
        base.OnShipFixedUpdate();
        if (m_isAlive)
        {
            m_momentumMovement.AddInput(transform.forward);
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
        }
    }

    protected override void RotateShip()
    {
        Vector3 relativePos = mPlayerShip.gameObject.transform.position - transform.position;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, relativePos.normalized, Mathf.Deg2Rad * (mShipRotationSpeed * Time.fixedDeltaTime), 0.0f);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    protected void CheckAngleOfPlayer()
    {
        Vector3 relativePos = mPlayerShip.gameObject.transform.position - transform.position;
        float angle = Vector3.Angle(relativePos, transform.forward);
        if (angle <= mShipShootAngle && angle >= -mShipShootAngle)
        {
            m_playerReaction -= Time.fixedDeltaTime;
            if (m_playerReaction <= 0.0f)
                mWeaponController.FireWeapon();
        }
        else
            m_playerReaction = m_maxPlayerReaction;
    }

    protected override void OnShipDie()
    {
        base.OnShipDie();
        if (mLastCauseOfDamage.transform.root == mPlayerShip.transform.root)
        {
            BroadcastMessage("SetupScoreController", SendMessageOptions.DontRequireReceiver);
        }
        FindObjectOfType<GameManager>().DestroyedShip(mEnemyType);
        if (GetComponent<SpawnLoot>() && Random.Range(0, 10) >= 9) GetComponent<SpawnLoot>().DropLoot();
        Destroy(gameObject, 1.0f);
    }
}
