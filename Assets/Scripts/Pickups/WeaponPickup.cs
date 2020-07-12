using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] protected GameObject mWeapon;

    protected virtual void OnItemPickUp(PlayerController player)
    {
        
    }
}
