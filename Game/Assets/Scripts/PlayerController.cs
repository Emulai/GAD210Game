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
    private float xRotate = 0.0f;
    private float yRotate = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {

        Vector3 fForward = transform.forward * verticalAxisValue;
        Vector3 fRight = transform.right * horizontalAxisValue;
        
        // Debug.Log(f);
        rb.AddForce(fForward * speed);
        rb.AddForce(fRight * speed);

        // transform.position += transform.forward * verticalAxisValue * speed;
        // transform.position += transform.right * horizontalAxisValue * speed;

        transform.rotation = Quaternion.Euler(0, yRotate, 0);
        Camera.main.transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);

    }

    public float HorizontalValue {
        set { horizontalAxisValue = value; }
    }

    public float VerticalValue {
        set { verticalAxisValue = value; }
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
