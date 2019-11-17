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
        if (!hasGun) {
            gun.gameObject.SetActive(false);
        }
        controllable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.IsPaused && controllable) {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            player.HorizontalValue = horizontal;

            player.VerticalValue = vertical;

            if (Input.GetButtonDown("Jump"))
            {
                player.Jump();
            }

            if (mouseX != 0.0f)
            {
                player.RotateY = mouseX;
            }

            if (mouseY != 0.0f)
            {
                player.RotateX = mouseY;
            }

            if (hasGun) {
                if (Input.GetButtonDown("Fire1"))
                {
                    gun.Shoot();
                }

                if (Input.GetButtonDown("Fire2"))
                {
                    Vector3 targetPosition = new Vector3();
                    if (gun.GetTeleportTarget(out targetPosition))
                    {
                        player.Teleport(targetPosition);
                    }
                }
            }

            if (Input.GetButtonDown("Activate")) {
                player.Activate();
            }
        }
    }

    public void GetGun() {
        hasGun = true;
        gun.gameObject.SetActive(true);
    }

    public bool IsControllable {
        set { controllable = value; }
    }

    public bool HasGun {
        get { return hasGun; }
        set { hasGun = value; }
    }
}
