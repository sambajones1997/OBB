using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Managers/PlanetSpawner")]
public class PlanetSpawner : MonoBehaviour
{
    public List<GameObject> mPlanets;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(mPlanets[Random.Range(0, mPlanets.Count - 1)], new Vector3(0.0f, -2.0f, 0.0f), Quaternion.Euler(90.0f, 0.0f, 0.0f));
        enabled = false;
    }
}
