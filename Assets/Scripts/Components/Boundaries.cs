using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour {

    Rigidbody mRigidBody;
    float mTopBoundary = 85.0f;
    float mRightBoundary = 135.0f;
    // Use this for initialization
    void Start ()
    {
        mRigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (mRigidBody.position.x <= -mRightBoundary)
        {
            mRigidBody.position = new Vector3(mRightBoundary, mRigidBody.position.y, mRigidBody.position.z);
        }
        else if (mRigidBody.position.x >= mRightBoundary)
        {
            mRigidBody.position = new Vector3(-mRightBoundary, mRigidBody.position.y, mRigidBody.position.z);
        }
        else if (mRigidBody.position.z >= mTopBoundary)
        {
            mRigidBody.position = new Vector3(mRigidBody.position.x, mRigidBody.position.y, -mTopBoundary);
        }
        else if (mRigidBody.position.z <= -mTopBoundary)
        {
            mRigidBody.position = new Vector3(mRigidBody.position.x, mRigidBody.position.y, mTopBoundary);
        }
    }
}
