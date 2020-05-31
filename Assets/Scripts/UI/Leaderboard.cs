using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public List<Text> mScores;
    public Text mInstructionText;
    public RawImage mHoldTimerBar;
    float mButtonHold = 2.5f;
    float mDelay = 0.25f;
    bool bButtonIsHeld = false;

    private void Start()
    {
        for (int i =0; i < 5; i++) mScores[i].text = ScoreClass.mHighScores[i].ToString();
    }

    public void UpdateList(int newScore)
    {
        ScoreClass.Load();
        for (int i = 0; i < 5; i++)
        {
           if (ScoreClass.mHighScores[i] < newScore )
            {
                mScores[i].text = newScore.ToString();
                PushLeaderBoard(i);
                ScoreClass.mHighScores[i] = newScore;
                ScoreClass.SaveScores();
                mScores[i].gameObject.AddComponent<FlashUI>();
                mScores[i].gameObject.GetComponent<FlashUI>().mText = mScores[i];
                for (int j = 0; j < 5; j++) mScores[j].text = ScoreClass.mHighScores[j].ToString(); // Update new list
                break;
            }
        }
    }

    public void PushLeaderBoard(int index)
    {
        int T = 0;
        T = ScoreClass.mHighScores[index];
        if (index < 4)
        {
            int temp = ScoreClass.mHighScores[index];
            ScoreClass.mHighScores[index] = T;
            T = temp;
            index++;
        }  
    }

    private void Update()
    {
        if (mDelay > 0.0f) mDelay -= Time.deltaTime;
        else
        {
            if (Input.GetButtonUp("Fire1") && bButtonIsHeld)
            {
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    mInstructionText.text = "Hold Button To Quit";
                    bButtonIsHeld = true;
                }

                if (bButtonIsHeld)
                {
                    mButtonHold -= Time.deltaTime;

                    mHoldTimerBar.transform.localScale = new Vector3(1.0f - (mButtonHold / 2.5f), mHoldTimerBar.transform.localScale.y, mHoldTimerBar.transform.localScale.z);
                    if (mHoldTimerBar.transform.localScale.x > 1.0f) mHoldTimerBar.transform.localScale = new Vector3(1.0f, mHoldTimerBar.transform.localScale.y, mHoldTimerBar.transform.localScale.z);
                    if (mButtonHold <= 0.0f) Application.Quit();
                }
            }
        }
    }
}
