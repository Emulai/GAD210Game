using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb = null;
    private float horizontalAxisValue = 0.0f;
    private float verticalAxisValue = 0.0f;
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float jumpForce = 250.0f;
    private float xRotate = 0.0f;
    private float yRotate = 0.0f;
    private bool isGrounded = false;
    public bool onRamp = false;
    [SerializeField]
    public LayerMask mask = (1 << 8) | (1 << 9);

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

    void FixedUpdate() {

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

        Debug.DrawLine(
            new Vector3(
                transform.position.x, 
                transform.position.y, 
                transform.position.z),
            new Vector3(
                transform.position.x, 
                transform.position.y - 1.1f, 
                transform.position.z
            ),
            Color.red
        );

        if (hit.collider != null) {
            int layer = hit.collider.gameObject.layer;
            Debug.Log(layer);
            if (layer == 8 ||
                layer == 9) 
            {
                isGrounded = true;
                if (hit.collider.tag == "Ramp") {
                    onRamp = true;
                }
                else {
                    onRamp = false;
                }
            }
        }
        else {
            isGrounded = false;
            onRamp = false;
        }

        // And not grounded
        if (verticalAxisValue == 0.0f 
            && horizontalAxisValue == 0.0f
            && isGrounded) 
        {
            rb.drag = 10.0f;
        }
        else {
            rb.drag = 0.0f;
        }

        Vector3 fForward = transform.forward * verticalAxisValue;
        Vector3 fRight = transform.right * horizontalAxisValue;

        if (onRamp && (verticalAxisValue != 0.0f || horizontalAxisValue != 0.0f)) {
            Vector3 rampFactor = new Vector3(0, 2, 0);
            fForward += rampFactor;
            fRight += rampFactor;
        }

        Vector3 fDirectional = fForward + fRight;
        Debug.DrawRay(transform.position, fDirectional, Color.blue);

        // rb.AddForce(fForward);
        // rb.AddForce(fRight);
        rb.AddForce(fDirectional);

        transform.rotation = Quaternion.Euler(0, yRotate, 0);
        Camera.main.transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);

    }

    public void Jump() {
        if (isGrounded) {
            rb.drag = 0.0f;
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public float HorizontalValue {
        set { horizontalAxisValue = value * speed; }
    }

    public float VerticalValue {
        set { verticalAxisValue = value * speed; }
    }

    public float RotateX {
        set { 
                float x = xRotate;
                x += -value * 5.0f;
                xRotate = Mathf.Clamp(x, -89.0f, 89.0f);
        }
    }

    public float RotateY {
        set { yRotate += value * 5.0f; }
    }
}
