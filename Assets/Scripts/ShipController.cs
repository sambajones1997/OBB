using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MomentumMovement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ExplosionComponent))]
public class ShipController : MonoBehaviour
{
    public GameObject mLastCauseOfDamage;
    protected Health mHealthComponent;
    protected WeaponController mWeaponController;

    protected bool bIsAlive = true;
    [SerializeField] protected float mShipRotationSpeed = 90.0f;
    void Start()
    {
        mWeaponController = GetComponentInChildren<WeaponController>();
        mHealthComponent = GetComponent<Health>();
        OnShipStart();
        if (!mWeaponController)
            Die();
    }

    private void Update()
    {
        OnShipUpdate();
    }

    private void FixedUpdate()
    {
        OnShipFixedUpdate();
    }

    protected void Die()
    {
        if (!bIsAlive)
            return;

        OnShipDie();
        gameObject.SendMessage("Explode", SendMessageOptions.DontRequireReceiver);
        GetComponent<Collider>().enabled = false;
        foreach (var v in GetComponentsInChildren<Renderer>())
        {
            Destroy(v);
        }
        bIsAlive = false;
        Destroy(gameObject, 1.0f);
        enabled = false;
    }

    protected virtual void OnShipStart() { }
    protected virtual void OnShipFixedUpdate() { }
    protected virtual void OnShipUpdate() { }
    protected virtual void RotateShip() { }
    protected virtual void OnShipDie() { }
}
