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

    public static GameObject GetGameObjectFromPool(PoolSystem poolHandle, bool bRemoveObjectFromPool = true)
    {
        if (poolHandle.Name.Length > 0 && poolHandle.PoolObjects.Count > 0)
        {
            GameObject obj = poolHandle.PoolObjects[0].gameObject;
            if (bRemoveObjectFromPool)
            {
                poolHandle.PoolObjects.RemoveAt(0);
            }
            return obj;
        }
        return null;
    }

    public static T GetObjectFromPool<T>(PoolSystem poolHandle, bool bRemoveObjectFromPool = true)
    {
        if (poolHandle.Name.Length > 0 && poolHandle.PoolObjects.Count > 0)
        {
            T comp = poolHandle.PoolObjects[0].gameObject.GetComponent<T>();
            if (comp != null && bRemoveObjectFromPool)
            {
                poolHandle.PoolObjects.RemoveAt(0);
            }
            return comp;
        }
        return default(T);
    }

    public static string GetPoolName(string objectName)
    {
        return objectName + "_Pool";
    }

    public static void FlushPools()
    {
        foreach(PoolSystem pool in Pools)
        {
            pool.PoolObjects.Clear();
        }

        Pools.Clear();
    }
}
