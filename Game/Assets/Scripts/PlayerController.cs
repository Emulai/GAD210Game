using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _jumpForce = 250.0f;
    [SerializeField]
    private LayerMask _mask = (1 << 8) | (1 << 9);

    [Header("Debug Variables")]
    [SerializeField]
    private bool _displayMoveVector = false;

    private Rigidbody _rb = null;
    private float _horizontalAxisValue = 0.0f;
    private float _verticalAxisValue = 0.0f;
    private float _xRotate = 0.0f;
    private float _yRotate = 0.0f;
    private bool _isGrounded = false;
    private bool _onRamp = false;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

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
            _mask);

        if (hit.collider != null) 
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == 8 ||
                layer == 9) 
            {
                _isGrounded = true;
                if (hit.collider.tag == "Ramp") 
                {
                    _onRamp = true;
                }
                else if (hit.collider.tag == "Platform")
                {
                    transform.SetParent(hit.collider.transform);
                }
                else 
                {
                    _onRamp = false;
                    transform.SetParent(null);
                }
            }
        }
        else 
        {
            _isGrounded = false;
            _onRamp = false;
            transform.SetParent(null);
        }

        Vector3 fForward = transform.forward * _verticalAxisValue;
        Vector3 fRight = transform.right * _horizontalAxisValue;

        if (_onRamp && (_verticalAxisValue != 0.0f || _horizontalAxisValue != 0.0f)) 
        {
            Vector3 rampFactor = new Vector3(0, 2, 0);
            fForward += rampFactor;
            fRight += rampFactor;
        }

        fDirectional += fForward + fRight;
        if (_displayMoveVector) 
        {
            Debug.DrawRay(transform.position, fDirectional, Color.blue);
        }

        fDirectional.y = _rb.velocity.y;
        _rb.velocity = fDirectional;

        transform.rotation = Quaternion.Euler(0, _yRotate, 0);
        Camera.main.transform.rotation = Quaternion.Euler(_xRotate, _yRotate, 0);

    }

    public void Jump() 
    {
        if (_isGrounded) 
        {
            _rb.drag = 0.0f;
            _rb.AddForce(transform.up * _jumpForce);
        }
    }

    public float HorizontalValue 
    {
        set { _horizontalAxisValue = value * _speed; }
    }

    public float VerticalValue 
    {
        set { _verticalAxisValue = value * _speed; }
    }

    public float RotateX 
    {
        set 
        { 
            float x = _xRotate;
            x += -value * 5.0f;
            _xRotate = Mathf.Clamp(x, -89.0f, 89.0f);
        }
    }

    public float RotateY 
    {
        set { _yRotate += value * 5.0f; }
    }
}
