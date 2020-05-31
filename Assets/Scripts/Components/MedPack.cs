using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedPack : MonoBehaviour
{
    public float mRotationSpeedY = 45.0f;
    public float mRotationSpeedZ = 45.0f;

    private void Start()
    {
        mRotationSpeedY = Random.Range(0.0f, mRotationSpeedY);
        mRotationSpeedZ = Random.Range(0.0f, mRotationSpeedZ);
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0.0f, mRotationSpeedY, mRotationSpeedZ) * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Health>().AddHealth(50);
            Destroy(gameObject);
        }
    }
}
