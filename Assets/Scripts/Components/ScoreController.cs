using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] protected int m_scoreToGive = 100;
    List<ScoreVoxelComponent> m_scoreVoxels;
    [SerializeField] bool m_activate;

    public void SetupScoreController()
    {
        transform.parent = null;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        m_scoreVoxels = new List<ScoreVoxelComponent>(GetComponentsInChildren<ScoreVoxelComponent>(true));
        PlayerController m_playerController = FindObjectOfType<PlayerController>();

        if (m_scoreVoxels.Count > 0)
        {
            int scorePerVoxel = m_scoreToGive / m_scoreVoxels.Count;
            int remaining = m_scoreToGive - (scorePerVoxel * m_scoreVoxels.Count);

            foreach (var score in m_scoreVoxels)
            {
                score.m_score = scorePerVoxel;
                score.m_playerController = m_playerController;
                score.gameObject.SetActive(true);
            }
            m_scoreVoxels[0].m_score += remaining; // if we have some left over e.g. 100 / 99 would leave 1 remaining in floating so add that to first one
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0.0f, 45.0f * Time.fixedDeltaTime, 0.0f));
    }

}
