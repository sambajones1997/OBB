using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Managers/GameManager")]
public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool m_isPlayerAlive = false;

    public Leaderboard mLeaderBoard;

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    public void PlayerHasDied(int mScore)
    {
        mLeaderBoard.gameObject.SetActive(true);
        mLeaderBoard.UpdateList(mScore);
        m_isPlayerAlive = false;
    }
}
