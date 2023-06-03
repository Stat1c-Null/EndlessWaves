using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walk_speed;
    public float sprint_speed;
    public Transform orientation;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    bool grounded;
    private bool sprinting = false;
    [Header("Crouching")]
    public float crouch_speed;
    public float crouchYScale;
    float startYScale;

    [Header("Keyboard key")]
    public KeyCode jumpKey  = KeyCode.Space;
    public KeyCode sprintKey  = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Stats")]
    public float health = 100f;
    private float maxHealth = 100f;
    public float restoredHealth = 25f;
    private float reloadedHealth;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float MovingStaminaRegen = 5f;
    public float NotMovingStaminaRegen = 10f;
    public float staminaConsum = 10f;
    public float enemyDamage = 0.1f;
    public float thirstStat = 100f;
    public float maxThirstStat = 100f;
    public float hungerStat = 100f;
    public float maxHungerStat = 100f;
    public float hungerDec = 0.1f;
    public float thirstDec = 0.3f;
    [Header("UI")]
    public Text currentAmmoText;
    public Text maxAmmoText;
    public Text healthText;
    public Text thirstText;
    public Text hungerText;
    public Image StaminUi;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    public MoveState state;

    public enum MoveState 
    {
        walk,
        sprint,
        crouch,
        air
    }
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        healthText.text = health + " H";
        thirstText.text = thirstStat + "TH";
        hungerText.text = hungerStat + "H";

        //Get players original size
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        HungerThirstUpdate();
        StateHandler();
        //Apply drag
        if (grounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0;
        }

        //Update UI
        healthText.text = health.ToString("0") + " H";//Remove decimal numbers at the end of health
        thirstText.text = thirstStat.ToString("0") + "TH";
        hungerText.text = hungerStat.ToString("0") + "H";
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
        if(Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Start Crouching
        if(Input.GetKey(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            //Push player to the ground 
            //rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);// ForceMode.Impulse does a little physics push
        }
        
        //Stop Crouching
        if (Input.GetKeyUp(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

    }
    void StateHandler()
    {
        //Mode Sprinting
        if(grounded && Input.GetKey(sprintKey))
        {
            state = MoveState.sprint;
            moveSpeed = sprint_speed;
        }

        //Mode Walking
        else if(grounded)
        {
            state = MoveState.walk;
            moveSpeed = walk_speed;
        }

        //Mode Crouching
        else if(Input.GetKey(crouchKey))
        {
            state = MoveState.crouch;
            moveSpeed = sprint_speed;
        }

        //Mode Air
        else {
            state = MoveState.air;
        }
    }

    private void MovePlayer()
    {
        //transform.rotation = Quaternion.Euler(orientation.eulerAngles).normalized;
        // calc movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //on ground
        if (grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f,ForceMode.Force);
        }
        //In air 
        else if (!grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
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

    void OnCollisionEnter(Collision collision)
    {
        //Check collision with food and water
        if(collision.gameObject.tag == "Food")
        {
            hungerStat += Random.Range(10, 25);
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Water")
        {
            thirstStat += Random.Range(15, 30);
            Destroy(collision.gameObject);
        }
    }

    // ground check
    void OnCollisionStay(Collision collision) 
    {
        if(collision.gameObject.tag == "Ground") {
            grounded = true;
        }

        //Check for collision with ammo or health box
        if(collision.gameObject.tag == "HealthBox" && health < maxHealth) 
        {
            reloadedHealth = maxHealth - health;
            //Check if amount of health needed to restore is less than 25, so then we can just give player max hp
            if(reloadedHealth < restoredHealth)
            {
                health = maxHealth;
                Destroy(collision.gameObject);
            //If health is less than 25, we just gonna add 25 to the health
            } else if(reloadedHealth > restoredHealth) {
                health += restoredHealth;
                Destroy(collision.gameObject);
            }
        }
    }

    void OnCollisionExit(Collision collision) 
    {
        if(collision.gameObject.tag == "Ground") {
            grounded = false;
        }
    }

    void HungerThirstUpdate()
    {
        thirstStat -= thirstDec * Time.deltaTime;
        hungerStat -= hungerDec * Time.deltaTime;
        if(thirstStat > maxThirstStat)
        {
            thirstStat = maxThirstStat;
        }
        if(hungerStat > maxHungerStat)
        {
            hungerStat = maxHungerStat;
        }
    }
}
