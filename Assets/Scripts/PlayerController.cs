using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWeaponController))]
public class PlayerController : ShipController
{
    public GameUI mGameUI;
    public GameManager mGameManager;
    [SerializeField] protected float mKillStreakTimeOut = 5.0f;
    Rigidbody mRigidBody;
    int mScore = 0;
    bool m_playerIsOutOfBounds = false;
    int mCurrentKills = 0;
    int mCurrentKillStreak = 0;
    float mKillStreakTimer = 0.0f;
    float mTimeSinceLastKill = 0.0f;
    float m_outOfBoundsTimer = 3.0f;
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
        mRigidBody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    protected override void OnShipUpdate ()
    {
        mWeaponController.FireWeapon();
        mTimeSinceLastKill += Time.deltaTime;
        if (mTimeSinceLastKill >= mKillStreakTimeOut)
        {
            mTimeSinceLastKill = 0.0f;
            mKillStreakTimer = 0.0f;
            mCurrentKillStreak = 0;
        }
        GameInput();
    }

    private void FixedUpdate()
    {
        if (bIsAlive)
        {
            mGameUI.UpdateHealth(mHealthComponent.mHealth, mHealthComponent.mMaxHealth);
            if (m_playerIsOutOfBounds)
            {
                m_outOfBoundsTimer -= Time.fixedDeltaTime;
                if (m_outOfBoundsTimer <= 0.0f)
                {
                    mHealthComponent.TakeDamage(new DamagePackage(1000, null));
                }
            }
        }
    }

    void GameInput()
    {
        if (Input.GetButton("Fire1"))
        {
            mRigidBody.transform.Rotate(Vector3.up * Time.deltaTime * -mShipRotationSpeed);
        }

        if (Input.GetButton("Fire2"))
        {
            mRigidBody.transform.Rotate(Vector3.up * Time.deltaTime * mShipRotationSpeed);
        }
    }

    public void AddScore(ScorePackage scorePackage)
    {
        int currentThousand = mScore / 1000;
        mScore += scorePackage.mScore;
        if (currentThousand < (mScore / 1000)) mHealthComponent.AddHealth(25);
        mGameUI.UpdateScore(mScore);

        if (scorePackage.mPlayerKill)
        {
            ++mCurrentKills;
            ++mCurrentKillStreak;
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
        if (mCurrentKillStreak > 0)
        {
            mKillStreakTimer += mTimeSinceLastKill;
            if (mCurrentKillStreak >= 10 && mCurrentKillStreak <= 20)
            {
                // boom double fire rate
            }
        } // else we start the timer at 0.0f
    }

    protected void OutOfBounds(bool outOfBounds)
    {
        m_playerIsOutOfBounds = outOfBounds;
        mGameUI.SendMessage("PlayerOutOfBounds", m_playerIsOutOfBounds);
        if (!outOfBounds)
            m_outOfBoundsTimer = 3.0f;
    }
}
