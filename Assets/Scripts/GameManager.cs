using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Managers/GameManager")]
public class GameManager : MonoBehaviour
{
    [SerializeField] protected List<SpawnerComponent> m_spawnerComponents;
    [HideInInspector] public PlayerController m_playerController { get; private set; }
    [HideInInspector] public bool m_isPlayerAlive { get; private set; }

    public Leaderboard mLeaderBoard;

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        ObjectPoolingSystem.FlushPools();
    }

    public void PlayerHasSpawned(PlayerController playerController)
    {
        m_isPlayerAlive = true;
        m_playerController = playerController;
    }

    public void PlayerHasDied(int mScore)
    {
        mLeaderBoard.gameObject.SetActive(true);
        mLeaderBoard.UpdateList(mScore);
        m_isPlayerAlive = false;
    }

    public void GameStart()
    {
        foreach (var spawner in m_spawnerComponents)
        {
            spawner.SetGameManager(this);
            spawner.enabled = (true);
        }
    }
}
