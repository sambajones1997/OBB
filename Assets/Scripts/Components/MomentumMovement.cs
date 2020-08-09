using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumMovement : MonoBehaviour
{
    Rigidbody mRigidBody;
    public Vector3 mMomentumVector;
    public float mShipSpeed = 10.0f;
    [SerializeField] protected float mMomentumDrag = 0.75f; // obviously there's no drag in space but it helps to control the ship
    private void Start()
    {
        mMomentumVector = Vector3.forward;
        mRigidBody = GetComponentInParent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        AddInput(Vector3.forward);
        mRigidBody.MovePosition(mRigidBody.position + (mMomentumVector * mShipSpeed * Time.fixedDeltaTime));
    }

    public void AddInput(Vector3 momentum)
    {
        mMomentumVector *= 0.75f;
        Vector3 newMomentum = mMomentumVector + (transform.forward * Time.fixedDeltaTime);
        newMomentum.y = 0.0f;
        mMomentumVector = newMomentum.normalized;
    }
}
