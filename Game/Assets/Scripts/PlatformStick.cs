using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStick : MonoBehaviour
{
    void OnTriggerEnter(Collider col) {
        Debug.Log("Trigger");
        if (col.tag == "Player" || col.tag == "Projectile") {
            col.transform.parent = transform;
            // col.transform.SetParent(transform, false);
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "Player" || col.tag == "Projectile") {
            col.transform.parent = null;
            // col.transform.SetParent(null, false);
        }
    }
}
