using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    public List<GameObject> mLootObjects;

    public void DropLoot()
    {
        Instantiate(mLootObjects[Random.Range(0, mLootObjects.Count)], gameObject.transform.position, new Quaternion());
    }
}
