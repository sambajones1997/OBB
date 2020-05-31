using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuInput : MonoBehaviour
{
    bool bFadeUI = false;
    float mFadeTimer = 1.0f;
    AsyncOperation mAsyncOperation;
    CanvasGroup mCanvasGroup;

    private void Start()
    {
        mCanvasGroup = GetComponent<CanvasGroup>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (bFadeUI)
        {
            mFadeTimer -= Time.fixedDeltaTime;
            mCanvasGroup.alpha = mFadeTimer;
            if (mFadeTimer <= 0.0f)
            {
                bFadeUI = false;
                mAsyncOperation.allowSceneActivation = true;
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            mAsyncOperation = SceneManager.LoadSceneAsync(1);
            mAsyncOperation.allowSceneActivation = false;
            bFadeUI = true;
        }
    }
}
