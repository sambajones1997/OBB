using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ExplosionComponent))]
public class ShipController : MonoBehaviour
{
    [Header("Ship Controller")]
    [SerializeField] protected GameObject m_shipRoot; // Root of the ship that will move forward and will be kept in relation to world x,y,z -- if this rotates, the player rotates
    [SerializeField] protected GameObject m_shipBase; // Base of the ship that will be used to rotate and shoot at the enemies. If this rotates, it doesnt affect ship movement
    public GameObject mLastCauseOfDamage;
    protected Health mHealthComponent;
    protected WeaponController mWeaponController;
    protected Rigidbody m_rigidBody;

    protected bool m_isAlive = true;
    [SerializeField] protected float mShipRotationSpeed = 90.0f;
    void Start()
    {
        mWeaponController = GetComponentInChildren<WeaponController>();
        mHealthComponent = GetComponent<Health>();
        m_rigidBody = GetComponentInChildren<Rigidbody>();
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
        if (!m_isAlive)
            return;

        OnShipDie();
        gameObject.SendMessage("Explode", SendMessageOptions.DontRequireReceiver);
        GetComponent<Collider>().enabled = false;
        foreach (var v in GetComponentsInChildren<Renderer>())
        {
            Destroy(v);
        }
        m_isAlive = false;
        //enabled = false;
    }

    protected virtual void OnShipStart() { }
    protected virtual void OnShipFixedUpdate() { }
    protected virtual void OnShipUpdate() { }
    protected virtual void RotateShip() { }
    protected virtual void OnShipDie() { }

    public bool IsAlive() { return m_isAlive; }
}
