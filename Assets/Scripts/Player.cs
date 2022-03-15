using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    //VARIABLES
    [Header("Movement")]
    public float movementSpeed;
    private float ogMoveSpeed;
    public float runSpeed;
    public float jumpForce = 2.0f;
    [HideInInspector]public bool IsGrounded;
    [Header("Game Objects")]
    public GameObject camera;
    public GameObject PlayerObj;
    private Vector3 jump;
    private bool moving = false;
    private bool sprinting = false;
    [HideInInspector] Rigidbody rb;
    public GameObject bulletSpawnPoint;
    public float waitTime;
    public GameObject bullet;
    //Stats
    [Header("Stats")]
    public float health = 100f;
    private float maxHealth = 100f;
    private float reloadedHealth;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float MovingStaminaRegen = 5f;
    public float NotMovingStaminaRegen = 10f;
    public float staminaConsum = 10f;
    public float enemyDamage = 0.1f;
    [HideInInspector] public float points;
    private bool touchingEnemy = false;
    //Ammo
    [Header("Ammo")]
    public int maxHandgunAmmo = 100;
    public int currentHandgunAmmo = 100;
    public int maxHandgunClipAmmo = 11;
    public int currentHandgunClipAmmo = 11;
    private int reloadedAmmo;
    public float restoredHealth = 25f;
    public int restoredAmmo = 30;
    //UI
    [Header("UI")]
    public Text currentAmmoText;
    public Text maxAmmoText;
    public Text healthText;
    public Image StaminUi;

    //METHODS
    // Start is called before the first frame update
    void Start()
    {
        ogMoveSpeed = movementSpeed;
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        //Assign variable to text
        currentAmmoText.text = currentHandgunClipAmmo + "/" + maxHandgunClipAmmo + " C";
        maxAmmoText.text = currentHandgunAmmo + "/" + maxHandgunAmmo + " M";
        healthText.text = health + " H";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        //Player follow the mouse
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;
        
        if(playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0;//Dont rotate vertically 
            targetRotation.z = 0;
            PlayerObj.transform.rotation = Quaternion.Slerp(PlayerObj.transform.rotation, targetRotation, 4f * Time.deltaTime);
        }

        //Player Movement
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
        } 
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
        }
        //Detect if player not moving
        if(!(Input.GetKey(KeyCode.S)) && !(Input.GetKey(KeyCode.W)) && !(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D))) 
        {
            moving = false;
        } else {
            moving = true;
        }
        //Sprinting
        if(Input.GetKey(KeyCode.LeftShift) && stamina > 0 && moving == true)//Running
        {
            movementSpeed = runSpeed;
            stamina -= staminaConsum * Time.deltaTime;
            sprinting = true;
        } else if(sprinting == false && stamina < maxStamina && moving == true) {//Moving but stamina is not full, regen slowly
            stamina += MovingStaminaRegen * Time.deltaTime;
            movementSpeed = ogMoveSpeed;
        } else if(sprinting == false && stamina < maxStamina && moving == false) {//Not moving but stamina is not full, regen quickly
            stamina += NotMovingStaminaRegen * Time.deltaTime;
            movementSpeed = ogMoveSpeed;
        } 
        else//Not running but stamina is full
        {
            movementSpeed = ogMoveSpeed;
        }
        //Not sprinting
        if(!(Input.GetKey(KeyCode.LeftShift))) 
        {
            sprinting = false;
        }
        //Set stamina too max if it's too big
        if(stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        //Jumping
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            IsGrounded = false;
        }

        //Shooting
        if(Input.GetMouseButtonDown(0) && currentHandgunClipAmmo > 0)
        {
            currentHandgunClipAmmo -= 1;//Remove ammo
            Shoot();
        }

        //Reload
        if(Input.GetKeyDown(KeyCode.R) && currentHandgunClipAmmo < maxHandgunClipAmmo)
        {
            reloadedAmmo = maxHandgunClipAmmo - currentHandgunClipAmmo;
            if(currentHandgunAmmo >= reloadedAmmo)
            {
                currentHandgunAmmo -= reloadedAmmo;//Take ammo away from total
                currentHandgunClipAmmo += reloadedAmmo;//Put ammo in the clip
            }  else if(currentHandgunAmmo < reloadedAmmo && currentHandgunAmmo > 0)
            {
                currentHandgunClipAmmo += currentHandgunAmmo;
                currentHandgunAmmo = 0;
            }
            else
            {
                Debug.Log("No ammo in backpack");
            }
        }

        //Decrease health if colliding with enemy
        if(touchingEnemy) 
        {
            health -= enemyDamage;
        }
        //Kill Player
        if(health <= 0) {
            Destroy(this.gameObject);
        }
        //Update UI
        currentAmmoText.text = currentHandgunClipAmmo + "/" + maxHandgunClipAmmo + " C";
        maxAmmoText.text = currentHandgunAmmo + "/" + maxHandgunAmmo + " M";
        healthText.text = health.ToString("0") + " H";//Remove decimal numbers at the end of health
    }

    void Shoot()
    {
        Instantiate(bullet.transform, bulletSpawnPoint.transform.position, PlayerObj.transform.rotation);
    }

    void UpdateUI()
    {
        //Update Stamina UI
        StaminUi.fillAmount = stamina / maxStamina;
    }


    void OnCollisionStay(Collision collision)
    {
        //Check if colliding with enemy
        if(collision.gameObject.tag == "Enemy")
        {
            touchingEnemy = true;
        }
        //Check if colliding with enemy
        if(collision.gameObject.tag == "EnemyRandom")
        {
            touchingEnemy = true;
        }
        //Check if colliding with ground
        if(collision.gameObject.tag == "Ground")
        {
            IsGrounded = true;
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
        if(collision.gameObject.tag == "AmmoBox" && currentHandgunAmmo < maxHandgunAmmo) 
        {
            reloadedAmmo = maxHandgunAmmo - currentHandgunAmmo;
            //Check if amount of health needed to restore is less than 25, so then we can just give player max hp
            if(reloadedAmmo < restoredAmmo)
            {
                currentHandgunAmmo = maxHandgunAmmo;
                Destroy(collision.gameObject);
            //If health is less than 25, we just gonna add 25 to the health
            } else if(reloadedAmmo > restoredAmmo) {
                currentHandgunAmmo += restoredAmmo;
                Destroy(collision.gameObject);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        //Check if player stopped colliding with enemy
        if(collision.gameObject.tag == "Enemy")
        {
            touchingEnemy = false;
        }
         //Check if player stopped colliding with enemy
        if(collision.gameObject.tag == "EnemyRandom")
        {
            touchingEnemy = false;
        }
    }
}
