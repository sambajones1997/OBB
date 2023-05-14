using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem
{
    public string Name;
    public List<MonoBehaviour> PoolObjects = new List<MonoBehaviour>();
}

public static class ObjectPoolingSystem
{
    // Start is called before the first frame update

    static List<PoolSystem> Pools = new List<PoolSystem>();

    public static bool FindPoolSystem(string poolName, out PoolSystem poolSystem)
    {
        foreach (PoolSystem pool in Pools)
        {
            if (pool.Name == poolName)
            {
                poolSystem = pool;
                return true;
            }
        }
        poolSystem = new PoolSystem();
        return false;
    }

    public static PoolSystem FindCreatePool(string poolName)
    {
        PoolSystem poolSystem = new PoolSystem();

        if (FindPoolSystem(poolName, out poolSystem))
        {
            return poolSystem;
        }

        poolSystem.Name = poolName;
        Pools.Add(poolSystem);
        return Pools[Pools.Count - 1];
    }
}
