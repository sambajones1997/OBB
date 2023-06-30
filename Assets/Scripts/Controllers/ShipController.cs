using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Controllers/ShipController")]

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ExplosionComponent))]
public class ShipController : MonoBehaviour
{
    [Header("Ship Controller")]
    [SerializeField] protected GameObject m_shipRoot; // Root of the ship that will move forward and will be kept in relation to world x,y,z -- if this rotates, the player rotates
    [SerializeField] protected GameObject m_shipBase; // Base of the ship that will be used to rotate and shoot at the enemies. If this rotates, it doesnt affect ship movement
    public GameObject m_lastCauseOfDamage;
    protected Health m_healthComponent;
    protected WeaponController m_weaponController;
    protected Rigidbody m_rigidBody;
    protected GameManager m_gameManager;

    protected bool m_isAlive = true;
    protected bool m_bDestroyOnDeath = false;
    [SerializeField] protected float m_shipRotationSpeed = 90.0f;
    void Start()
    {
        m_weaponController = GetComponentInChildren<WeaponController>();
        m_healthComponent = GetComponent<Health>();
        m_rigidBody = GetComponentInChildren<Rigidbody>();
        OnShipStart();
        if (!m_weaponController)
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
        // we'll disable components we no-longer want active rather than destroying for two reasons
        // Destroying is more likely more expensive, uncoupling it from the game object - simply disabling it will work well for us
        // Destroying a component is not done on the same-frame. 
        // 3. We'll be destroying the game object soon anyway so might as well skip the whole de-activation thing.
        GetComponent<Collider>().enabled = false;
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
        foreach (var thruster in GetComponentsInChildren<Thruster>())
        {
            thruster.enabled = false;
        }

        m_isAlive = false;

        if (m_bDestroyOnDeath)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnShipStart() { }
    protected virtual void OnShipFixedUpdate() { }
    protected virtual void OnShipUpdate() { }
    protected virtual void RotateShip() { }
    protected virtual void OnShipDie() { }

    public bool IsAlive() { return m_isAlive; }

    public void TakenDamage(GameObject causeOfDamage = null) { m_lastCauseOfDamage = causeOfDamage; }

    public void SetGameManager(GameManager gameManager)
    {
        m_gameManager = gameManager;
    }
}
