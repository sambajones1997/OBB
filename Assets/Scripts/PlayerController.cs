using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ShipController
{
    public GameUI mGameUI;
    public Leaderboard mLeaderboard;

    Rigidbody mRigidBody;
    int mScore = 0;

    protected override void OnShipStart()
    {
        Random.InitState(System.DateTime.Now.Second);
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
        GameInput();
    }

    private void FixedUpdate()
    {
        if (bIsAlive) mGameUI.UpdateHealth(mHealthComponent.mHealth, mHealthComponent.mMaxHealth);
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

    public void AddScore(int score)
    {
        int currentThousand = mScore / 1000;
        mScore += score;
        if (currentThousand < (mScore / 1000)) mHealthComponent.AddHealth(25);
        mGameUI.UpdateScore(mScore);
    }

    public void OnShipDie()
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
}
