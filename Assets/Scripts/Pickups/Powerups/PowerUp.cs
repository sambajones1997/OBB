using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pickup
{
    [Tooltip("Name Displayed on UI When Picked Up")]
    [SerializeField] string m_powerUpName;

    [Tooltip("Maximum Duration of Pickup")]
    [SerializeField] float m_powerUpDuration; 

    protected override void OnItemPickUp(PlayerController player)
    {
        OnPowerUpPickUp(player);
    }

    protected virtual void OnPowerUpPickUp(PlayerController player) { }
}
