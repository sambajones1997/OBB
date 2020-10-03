using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreVoxelComponent : MonoBehaviour
{
    [SerializeField] protected int m_scoreToGive = 25;
    [SerializeField] protected float m_scoreSpeed = 5.0f;
    protected PlayerController m_playerObject;

    // Start is called before the first frame update
    void Start()
    {
        m_playerObject = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementVector = (m_playerObject.transform.position - transform.position).normalized;
        transform.Translate(movementVector * m_scoreSpeed * Time.fixedDeltaTime);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_playerObject)
        {
            ScorePackage scorePackage = new ScorePackage(m_scoreToGive, true);
            m_playerObject.AddScore(scorePackage);
            Destroy(gameObject);
        }
    }
}
