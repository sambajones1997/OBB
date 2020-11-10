using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreVoxelComponent : MonoBehaviour
{
    [HideInInspector] public PlayerController m_playerObject;
    [HideInInspector] public int m_score = 25;
    private float m_scoreSpeed = 0.0f;
    private float m_maxScoreSpeed = 100.0f;
    private float m_maxAcceleration = 25.0f;
    private float m_idleTime = 0.0f;
    private bool m_scoreGiven = false;

    // Start is called before the first frame update
    private void Start()
    {
        m_scoreSpeed = Random.Range(0.0f, 10.0f);
        m_idleTime = Random.Range(0.0f, 2.5f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_idleTime > 0.0f)
        {
            m_idleTime -= Time.fixedDeltaTime;
            if (m_idleTime <= 0.0f)
                transform.parent = null;
        }
        else
        {
            if (!m_playerObject)
                Destroy(gameObject);
            m_scoreSpeed += Time.fixedDeltaTime * Random.Range(1.0f, m_maxAcceleration);
            Mathf.Clamp(m_score, 0.0f, m_maxScoreSpeed);
            transform.LookAt(m_playerObject.transform);
            transform.Translate(Vector3.forward * m_scoreSpeed * Time.fixedDeltaTime);

            if ((m_playerObject.transform.position - transform.position).sqrMagnitude <= 1.0f)
            {
                if (!m_scoreGiven)
                {
                    ScorePackage scorePackage = new ScorePackage(m_score, true);
                    m_playerObject.AddScore(scorePackage);
                    m_scoreGiven = true;
                    Destroy(gameObject);
                }
            }
        }
    }

}
