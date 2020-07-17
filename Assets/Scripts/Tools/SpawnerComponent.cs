using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{

    [SerializeField] List<GameObject> m_entitiesToSpawn = new List<GameObject>();
    [SerializeField] List<GameObject> m_entitySpawnLocations = new List<GameObject>();
    [SerializeField] List<SpawnedEntity> m_entitiesSpawned = new List<SpawnedEntity>();
    [Tooltip("The number of entities spawned on the frame the spawner is activated")]
    [SerializeField] int m_initialSpawned = 0;
    [Tooltip("The total number of entities that can be active at any one time")]
    [SerializeField] int m_maxActiveNumber = 0;
    [Tooltip("The total number of entities to spawn from this spawner -- spawner will deactivate on completion unless resetSpawner is ticked")]
    [SerializeField] int m_totalToSpawn = 0;
    [SerializeField] float m_minTimeBetweenSpawn = 1.0f;
    [SerializeField] float m_maxTimeBetweenSpawn = 1.0f;
    [Tooltip("If true, all entities currently spawned using the spawner wwill die when the spawner is de-activated")]
    [SerializeField] bool m_bResetSpawner = false;
    protected int m_entityObjectIndex = 0;
    protected int m_currentActiveAmount = 0;
    protected int m_totalSpawned = 0;
    protected int m_entitySpawnIndex = 0;
    protected int m_locationSpawnIndex = 0;

    float m_timeBetweenSpawn = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (m_entitySpawnLocations.Count <= 0)
            m_entitySpawnLocations.Add(gameObject);

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
        m_timeBetweenSpawn -= Time.fixedDeltaTime;
        if (m_timeBetweenSpawn <= 0.0f && m_currentActiveAmount < m_maxActiveNumber && (m_totalSpawned < m_totalToSpawn || m_totalToSpawn <= 0))
            SpawnEntity();
    }

    protected void SpawnEntity()
    {
        SpawnedEntity spawn = null;
        spawn = Instantiate(m_entitiesToSpawn[m_entityObjectIndex], m_entitySpawnLocations[m_locationSpawnIndex].transform).AddComponent<SpawnedEntity>();
        spawn.SetSpawnerAndIndex(this, m_totalSpawned);

        m_entitiesSpawned.Add(spawn);
        ++m_locationSpawnIndex;
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
            this.enabled = false;// Disable spawner after all spawning is complete to avoid updates

        if (m_minTimeBetweenSpawn > m_maxTimeBetweenSpawn)
            m_minTimeBetweenSpawn = m_maxTimeBetweenSpawn;
        m_timeBetweenSpawn = Random.Range(m_minTimeBetweenSpawn, m_maxTimeBetweenSpawn);
    }

    public void EntityDestroyed()
    {
        --m_currentActiveAmount;
    }
}
