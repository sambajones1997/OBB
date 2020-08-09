using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] protected float m_maxXDistance = 55.0f;
    [SerializeField] protected float m_maxYDistance = 10.0f;
    [SerializeField] protected float m_maxZDistance = 30.0f;
    [SerializeField] protected float m_cameraSpeed = 1.0f;
    [SerializeField] protected float m_minCamDistance = 10.0f;
    [SerializeField] protected SpawnerComponent m_playerSpawner;
    [SerializeField] protected GameManager m_gameManager;
    protected GameObject m_playerShip = null;
    // Start is called before the first frame update


    private void Update()
    {
        if (m_playerShip)
        {
            Vector3 vectorToPlayer = m_playerShip.transform.position - transform.position;
            vectorToPlayer.y = 0.0f;
            if (vectorToPlayer.sqrMagnitude > m_minCamDistance * m_minCamDistance)
                transform.Translate(vectorToPlayer * m_cameraSpeed * Time.deltaTime, Space.World);


        }
    }

    private void FixedUpdate()
    {
        if (!m_playerShip && m_gameManager.bIsPlayerAlive && m_playerSpawner.gameObject.activeSelf)
        {
            m_playerShip = FindObjectOfType<PlayerController>().gameObject;
        }
    }
}
