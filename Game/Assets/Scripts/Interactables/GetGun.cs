using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGun : MonoBehaviour, IInteractive
{
    // Implementation of IInteractive
    public void Activate(PlayerController activator) {
        activator.gameObject.GetComponent<PlayerInput>().GetGun();
        Destroy(transform.gameObject);
    }

    // Implementation of IInteractive
    public string Info() {
        return "Pickup Teleport Gun";
    }
}
