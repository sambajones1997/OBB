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
    [SerializeField] protected float m_playerSpeed = 20.0f;
    protected float m_KillStreakTimer = 0.0f;
    protected float m_TimeSinceLastKill = 0.0f;
    protected int mScore = 0;
    protected int m_CurrentKills = 0;
    protected int m_CurrentKillStreak = 0;

    protected Vector3 m_movementVector;

    protected override void OnShipStart()
    {
        if (!mGameUI)
            mGameUI = FindObjectOfType<GameUI>();

        if (!mGameManager)
            mGameManager = FindObjectOfType<GameManager>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mGameUI.UpdateHealth(mHealthComponent.mHealth, mHealthComponent.mMaxHealth);
        mGameUI.UpdateScore(mScore);

        m_movementVector = Vector3.forward;
    }
    // Update is called once per frame
    protected override void OnShipUpdate ()
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

    private void FixedUpdate()
    {
        if (bIsAlive)
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
        m_rigidBody.MovePosition(m_rigidBody.position + (m_movementVector * m_playerSpeed * Time.deltaTime));


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
        mGameManager.SendMessage("PlayerHasDied", mScore);
        mGameUI.gameObject.SetActive(false);
        FindObjectOfType<GameManager>().bIsPlayerAlive = false;
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
