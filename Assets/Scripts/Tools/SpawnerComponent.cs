using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{
    [SerializeField] List<GameObject> m_entitiesToSpawn = new List<GameObject>();
    [SerializeField] List<GameObject> m_entitySpawnLocations = new List<GameObject>();
    [SerializeField] List<SpawnedEntity> m_entitiesSpawned = new List<SpawnedEntity>();
    [SerializeField] int m_initialSpawned = 0;
    [SerializeField] int m_maxActiveNumber = 0;
    [SerializeField] int m_totalToSpawn = 0;
    protected int m_currentActiveAmount = 0;
    protected int m_totalSpawned = 0;
    protected int m_entitySpawnIndex = 0;
    protected int m_locationSpawnIndex = 0;

    [SerializeField] float m_maxTimeBetweenSpawn = 1.0f;
    float m_timeBetweenSpawn = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (m_entitySpawnLocations.Count <= 0)
            m_locationSpawnIndex = -1;
        if (m_initialSpawned > 0)
        {
            for (int i = 0; i < m_initialSpawned; ++i)
            {
                SpawnEntity();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_timeBetweenSpawn -= Time.fixedDeltaTime;
        if (m_timeBetweenSpawn <= 0.0f && m_currentActiveAmount < m_maxActiveNumber && m_totalSpawned < m_totalToSpawn)
            SpawnEntity();
    }

    protected void SpawnEntity()
    {
        SpawnedEntity spawn = null;
        if (m_locationSpawnIndex <= -1)
            spawn = Instantiate(m_entitiesToSpawn[0], transform).AddComponent<SpawnedEntity>();
        else
            spawn = Instantiate(m_entitiesToSpawn[0], m_entitySpawnLocations[m_locationSpawnIndex].transform).AddComponent<SpawnedEntity>();
        spawn.SetSpawnerAndIndex(this, m_totalSpawned);

        m_entitiesSpawned.Add(spawn);
        ++m_locationSpawnIndex;
        if (m_locationSpawnIndex >= m_entitySpawnLocations.Count)
            m_locationSpawnIndex = 0;

        ++m_totalSpawned;
        m_timeBetweenSpawn = m_maxTimeBetweenSpawn;
        if (m_entitiesToSpawn.Count > 0)
        {
            ++m_entitySpawnIndex;
            if (m_entitySpawnIndex >= m_entitiesToSpawn.Count)
                m_entitySpawnIndex = 0;
        }

        if (m_totalSpawned >= m_totalToSpawn)
            this.enabled = false;// Disable spawner after all spawning is complete to avoid updates
    }

    public void EntityDestroyed()
    {
        m_currentActiveAmount--;
    }
}
