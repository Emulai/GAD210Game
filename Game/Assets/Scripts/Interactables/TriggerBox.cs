using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TriggerBox : MonoBehaviour, IInteractive
{
    private bool isActive = false;
    private Rigidbody body = null;
    private PlayerController controller = null;

    void Start() {
        body = GetComponent<Rigidbody>();
    }

    void Update() {
        // While active, ensure this object is the player's only active object
        if (isActive) {
            controller.VisibleObject = this;
        }
        // Else set the player's active object null, and remove link to player
        else {
            if (controller != null) {
                controller.VisibleObject = null;
            }
            controller = null;
        }
    }

    // Box activation simply allows the player to pick up and move the box
    public void Activate(PlayerController activator) {
        isActive = !isActive;

        controller = activator;

        if (isActive) {
            body.isKinematic = true;
            transform.SetParent(Camera.main.gameObject.transform);
        }
        else {
            body.isKinematic = false;
            transform.SetParent(null);
        }
    }

    // Return a different string depending on active status
    public string Info() {
        if (isActive) {
            return "Drop box";
        }
        else {
            return "Pickup box";
        }
    }
}
