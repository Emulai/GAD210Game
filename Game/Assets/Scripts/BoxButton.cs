using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxButton : MonoBehaviour
{
    [SerializeField]
    private GameObject door = null;

    [Header("Debug")]
    [SerializeField]
    private bool debug = false;
    private Animator anim = null;

    void Start() {
        anim = door.GetComponent<Animator>();
    }

    void Update() {
        if (debug) {
            Debug.DrawLine(transform.position, door.gameObject.transform.position, Color.green);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "TriggerBox") {
            
            anim.SetTrigger("Open");

        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "TriggerBox") {
            
            anim.SetTrigger("Close");

        }
    }
}
