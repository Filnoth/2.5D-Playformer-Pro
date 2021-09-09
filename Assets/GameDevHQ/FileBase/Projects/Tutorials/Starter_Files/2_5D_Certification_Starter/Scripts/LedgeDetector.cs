using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    [SerializeField]
    private Vector3 _handPOS, _standPOS;
    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("LedgeGrabChecker"))
        {
            var player = other.transform.parent.GetComponent<Player>();

            if (player != null)
            {
                player.LedgeGrab(_handPOS, this);
            }
        }

    }

    public Vector3 GetStandPos()
    {
        return _standPOS;
    }
}
