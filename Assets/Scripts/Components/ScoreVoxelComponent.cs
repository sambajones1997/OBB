using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreVoxelComponent : MonoBehaviour
{
    [HideInInspector] public GameManager m_gameManager;
    private PlayerController m_playerController;
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
        m_playerController = m_gameManager.m_playerController;
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
            if (!m_gameManager.m_isPlayerAlive)
            {
                GiveScore();
            }
            else
            {
                m_scoreSpeed += Time.fixedDeltaTime * Random.Range(1.0f, m_maxAcceleration);
                Mathf.Clamp(m_score, 0.0f, m_maxScoreSpeed);
                transform.LookAt(m_playerController.transform);
                transform.Translate(Vector3.forward * m_scoreSpeed * Time.fixedDeltaTime);

                if ((m_playerController.transform.position - transform.position).sqrMagnitude <= 50.0f)
                {
                    if (!m_scoreGiven)
                    {
                        GiveScore();
                    }
                }
            }   
        }
    }

    void GiveScore()
    {
        ScorePackage scorePackage = new ScorePackage(m_score, true);
        m_playerController.AddScore(scorePackage);
        m_scoreGiven = true;
        Destroy(gameObject);
    }

}
