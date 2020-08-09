using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    public Text mScoreText;
    public Text mHealthText;
    public Text m_ArtilleryZoneText;
    // DEBUG VARIABLES
    public Text mFPSText;
    public Text mAvgFPSText;
    int mFPSIndexes = 0;
    List<int> mFPSList = new List<int>();

    float m_ArtilleryZoneTimer = 0.5f;
    bool m_playerOutOfBounds = false;
    private void Start()
    {
        mScoreText.text = "0";
        mFPSList.Add(((int)(1.0f / Time.deltaTime)));
    }

    private void Update()
    {
       if (mFPSList.Count >= 1000)
       {
           mFPSList[mFPSIndexes] = ((int)(1.0f / Time.deltaTime));
           mFPSIndexes++;
           if (mFPSIndexes >= 1000) mFPSIndexes = 0;
       }
       else
       {
           mFPSList.Add(((int)(1.0f / Time.deltaTime)));
       }
       mFPSText.text = mFPSList[mFPSList.Count - 1].ToString();
    }

    private void FixedUpdate()
    {
        int totalFPS = 0;
        for (int i = 0; i < mFPSList.Count; i++) totalFPS += mFPSList[i];
        mAvgFPSText.text = "Avg: " + (totalFPS / mFPSList.Count);

        if (m_playerOutOfBounds)
        {
            m_ArtilleryZoneTimer -= Time.fixedDeltaTime;
            if (m_ArtilleryZoneTimer <= 0.0f)
            {
                m_ArtilleryZoneText.enabled = !m_ArtilleryZoneText.enabled;
                m_ArtilleryZoneTimer = 0.5f;
            }
        }
    }

    public void UpdateScore(int score)
    {
        mScoreText.text = score.ToString();
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        mHealthText.text = health + "/" +  maxHealth;
    }

    public void PlayerOutOfBounds(bool val)
    {
        m_playerOutOfBounds = val;
        m_ArtilleryZoneTimer = 0.5f;
        m_ArtilleryZoneText.enabled = val;
    }
}
