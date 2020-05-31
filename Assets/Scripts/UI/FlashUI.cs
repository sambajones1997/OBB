using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashUI : MonoBehaviour
{
    public Text mText;
    public float mTextFlash=1.0f;
    public bool bIsVisible = true;

    private void FixedUpdate()
    {
        mTextFlash -= Time.fixedDeltaTime;
        if (mTextFlash <= 0.0f)
        {
            if (bIsVisible)
            {
                mTextFlash = 1.0f;
                bIsVisible = false;
                mText.enabled = false;
            }
            else
            {
                mTextFlash = 1.0f;
                bIsVisible = true;
                mText.enabled = true;
            }
        }
    }

}
