using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade3DText : MonoBehaviour
{
    Color mColor;
    float mFadeTimer = 1.0f;
    float mTextSpeed = 5.0f;

	// Use this for initialization
	void Start () {
        mColor = GetComponent<TextMesh>().color;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.Translate(new Vector3(0.0f, 0.0f, 1.0f * mTextSpeed * Time.fixedDeltaTime), Space.World);
        // Colour
        mFadeTimer -= Time.fixedDeltaTime;
        mColor.a = mFadeTimer;
        GetComponent<TextMesh>().color = mColor;
        if (mFadeTimer <= 0.0f)
        {
            mFadeTimer = 0.0f;
            Destroy(gameObject);
        }
	}
}
