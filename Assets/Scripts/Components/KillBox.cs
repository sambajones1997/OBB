using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    [SerializeField] protected bool m_invert = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            if (m_invert)
                other.gameObject.SendMessage("OutOfBounds", false, SendMessageOptions.DontRequireReceiver);
            else
                other.gameObject.SendMessage("OutOfBounds", true, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            if (m_invert)
                other.gameObject.SendMessage("OutOfBounds", true, SendMessageOptions.DontRequireReceiver);
            else
                other.gameObject.SendMessage("OutOfBounds", false, SendMessageOptions.DontRequireReceiver);
        }
    }
}
