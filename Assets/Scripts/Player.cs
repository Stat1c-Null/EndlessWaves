using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //VARIABLES
    public float movementSpeed;
    public GameObject camera;

    public GameObject PlayerObj;

    public GameObject bulletSpawnPoint;
    public float waitTime;
    public GameObject bullet;

    //METHODS
    // Start is called before the first frame update
    void Start()
    {
        
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

        //Shooting
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shoot");
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
    }
}
