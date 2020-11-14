using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] private GameManager m_gameManager;
    public float mCountDownTimer = 5.0f; // How long the game should count in for
    public Text mCountDownText;
    public Canvas mGameCanvas;

    // Update is called once per frame

    private void Start()
    {
       if (!m_gameManager)
        {
            m_gameManager = FindObjectOfType<GameManager>();
        }
    }

    void FixedUpdate ()
    {
        mCountDownTimer -= Time.fixedDeltaTime;
        if (mCountDownTimer < 0.0f) mCountDownText.text = "GO";
        else mCountDownText.text = ((int)(mCountDownTimer + 1.0f)).ToString();

        if (mCountDownTimer <= -1.0f)
        {
            mGameCanvas.gameObject.SetActive(true);
            m_gameManager.SendMessage("GameStart");
            Destroy(gameObject);
        }
	}
}
