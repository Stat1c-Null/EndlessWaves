using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    //VARIABLES
    public float movementSpeed;
    public GameObject camera;

    public GameObject PlayerObj;
    private Vector3 jump;
    public float jumpForce = 2.0f;
    public bool IsGrounded;
    Rigidbody rb;

    public GameObject bulletSpawnPoint;
    public float waitTime;
    public GameObject bullet;
    public float points;
    public float health = 100f;
    public float enemyDamage = 0.1f;
    private bool touchingEnemy = false;

    //METHODS
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
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

        //Jumping
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            IsGrounded = false;
        }

        //Shooting
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shoot");
            Shoot();
        }

        //Decrease health if colliding with enemy
        if(touchingEnemy) 
        {
            health -= enemyDamage;
        }
    }

    void Shoot()
    {
        Instantiate(bullet.transform, bulletSpawnPoint.transform.position, PlayerObj.transform.rotation);
    }



    void OnCollisionStay(Collision collision)
    {
        //Check if colliding with enemy
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Colliding with enemy");
            touchingEnemy = true;
        }
        //Check if colliding with ground
        if(collision.gameObject.tag == "Ground")
        {
            IsGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        //Check if player stopped colliding with enemy
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Stopped Colliding with enemy");
            touchingEnemy = false;
        }
    }
}
