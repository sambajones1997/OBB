using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWeaponController))]
public class PlayerController : ShipController
{
    [Header("Player Controller")]
    public GameUI mGameUI;
    public GameManager mGameManager;
    [SerializeField] protected GameObject m_directionArrow;
    [SerializeField] protected float mKillStreakTimeOut = 5.0f;
    protected float m_KillStreakTimer = 0.0f;
    protected float m_TimeSinceLastKill = 0.0f;
    protected int mScore = 0;
    protected int m_CurrentKills = 0;
    protected int m_CurrentKillStreak = 0;

    protected Vector3 m_movementVector;
    protected MomentumMovement m_momentumMovement;

    protected float m_playerDeathTimer = -1.0f;
    protected override void OnShipStart()
    {
        if (!mGameUI)
            mGameUI = FindObjectOfType<GameUI>();

        if (!mGameManager)
            mGameManager = FindObjectOfType<GameManager>();

        if (!m_momentumMovement)
            m_momentumMovement = GetComponentInChildren<MomentumMovement>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mGameUI.UpdateHealth(mHealthComponent.mHealth, mHealthComponent.mMaxHealth);
        mGameUI.UpdateScore(mScore);

        m_movementVector = Vector3.forward;
    }
    // Update is called once per frame
    protected override void OnShipUpdate ()
    {
        if (m_isAlive)
        {
            mWeaponController.FireWeapon();
            m_TimeSinceLastKill += Time.deltaTime;
            if (m_TimeSinceLastKill >= mKillStreakTimeOut)
            {
                m_TimeSinceLastKill = 0.0f;
                m_KillStreakTimer = 0.0f;
                m_CurrentKillStreak = 0;
            }
            GameInput();
        }
        else
        {
            if (m_playerDeathTimer > 0.0f)
            {
                m_playerDeathTimer -= Time.deltaTime;
                if (m_playerDeathTimer <= 0.0f)
                {
                    mGameManager.SendMessage("PlayerHasDied", mScore);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_isAlive)
        {
            mGameUI.UpdateHealth(mHealthComponent.mHealth, mHealthComponent.mMaxHealth);
        }
    }

    void GameInput()
    {
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        moveVector.Normalize();
        if (moveVector.sqrMagnitude > 0.0f)
        {
            m_movementVector = moveVector;
            m_directionArrow.transform.rotation = Quaternion.Euler(0.0f, Vector3.SignedAngle(Vector3.forward, m_movementVector, Vector3.up), 0.0f);
        }
        m_momentumMovement.AddInput(m_movementVector);


         Vector3 shootVector = new Vector3(Input.GetAxisRaw("ShootHorizontal"), 0.0f, Input.GetAxisRaw("ShootVertical"));
        if (shootVector.sqrMagnitude > 0.0f)
            m_shipBase.transform.localRotation = Quaternion.Euler(0.0f, Vector3.SignedAngle(Vector3.forward, shootVector, Vector3.up), 0.0f);
    }

    public void AddScore(ScorePackage scorePackage)
    {
        int currentThousand = mScore / 1000;
        mScore += scorePackage.mScore;
        if (currentThousand < (mScore / 1000)) mHealthComponent.AddHealth(25);
        mGameUI.UpdateScore(mScore);

        if (scorePackage.mPlayerKill)
        {
            ++m_CurrentKills;
            ++m_CurrentKillStreak;
            KillStreakCalculator();
        }
    }

    protected override void OnShipDie()
    {
        m_playerDeathTimer = 1.0f;
        mGameUI.gameObject.SetActive(false);
    }

    protected void KillStreakCalculator()
    {
        if (m_CurrentKillStreak > 0)
        {
            m_KillStreakTimer += m_TimeSinceLastKill;
            if (m_CurrentKillStreak >= 10 && m_CurrentKillStreak <= 20)
            {
                // boom double fire rate
            }
        } // else we start the timer at 0.0f
    }

    // Kill player straight away
    protected void OutOfBounds(bool isOutOfBounds)
    {
        if (isOutOfBounds)
            mHealthComponent.TakeDamage(new DamagePackage(1000, null));
    }
}
