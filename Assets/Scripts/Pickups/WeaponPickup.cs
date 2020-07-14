using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    [SerializeField] protected GameObject mWeapon;

    protected override void OnItemPickUp(PlayerController player)
    {
        player.transform.root.SendMessage("PickUpNewWeapon", mWeapon);
        Destroy(gameObject);
    }
}
