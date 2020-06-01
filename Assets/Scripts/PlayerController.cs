using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum EKeyPressType { None, Hold,Tap, TapHold};
    public EKeyPressType mKeyPressType;
    public GameUI mGameUI;
    public Leaderboard mLeaderboard;
    Health mHealth;
    Rigidbody mRigidBody;
    float mKeyPressDuration = 0.1f;
    float mTurnRate = 90.0f;
    int mScore = 0;

    protected Weapon m_weapon;

    bool bIsHeld = false;
    bool bIsAlive = true;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Second);
        mHealth = GetComponent<Health>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mGameUI.UpdateHealth(mHealth.mHealth, mHealth.mMaxHealth);
        mGameUI.UpdateScore(mScore);
        mRigidBody = GetComponent<Rigidbody>();
        m_weapon = GetComponentInChildren<Weapon>();
    }
    // Update is called once per frame
    void Update ()
    {
        GameInput();
    }

    private void FixedUpdate()
    {
        if (mHealth.GetHealth() <= 0 && bIsAlive) DestroyPlayer();
        else mGameUI.UpdateHealth(mHealth.mHealth, mHealth.mMaxHealth);
    }

    void GameInput()
    {
        if (mKeyPressType != EKeyPressType.None && mKeyPressDuration > 0.0f) mKeyPressDuration -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
            bIsHeld = true;
            if (mKeyPressType == EKeyPressType.Tap)
            {
                mKeyPressType = EKeyPressType.TapHold;
            }
            else mKeyPressType = EKeyPressType.Hold;
            mKeyPressDuration = 0.2f;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            bIsHeld = false;
            if (mKeyPressDuration > 0.0f && mKeyPressType == EKeyPressType.Hold)
            {
                mKeyPressType = EKeyPressType.Tap;
                mKeyPressDuration = 0.2f;
            }
            else
            {
                mKeyPressType = EKeyPressType.None;
                mKeyPressDuration = 0.0f;
            }
        }

        switch (mKeyPressType)
        {
            case EKeyPressType.Hold:
                if (mKeyPressDuration <= 0.0f)
                {
                    mRigidBody.transform.Rotate(Vector3.up * Time.deltaTime * -mTurnRate);
                }
                break;
            case EKeyPressType.Tap:
                m_weapon.Fire();
                mKeyPressType = EKeyPressType.None;
                break;
        }

        if (Input.GetButton("Fire2"))
        {
            mRigidBody.transform.Rotate(Vector3.up * Time.deltaTime * mTurnRate);
        }

        if (!bIsHeld && mKeyPressType != EKeyPressType.None && mKeyPressDuration <= 0.0f)
        {
            mKeyPressType = EKeyPressType.None;
            mKeyPressDuration = 0.1f;
        }
    }

    public void AddScore(int score)
    {
        int currentThousand = mScore / 1000;
        mScore += score;
        if (currentThousand < (mScore / 1000)) mHealth.AddHealth(25);
        mGameUI.UpdateScore(mScore);
    }

    void DestroyPlayer()
    {
        mLeaderboard.gameObject.SetActive(true);
        mGameUI.gameObject.SetActive(false);
        mLeaderboard.UpdateList(mScore);
        GetComponent<ExplosionComponent>().Explode();
        foreach (var v in GetComponentsInChildren<Renderer>())
        {
            Destroy(v);
        }
        FindObjectOfType<GameManager>().bIsPlayerAlive = false;
        bIsAlive = false;
        Destroy(gameObject, 1.0f);
        Destroy(this);
    }

    public void ChangeWeaponType(Weapon.EWeaponType newWeaponType)
    {
        m_weapon.ChangeWeaponType(newWeaponType);
        switch (m_weapon.mWeaponType)
        {
            case Weapon.EWeaponType.Blaster:
                m_weapon.mFireRate = 10;
                break;
            case Weapon.EWeaponType.Shotgun:
                m_weapon.mFireRate = 5;
                break;
            case Weapon.EWeaponType.Rockets:
                m_weapon.mFireRate = 1;
                break;
        }
    }
}
