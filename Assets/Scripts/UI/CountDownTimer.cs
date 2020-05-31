using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public GameObject mPlayerShip;
    public float mCountDownTimer = 5.0f; // How long the game should count in for
    public Text mCountDownText;
    public Canvas mGameCanvas;

    private void Start()
    {
        mPlayerShip.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        mCountDownTimer -= Time.fixedDeltaTime;
        if (mCountDownTimer < 0.0f) mCountDownText.text = "GO";
        else mCountDownText.text = ((int)(mCountDownTimer + 1.0f)).ToString();

        if (mCountDownTimer <= -1.0f)
        {
            FindObjectOfType<GameManager>().bIsPlayerAlive = true;
            mPlayerShip.SetActive(true);
            mGameCanvas.gameObject.SetActive(true);
            Destroy(gameObject);
        }
	}
}
