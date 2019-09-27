using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float jumpForce = 250.0f;
    [SerializeField]
    private LayerMask mask = (1 << 8) | (1 << 9);

    [Header("Debug Variables")]
    [SerializeField]
    private bool displayMoveVector = false;

    private Rigidbody rb = null;
    private float horizontalAxisValue = 0.0f;
    private float verticalAxisValue = 0.0f;
    private float xRotate = 0.0f;
    private float yRotate = 0.0f;
    private bool isGrounded = false;
    private bool onRamp = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Move to game controller
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() 
    {
        Vector3 fDirectional = new Vector3();

        RaycastHit hit;
        Physics.Raycast(
            new Vector3(
                transform.position.x, 
                transform.position.y, 
                transform.position.z), 
            -transform.up, 
            out hit,
            1.1f, 
            mask);

        if (hit.collider != null) 
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == 8 ||
                layer == 9) 
            {
                isGrounded = true;
                if (hit.collider.tag == "Ramp") 
                {
                    onRamp = true;
                }
                else if (hit.collider.tag == "Platform")
                {
                    transform.SetParent(hit.collider.transform);
                }
                else 
                {
                    onRamp = false;
                    transform.SetParent(null);
                }
            }
        }
        else 
        {
            isGrounded = false;
            onRamp = false;
            transform.SetParent(null);
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
}
