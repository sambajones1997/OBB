using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] protected float mRotationSpeedY = 45.0f;
    [SerializeField] protected float mRotationSpeedZ = 45.0f;

    protected void Start()
    {
        OnPickupStart();
    }

    protected void Update()
    {
        OnPickUpUpdate();
    }

    protected void FixedUpdate() 
    {
        transform.Rotate(new Vector3(0.0f, mRotationSpeedY, mRotationSpeedZ) * Time.fixedDeltaTime);
        OnPickUpFixedUpdate();
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {        
            OnItemPickUp(other.gameObject.GetComponent<PlayerController>());
        }
    }

    protected virtual void OnPickupStart() { }
    protected virtual void OnPickUpUpdate() { }
    protected virtual void OnPickUpFixedUpdate() { }
    protected virtual void OnItemPickUp(PlayerController player) { }
}
