﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private TeleGun gun = null;
    private PlayerController player = null;

    void Start() {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
