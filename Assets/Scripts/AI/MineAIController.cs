using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Controllers/MineAIController")]

public class MineAIController : ShipController
{
    float mHomingTimer = 1.0f;

    GameObject mHomingTarget;

    public float m_maxDist = 40.0f;
    protected float mMaxDistSquared = 10.0f;
    public float mRotationSpeed = 90.0f;

    public int mDamage = 50;

    protected override void OnShipStart()
    {
        mMaxDistSquared = m_maxDist * m_maxDist;
        transform.Rotate(new Vector3(0.0f, Random.Range(10.0f, 350.0f), 0.0f));
    }

    protected override void OnShipFixedUpdate()
    {
        base.OnShipFixedUpdate();
        if (m_isAlive)
        {
            if (mHomingTarget) FaceHomingTarget();
            if ((mHomingTimer -= Time.fixedDeltaTime) <= 0.0f)
            {
                CheckForNearbyShips();
                mHomingTimer = 1.0f;
            }
        }
    }

    private void CheckForNearbyShips()
    {
        foreach (var v in FindObjectsOfType<Weapon>())
        {
            if (Vector3.SqrMagnitude(v.gameObject.transform.position - transform.position) <= mMaxDistSquared)
            {
                mHomingTarget = v.gameObject;
                break;
            }
        }
    }

    void FaceHomingTarget()
    {
        transform.LookAt(mHomingTarget.transform);
    }

    protected override void OnShipDie()
    {
        GetComponent<GiveScore>().AddPlayerScore();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_isAlive)
        {
            DamagePackage damagePackage = new DamagePackage(mDamage, gameObject);
            gameObject.SendMessage("TakeDamage", damagePackage, SendMessageOptions.DontRequireReceiver);
            Die();
        }
    }
}
