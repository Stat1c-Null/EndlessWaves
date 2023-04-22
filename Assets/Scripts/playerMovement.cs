using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;
    [Header("Keyboard key")]
    public KeyCode jumpKey  = KeyCode.Space;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        //viewDirec = ThirdPersCamera.GetComponent<ThirdPersonCamera>().viewDirec;
        //Debug.Log(viewDirec);
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + 0.2f, ground);
        //Apply drag
        if (grounded) {
            Debug.Log("Test");
            rb.drag = 0;
        } else {
            rb.drag = groundDrag;
            
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Update is called once per frame
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump
        if(Input.GetKey(jumpKey) && readyToJump) {//Also chec k if grounded later on
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MovePlayer()
    {
        //transform.rotation = Quaternion.Euler(orientation.eulerAngles).normalized;
        // calc movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //on ground
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f,ForceMode.Force);

        //In air 

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
