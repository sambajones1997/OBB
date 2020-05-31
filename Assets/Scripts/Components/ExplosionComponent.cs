using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class ExplosionComponent : MonoBehaviour
{
    public GameObject mExplosionParticleSystem;
    public AudioClip mExplosionAudioClip;

	public void Explode()
    {
        Instantiate(mExplosionParticleSystem, transform.position, new Quaternion());
        GetComponentInParent<AudioSource>().clip = mExplosionAudioClip;
        GetComponentInParent<AudioSource>().Play();
        Destroy(this);
    }
}
