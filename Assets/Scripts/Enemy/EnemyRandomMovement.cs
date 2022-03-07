using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomMovement : MonoBehaviour
{
    public float movementSpeed = 6f;
    public float rotationSpeed = 10f;
    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    private bool chasing = false;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Detect if chasing player or no
        chasing = gameObject.GetComponent<EnemyFollow>().chasing;
        if(chasing == false){

            if(isWandering == false)
            {
                StartCoroutine(Wander());
            }
            if (isRotatingRight)
            {
                transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
            }
            if (isRotatingLeft)
            {
                transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
            }
            if (isWalking)
            {
                //rb.AddForce(transform.forward * movementSpeed);
                transform.Translate(transform.forward * Time.deltaTime * movementSpeed);
            }
        }
        
    }

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1,3);
        int rotateWait = Random.Range(1,3);
        int rotateDirection = Random.Range(1, 2);//Choose to rotate either left or right
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 5);
        //Start movement
        isWandering = true;
        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        //Chose rotation direction
        if(rotateDirection == 1)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        } else {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }

        isWandering = false;
    }
}
