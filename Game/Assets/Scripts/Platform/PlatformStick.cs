using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStick : MonoBehaviour
{
    // Make player, projectile & triggerbox stick
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player" || col.tag == "Projectile") {
            col.transform.parent = transform;
        }
        else if (col.tag == "TriggerBox") {
            // If not being carried by player
            if (!col.gameObject.GetComponent<TriggerBox>().IsActive) {
                col.transform.parent = transform;
            }
        }
    }

    // Make player, projectile & triggerbox not stick
    void OnTriggerExit(Collider col) {
        if (col.tag == "Player" || col.tag == "Projectile") {
            col.transform.parent = null;
        }
        else if (col.tag == "TriggerBox") {
            // If not being carried by player
            if (!col.gameObject.GetComponent<TriggerBox>().IsActive) {
                col.transform.parent = null;
            }
        }
    }
}
