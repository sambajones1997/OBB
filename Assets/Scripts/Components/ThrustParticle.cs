using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustParticle : MonoBehaviour
{
    float mParticleLifeSpan = 0.5f;

    private void FixedUpdate()
    {
        transform.localScale = new Vector3((2*mParticleLifeSpan),(2*mParticleLifeSpan),(2*mParticleLifeSpan));
        mParticleLifeSpan -= Time.fixedDeltaTime;
        if (mParticleLifeSpan <= 0.0f) Destroy(gameObject);
    }
}
