using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public GameObject mThrustParticle;

    private float mParticleSpawnTime = 0.25f;

    private void FixedUpdate()
    {
            if ((mParticleSpawnTime -= Time.fixedDeltaTime) <= 0.0f)
            {

                Instantiate(mThrustParticle, transform.position, new Quaternion());
                mParticleSpawnTime = 0.25f;
            }
    }
}
