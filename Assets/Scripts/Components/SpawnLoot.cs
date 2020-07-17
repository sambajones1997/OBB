using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    [SerializeField] public GameObject m_lootItem;
    [SerializeField] [Range(0.0f,1.0f)] public float m_lootChance;
}

public class SpawnLoot : MonoBehaviour
{
    [SerializeField] public List<Loot> mLootObjects;

    public void DropLoot()
    {
       // Instantiate(mLootObjects[Random.Range(0, mLootObjects.Count)], gameObject.transform.position, new Quaternion());
    }
}
