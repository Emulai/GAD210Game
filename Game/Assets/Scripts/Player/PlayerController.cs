﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float jumpForce = 250.0f;
    [SerializeField]
    private LayerMask groundMask = (1 << 8) | (1 << 9);
    [SerializeField]
    private float maxHealth = 100.0f;
    [SerializeField]
    private GameObject healthVisualiser = null;

    [Header("World Interaction")]
    [SerializeField]
    private float actionDistance = 3.0f;
    [SerializeField]
    private LayerMask actionMask = (1 << 10); // Hits Interactive layer only
    [SerializeField]
    private RawImage actionIcon = null;
    [SerializeField]
    private TMP_Text actionText = null;

    [Header("Debug Variables")]
    [SerializeField]
    private bool displayMoveVector = false;
    [SerializeField]
    private bool displayActionRaycast = false;

    private Rigidbody rb = null;
    private float horizontalAxisValue = 0.0f;
    private float verticalAxisValue = 0.0f;
    private float xRotate = 0.0f;
    private float yRotate = 0.0f;
    private bool isGrounded = false;
    private bool onRamp = false;
    private bool isViewingAction = false;
    private IInteractive visibleObject = null;
    private float currentHealth = 100.0f;
    private Image healthImage = null;
    private Color healthColour = Color.red;
    private float timeSinceLastHit = 0.0f;
    private GameManager manager = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        actionText.text = "";
        healthImage = healthVisualiser.GetComponent<Image>();
        healthColour = healthImage.color;
        manager = FindObjectOfType<GameManager>();

        // Move to game controller
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // If can see an IInteractive object, display UI
        if (isViewingAction) {
            actionIcon.gameObject.SetActive(true);
            actionText.gameObject.SetActive(true);
        }
        // Else do not display UI
        else {
            actionIcon.gameObject.SetActive(false);
            actionText.text = "";
            actionText.gameObject.SetActive(false);
        }

        // Death condition
        if (currentHealth <= 0.0f) {
            manager.GameOver();
        }

        // Controls "health bar" red-screen indicator & healing
        if (currentHealth < 100.0f) {

            // If not damaged for a second, heal!
            if (timeSinceLastHit >= 1.0f) {
                currentHealth++;
                timeSinceLastHit = 1.0f;
            }

            // Determine percentage of health lost, convert to alpha on red screen-size UI panel
            float lostHealth = maxHealth - currentHealth;
            healthColour.a = (lostHealth / maxHealth);
            healthImage.color = healthColour;
        }
        else {
            healthColour.a = 0.0f;
            healthImage.color = healthColour;
        }

        timeSinceLastHit += Time.deltaTime;
    }

    void FixedUpdate() 
    {
        Vector3 fDirectional = new Vector3();

        // Interaction Check
        LookForInteractive();

        // Ground Check
        RaycastHit groundHit;
        Physics.Raycast(
            new Vector3(
                transform.position.x, 
                transform.position.y, 
                transform.position.z), 
            -transform.up, 
            out groundHit,
            1.1f, 
            groundMask);

        if (groundHit.collider != null) 
        {
            int layer = groundHit.collider.gameObject.layer;
            if (layer == 8 ||
                layer == 9) 
            {
                isGrounded = true;
                if (groundHit.collider.tag == "Ramp") 
                {
                    onRamp = true;
                }
                // Out-dated (though functional) platforn code. Comment "in" in Sandbox mode
                else if (groundHit.collider.tag == "Platform")
                {
                    // transform.SetParent(hit.collider.transform);
                }
                else 
                {
                    onRamp = false;
                    // transform.SetParent(null);
                }
            }
        }
        else 
        {
            isGrounded = false;
            onRamp = false;
        }

        // Calc forward and right vectors
        Vector3 fForward = transform.forward * verticalAxisValue;
        Vector3 fRight = transform.right * horizontalAxisValue;

        // Compensate for loss of speed on ramps -- ramps only in Sandbox mode
        if (onRamp && (verticalAxisValue != 0.0f || horizontalAxisValue != 0.0f)) 
        {
            Vector3 rampFactor = new Vector3(0, 2, 0);
            fForward += rampFactor;
            fRight += rampFactor;
        }

        // Calculate final directional vector, and visualise it when debugging
        fDirectional += fForward + fRight;
        if (displayMoveVector) 
        {
            Debug.DrawRay(transform.position, fDirectional, Color.blue);
        }

        // Apply to velocity after grabbing current y
        fDirectional.y = rb.velocity.y;
        rb.velocity = fDirectional;

        // Rotate player and camera
        transform.rotation = Quaternion.Euler(0, yRotate, 0);
        Camera.main.transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);

    }

    private void LookForInteractive() {
        // Raycast ahead of camera [actionDistance] units to find interactive objects
        RaycastHit actionHit;
        Physics.Raycast(
            new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z
            ),
            Camera.main.transform.forward,
            out actionHit,
            actionDistance,
            actionMask
        );

        // Debug draw look vector
        if (displayActionRaycast) {
            Debug.DrawRay(
                new Vector3(
                    Camera.main.transform.position.x,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z
                ),
                Camera.main.transform.forward * actionDistance,
                Color.green
            );
        }

        // If an interactive object is found: make UI visible, grab object's interactive script, set UI text
        if (actionHit.collider != null) {
            if (visibleObject == null) {
                visibleObject = actionHit.collider.gameObject.GetComponent<IInteractive>();
            }

            if (!string.IsNullOrEmpty(visibleObject.Info())) {
                isViewingAction = true;

                actionText.text = visibleObject.Info();
            }
            else {
                isViewingAction = false;
            }
        }
        // Else make UI invisible and set object's interactive script to null
        else {
            isViewingAction = false;
            visibleObject = null;
        }
    }

    public void Activate() {
        // If there is a visible object, set that as the active object and Activate() it
        if (visibleObject != null) {
            visibleObject.Activate(this);
        }
    }

    // Move player to target position
    public void Teleport(Vector3 targetPosition) {
        transform.position = targetPosition;
    }

    // ...jump
    public void Jump() 
    {
        if (isGrounded) 
        {
            rb.drag = 0.0f;
            rb.AddForce(transform.up * jumpForce);
        }
    }

    // Handle damage to player health
    public void DamageHealth(float damage) {
        // Absolute value -- this function doesn't allow for healing damage
        Mathf.Abs(damage);

        currentHealth -= damage;
        timeSinceLastHit = 0.0f;
    }

    // Used by PlayerInput
    public float HorizontalValue 
    {
        set { horizontalAxisValue = value * speed; }
    }

    // Used by PlayerInput
    public float VerticalValue 
    {
        set { verticalAxisValue = value * speed; }
    }

    // Used by PlayerInput
    public float RotateX 
    {
        set 
        { 
            float x = xRotate;
            x += -value * 5.0f;
            xRotate = Mathf.Clamp(x, -89.0f, 89.0f);
        }
    }

    // Used by PlayerInput
    public float RotateY 
    {
        set { yRotate += value * 5.0f; }
    }

    public IInteractive VisibleObject {
        set { visibleObject = value; }
    }

    // Used by SaveSystem
    public float CurrentHealth {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
}
