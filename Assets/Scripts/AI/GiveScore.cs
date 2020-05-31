using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveScore : MonoBehaviour
{
    public int mScore;
    public GameObject mTextObject;

    public void AddPlayerScore()
    {
        if (FindObjectOfType<PlayerController>()) FindObjectOfType<PlayerController>().AddScore(mScore);
        TextMesh tm = Instantiate(mTextObject, GetComponentInParent<Rigidbody>().position, new Quaternion(), null).GetComponentInChildren<TextMesh>();

        tm.text = mScore.ToString();
    }
}
