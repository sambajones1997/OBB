using UnityEngine;
public class MedPack : Pickup
{
   [SerializeField] protected int mHealthRegeneration = 50;

    protected override void OnPickupStart()
    {
        mRotationSpeedY = Random.Range(0.0f, mRotationSpeedY);
        mRotationSpeedZ = Random.Range(0.0f, mRotationSpeedZ);
    }

    protected override void OnPickUpFixedUpdate()
    {

    }

    protected override void OnItemPickUp(PlayerController player) 
    {
        player.SendMessage("AddHealth", mHealthRegeneration);
    }
}
