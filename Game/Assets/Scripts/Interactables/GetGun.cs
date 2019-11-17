using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGun : MonoBehaviour, IInteractive
{
    public void Activate(PlayerController activator) {
        activator.gameObject.GetComponent<PlayerInput>().GetGun();
        Destroy(transform.gameObject);
    }

    public string Info() {
        return "Pickup Teleport Gun";
    }
}
