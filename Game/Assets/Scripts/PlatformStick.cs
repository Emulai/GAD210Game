using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStick : MonoBehaviour
{
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player" || col.tag == "Projectile") {
            col.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "Player" || col.tag == "Projectile") {
            col.transform.parent = null;
        }
    }
}
