using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        actionText.text = "";

        // Move to game controller
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (isViewingAction) {
            actionIcon.gameObject.SetActive(true);
            actionText.gameObject.SetActive(true);
        }
        else {
            actionIcon.gameObject.SetActive(false);
            actionText.text = "";
            actionText.gameObject.SetActive(false);
        }
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

        Vector3 fForward = transform.forward * verticalAxisValue;
        Vector3 fRight = transform.right * horizontalAxisValue;

        if (onRamp && (verticalAxisValue != 0.0f || horizontalAxisValue != 0.0f)) 
        {
            Vector3 rampFactor = new Vector3(0, 2, 0);
            fForward += rampFactor;
            fRight += rampFactor;
        }

        fDirectional += fForward + fRight;
        if (displayMoveVector) 
        {
            Debug.DrawRay(transform.position, fDirectional, Color.blue);
        }

        fDirectional.y = rb.velocity.y;
        rb.velocity = fDirectional;

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
            isViewingAction = true;

            if (visibleObject == null) {
                visibleObject = actionHit.collider.gameObject.GetComponent<IInteractive>();
            }

            actionText.text = visibleObject.Info();
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

    public void Teleport(Vector3 targetPosition) {
        transform.position = targetPosition;
    }

    public void Jump() 
    {
        if (isGrounded) 
        {
            rb.drag = 0.0f;
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public float HorizontalValue 
    {
        set { horizontalAxisValue = value * speed; }
    }

    public float VerticalValue 
    {
        set { verticalAxisValue = value * speed; }
    }

    public float RotateX 
    {
        set 
        { 
            float x = xRotate;
            x += -value * 5.0f;
            xRotate = Mathf.Clamp(x, -89.0f, 89.0f);
        }
    }

    public float RotateY 
    {
        set { yRotate += value * 5.0f; }
    }

    public IInteractive VisibleObject {
        set { visibleObject = value; }
    }
}
