using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    private GameObject ThirdPersCamera;
    Vector3 viewDirec;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ThirdPersCamera = GameObject.FindWithTag("Main Camera");
    }

    private void Update()
    {
        MyInput();
        //viewDirec = ThirdPersCamera.GetComponent<ThirdPersonCamera>().viewDirec;
        //Debug.Log(viewDirec);
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
    }
    private void MovePlayer()
    {
        //transform.rotation = Quaternion.Euler(orientation.eulerAngles).normalized;
        // calc movement direction
        
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f,ForceMode.Force);
    }
}
