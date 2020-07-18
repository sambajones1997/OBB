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
        float randChance = Random.Range(0, 1.0f); // Get a number between 0 and 1, we'll test this against spawnable items.
        int randObject = Random.Range(0, mLootObjects.Count);
        if (1.0f - mLootObjects[randObject].m_lootChance <= randChance) // a 0.7 chance means 1 - 0.7 = 0.3. So any value above 0.3 will spawn the object
        {
            Instantiate(mLootObjects[randObject].m_lootItem, gameObject.transform.position, new Quaternion());
        }
    }
}
