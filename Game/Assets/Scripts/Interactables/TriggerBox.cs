using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TriggerBox : MonoBehaviour, IInteractive
{
    private bool isActive = false;
    private Rigidbody body = null;
    private PlayerController controller = null;

    private Vector3 startPos;

    void Start() {
        // Used to respawn box if falling out of world
        startPos = transform.position;
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

    // Implementation of IInteractive
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

    // Implementation of IInteractive
    // Return a different string depending on active status
    public string Info() {
        if (isActive) {
            return "Drop box";
        }
        else {
            return "Pickup box";
        }
    }

    // Respawns box if it falls out of world
    public void Reset() {
        transform.position = startPos;
    }

    public bool IsActive {
        get { return isActive; }
    }
}
