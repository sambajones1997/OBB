using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Tools/SpawnerComponent")]
public class SpawnerComponent : MonoBehaviour
{
    protected enum ESpawnerType {Locations, Area};
    [SerializeField] protected ESpawnerType m_spawnerType;
    [SerializeField] List<GameObject> m_entitiesToSpawn = new List<GameObject>();
    [SerializeField] List<GameObject> m_entitySpawnLocations = new List<GameObject>();
    [SerializeField] List<SpawnedEntity> m_entitiesSpawned = new List<SpawnedEntity>();
    [SerializeField] BoxCollider m_spawnArea;
    [Tooltip("The number of entities spawned on the frame the spawner is activated")]
    [SerializeField] int m_initialSpawned = 0;
    [Tooltip("The total number of entities that can be active at any one time")]
    [SerializeField] int m_maxActiveNumber = 0;
    [Tooltip("The total number of entities to spawn from this spawner -- spawner will deactivate on completion unless resetSpawner is ticked")]
    [SerializeField] int m_totalToSpawn = 0;
    [SerializeField] float m_minTimeBetweenSpawn = 1.0f;
    [SerializeField] float m_maxTimeBetweenSpawn = 1.0f;
    [Tooltip("If true, all entities currently spawned using the spawner will die when the spawner is de-activated")]
    [SerializeField] bool m_bResetSpawner = false;
    [Tooltip("If true, when using spawn area then we can randomise y spawning in the bounds if wanted. else we use the y location of the transform of this game object")]
    [SerializeField] bool m_randomiseSpawnY = false;
    protected int m_entityObjectIndex = 0;
    protected int m_currentActiveAmount = 0;
    protected int m_totalSpawned = 0;
    protected int m_entitySpawnIndex = 0;
    protected int m_locationSpawnIndex = 0;

    GameManager m_gameManager;
    float m_timeBetweenSpawn = 0.0f;
    // Start is called before the first frame update

    void Start()
    {
        switch (m_spawnerType)
        {
            case ESpawnerType.Locations:
                if (m_entitySpawnLocations.Count <= 0)
                    m_entitySpawnLocations.Add(gameObject);
                break;
            case ESpawnerType.Area:
                if (!m_spawnArea && !(m_spawnArea = GetComponentInChildren<BoxCollider>())) // if there is no spawn area and one is not found in the children
                {
                    m_spawnArea = gameObject.AddComponent<BoxCollider>(); // add a basic spawn area
                    m_spawnArea.gameObject.layer = 14;
                }
                m_spawnArea.isTrigger = true;
                break;
        }


        if (m_initialSpawned > 0)
        {
            for (int i = 0; i < m_initialSpawned; ++i)
            {
                SpawnEntity();
            }
        }

        if (m_minTimeBetweenSpawn > m_maxTimeBetweenSpawn)
            m_minTimeBetweenSpawn = m_maxTimeBetweenSpawn;

        m_timeBetweenSpawn = Random.Range(m_minTimeBetweenSpawn, m_maxTimeBetweenSpawn);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_currentActiveAmount < m_maxActiveNumber) 
            m_timeBetweenSpawn -= Time.fixedDeltaTime;
        if (m_timeBetweenSpawn <= 0.0f && m_currentActiveAmount < m_maxActiveNumber && (m_totalSpawned < m_totalToSpawn || m_totalToSpawn <= 0))
            SpawnEntity();
    }

    protected void SpawnEntity()
    {
        {
            SpawnedEntity spawn = null;
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = new Quaternion();
            switch (m_spawnerType)
            {
                case ESpawnerType.Locations:
                    spawnPos = m_entitySpawnLocations[m_locationSpawnIndex].transform.position;
                    spawnRot = m_entitySpawnLocations[m_locationSpawnIndex].transform.rotation;
                    break;
                case ESpawnerType.Area:

                    float y = transform.position.y;
                    if (m_randomiseSpawnY)
                    {
                        y = transform.position.y + (Random.Range(-m_spawnArea.bounds.extents.y, m_spawnArea.bounds.extents.y));
                    }
                    spawnPos = new Vector3(transform.position.x + (Random.Range(-m_spawnArea.bounds.extents.x, m_spawnArea.bounds.extents.x)), y, transform.position.z + (Random.Range(-m_spawnArea.bounds.extents.z, m_spawnArea.bounds.extents.z)));
                    spawnRot = new Quaternion();
                    break;
            }
            spawn = Instantiate(m_entitiesToSpawn[m_entityObjectIndex], spawnPos, spawnRot, transform).AddComponent<SpawnedEntity>();
            spawn.SetSpawnerAndIndex(this, m_entitiesSpawned.Count);
            spawn.gameObject.SendMessage("SetGameManager", m_gameManager, SendMessageOptions.DontRequireReceiver);
            m_entitiesSpawned.Add(spawn);
            ++m_locationSpawnIndex;
        }

        if (m_locationSpawnIndex >= m_entitySpawnLocations.Count)
            m_locationSpawnIndex = 0;

        // go through each prototype spawning each different entity once before rolling back around
        ++m_entityObjectIndex;
        if (m_entityObjectIndex >= m_entitiesToSpawn.Count)
            m_entityObjectIndex = 0;

        ++m_currentActiveAmount;
        ++m_totalSpawned;
        if (m_entitiesToSpawn.Count > 0)
        {
            ++m_entitySpawnIndex;
            if (m_entitySpawnIndex >= m_entitiesToSpawn.Count)
                m_entitySpawnIndex = 0;
        }

        if (m_totalToSpawn > 0 && m_totalSpawned >= m_totalToSpawn)
            enabled = false;// Disable spawner after all spawning is complete to avoid updates

        if (m_minTimeBetweenSpawn > m_maxTimeBetweenSpawn)
            m_minTimeBetweenSpawn = m_maxTimeBetweenSpawn;
        m_timeBetweenSpawn = Random.Range(m_minTimeBetweenSpawn, m_maxTimeBetweenSpawn);
    }

    public void EntityDestroyed(int index)
    {
        m_entitiesSpawned.RemoveAt(index);
        --m_currentActiveAmount;
    }

    public void SetGameManager(GameManager gameManager)
    {
        m_gameManager = gameManager;
    }
}
