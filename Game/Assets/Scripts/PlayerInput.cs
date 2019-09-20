using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{

    private PlayerController _player = null;

    void Start() {
        _player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _player.HorizontalValue = horizontal;

        _player.VerticalValue = vertical;

        if (Input.GetButtonDown("Jump")) {
            _player.Jump();
        }

        if (mouseX != 0.0f) {
            _player.RotateY = mouseX;
        }

        if (mouseY != 0.0f) {
            _player.RotateX = mouseY;
        }
    }
}
