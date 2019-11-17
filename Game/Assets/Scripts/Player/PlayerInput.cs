using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private PauseManager manager = null;
    [SerializeField]
    private TeleGun gun = null;
    [SerializeField]
    public bool hasGun = true;
    private PlayerController player = null;
    private bool controllable = true;

    void Start() {
        player = GetComponent<PlayerController>();

        // If player doesn't have gun, turn it off
        if (!hasGun) {
            gun.gameObject.SetActive(false);
        }
        controllable = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Disable/enable input loop
        if (!manager.IsPaused && controllable) {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            player.HorizontalValue = horizontal;

            player.VerticalValue = vertical;

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                player.Jump();
            }

            // Rotate on X
            if (mouseX != 0.0f)
            {
                player.RotateY = mouseX;
            }

            // Rotate on Y
            if (mouseY != 0.0f)
            {
                player.RotateX = mouseY;
            }

            // If player has gun. allow use of gun
            if (hasGun) {

                // Shoot projectile
                if (Input.GetButtonDown("Fire1"))
                {
                    gun.Shoot();
                }

                // Teleport to projectile
                if (Input.GetButtonDown("Fire2"))
                {
                    Vector3 targetPosition = new Vector3();
                    if (gun.GetTeleportTarget(out targetPosition))
                    {
                        player.Teleport(targetPosition);
                    }
                }
            }

            // Activate IInteractive objects
            if (Input.GetButtonDown("Activate")) {
                player.Activate();
            }
        }
    }

    // Give player gun and make it visible
    public void GetGun() {
        hasGun = true;
        gun.gameObject.SetActive(true);
    }

    public bool IsControllable {
        set { controllable = value; }
    }

    // Used by SaveSystem
    public bool HasGun {
        get { return hasGun; }
        set { hasGun = value; }
    }
}
