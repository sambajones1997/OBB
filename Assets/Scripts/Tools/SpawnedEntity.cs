using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedEntity : MonoBehaviour
{
    protected SpawnerComponent m_spawner;
    protected int m_spawnerIndex = 0;

    public void SetSpawnerAndIndex(SpawnerComponent spawner, int spawnIndex)
    {
        m_spawner = spawner;
        m_spawnerIndex = spawnIndex;
    }

    public void SetSpawnIndex(int spawnIndex)
    {
        m_spawnerIndex = spawnIndex;
    }

    public void SetSpawner(SpawnerComponent spawner)
    {
        m_spawner = spawner;
    }
}
