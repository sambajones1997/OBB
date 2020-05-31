using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineAI : MonoBehaviour
{
    float mHomingTimer = 1.0f;

    GameObject mHomingTarget;
    bool bIsAlive = true;

    public float m_maxDist = 40.0f;
    protected float mMaxDistSquared = 10.0f;
    public float mRotationSpeed = 90.0f;

    public int mDamage = 50;

    private void Start()
    {
        mMaxDistSquared = m_maxDist * m_maxDist;
        transform.Rotate(new Vector3(0.0f, Random.Range(10.0f, 350.0f), 0.0f));
    }

    private void FixedUpdate()
    {
        if (bIsAlive)
        {
            if (mHomingTarget) FaceHomingTarget();
            if (GetComponent<Health>().mHealth <= 0) DestroyMine();
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

    public void DestroyMine()
    {
        GetComponent<GiveScore>().AddPlayerScore();
        if (GetComponent<ExplosionComponent>()) GetComponent<ExplosionComponent>().Explode();
        foreach (var v in GetComponentsInChildren<Renderer>())
        {
            Destroy(v);
        }
        bIsAlive = false;
        FindObjectOfType<GameManager>().DestroyedShip(GameManager.EEnemyType.Mine);
        Destroy(gameObject, 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bIsAlive)
        {
            if (other.gameObject.GetComponent<Health>())
            {
                other.gameObject.GetComponent<Health>().Damage(mDamage);
                DestroyMine();
            }
        }
    }
}
