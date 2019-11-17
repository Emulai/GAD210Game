using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    // Kill player or reset triggerbox position when contacted
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            // Kill Player
            other.gameObject.GetComponent<PlayerController>().DamageHealth(100.0f);
        }
        else if (other.gameObject.tag == "TriggerBox") { 
            // Reset to starting point
            other.gameObject.GetComponent<TriggerBox>().Reset();
        }
    }
}
